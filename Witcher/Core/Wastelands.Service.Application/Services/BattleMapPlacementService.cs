using AutoMapper;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Requests;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Services
{
	public class BattleMapPlacementService : IBattleMapPlacementService
	{
		private readonly IBattleRepository _battleRepository;
		private readonly IBattleMapRepository _battleMapRepository;
		private readonly ICreatureTemplateRepository _creatureTemplateRepository;
		private readonly IGameAccessGuard _gameAccessGuard;
		private readonly IBattleNotifier _battleNotifier;
		private readonly IMapper _mapper;

		public BattleMapPlacementService(
			IBattleRepository battleRepository,
			IBattleMapRepository battleMapRepository,
			ICreatureTemplateRepository creatureTemplateRepository,
			IGameAccessGuard gameAccessGuard,
			IBattleNotifier battleNotifier,
			IMapper mapper)
		{
			_battleRepository = battleRepository;
			_battleMapRepository = battleMapRepository;
			_creatureTemplateRepository = creatureTemplateRepository;
			_gameAccessGuard = gameAccessGuard;
			_battleNotifier = battleNotifier;
			_mapper = mapper;
		}

		public async Task<BattleMapViewDto> GetMapViewAsync(long battleId)
		{
			var battle = await GetBattleForGmAsync(battleId);
			var battleMap = battle.BattleMapId is { } mapId ? await _battleMapRepository.GetByIdAsync(mapId) : null;

			return await ToViewAsync(battle, battleMap);
		}

		public async Task<BattleMapViewDto> AttachMapAsync(AttachBattleMapRequest request)
		{
			var battle = await GetBattleForGmAsync(request.BattleId);
			var battleMap = request.BattleMapId is { } mapId ? await GetBattleMapAsync(mapId) : null;

			battle.AttachMap(battleMap);

			return await SaveAndNotifyAsync(battle, battleMap);
		}

		public async Task<BattleMapViewDto> PlaceParticipantAsync(PlaceParticipantOnMapRequest request)
		{
			var battle = await GetBattleForGmAsync(request.BattleId);
			if (battle.BattleMapId is not { } mapId)
			{
				throw new InvalidArgumentException(ErrorCode.BattleMapNotAttached, "К бою не подключена карта.");
			}

			var battleMap = await GetBattleMapAsync(mapId);
			battle.PlaceParticipantOnMap(request.Kind, request.ParticipantId, battleMap, request.Column, request.Row);

			return await SaveAndNotifyAsync(battle, battleMap);
		}

		public async Task<BattleMapViewDto> RemoveParticipantAsync(long battleId, ParticipantKind kind, long participantId)
		{
			var battle = await GetBattleForGmAsync(battleId);
			battle.RemoveParticipantFromMap(kind, participantId);

			var battleMap = battle.BattleMapId is { } mapId ? await _battleMapRepository.GetByIdAsync(mapId) : null;
			return await SaveAndNotifyAsync(battle, battleMap);
		}

		private async Task<BattleMapViewDto> SaveAndNotifyAsync(Battle battle, BattleMap? battleMap)
		{
			await _battleRepository.UpdateAsync(battle);
			await _battleNotifier.NotifyBattleUpdatedAsync(battle.Id);

			return await ToViewAsync(battle, battleMap);
		}

		private async Task<BattleMapViewDto> ToViewAsync(Battle battle, BattleMap? battleMap)
		{
			// Картинки существ живут в шаблонах — одним запросом на все шаблоны, встречающиеся в бою.
			var templateIds = battle.Creatures.Select(c => c.CreatureTemplateId).Distinct().ToList();
			var imageKeyByTemplateId = templateIds.Count == 0
				? []
				: (await _creatureTemplateRepository.GetByIdsAsync(templateIds)).ToDictionary(t => t.Id, t => t.ImageKey);

			var participants = battle.Creatures
				.Select(c => new BattleMapParticipantDto
				{
					Kind = ParticipantKind.Creature,
					Id = c.Id,
					Name = c.Name,
					ImageUrl = ToImageUrl(imageKeyByTemplateId.GetValueOrDefault(c.CreatureTemplateId)),
					CurrentHP = c.CurrentHP,
					MaxHP = c.MaxHP,
					Initiative = c.Initiative,
					Column = c.MapColumn,
					Row = c.MapRow,
				})
				.Concat(battle.Characters.Select(bc => new BattleMapParticipantDto
				{
					Kind = ParticipantKind.Character,
					Id = bc.CharacterId,
					Name = bc.Character.Name,
					ImageUrl = ToImageUrl(bc.Character.ImageKey),
					CurrentHP = bc.CurrentHP,
					MaxHP = bc.MaxHP,
					Initiative = bc.Initiative,
					Column = bc.MapColumn,
					Row = bc.MapRow,
				}))
				.OrderBy(p => p.Initiative ?? int.MaxValue)
				.ThenBy(p => p.Name)
				.ToList();

			// Карту могли уменьшить уже после расстановки — стоящих за новой границей показываем как
			// невыставленных (их можно выставить заново).
			foreach (var p in participants)
			{
				if (battleMap is null || p.Column is not { } column || p.Row is not { } row || battleMap.FindHex(column, row) is null)
				{
					p.Column = null;
					p.Row = null;
				}
			}

			return new BattleMapViewDto
			{
				BattleId = battle.Id,
				BattleName = battle.Name,
				Status = battle.Status,
				CurrentInitiative = battle.CurrentInitiative,
				Map = battleMap is null ? null : _mapper.Map<BattleMapDto>(battleMap),
				Participants = participants,
			};
		}

		private static string? ToImageUrl(string? imageKey) => imageKey is null ? null : $"/api/images/{imageKey}";

		private async Task<Battle> GetBattleForGmAsync(long battleId)
		{
			var battle = await _battleRepository.GetByIdAsync(battleId);
			NotFoundException.ThrowIfNull(battle, ErrorCode.BattleNotFound, nameof(Battle), nameof(Battle.Id), battleId.ToString());
			await _gameAccessGuard.GetGameOwnedByCurrentUserAsync(battle.GameId);

			return battle;
		}

		private async Task<BattleMap> GetBattleMapAsync(long battleMapId)
		{
			var battleMap = await _battleMapRepository.GetByIdAsync(battleMapId);
			NotFoundException.ThrowIfNull(battleMap, ErrorCode.BattleMapNotFound, nameof(BattleMap), nameof(BattleMap.Id), battleMapId.ToString());

			return battleMap;
		}
	}
}
