using AutoMapper;
using Wastelands.Core.Contracts.Contracts;
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
	/// <summary>
	/// Смотреть карту боя могут все, кому виден сам бой (скоуп BattleRepository): мастер — всегда, игрок —
	/// только пока бой идёт и в нём есть его персонаж. Менять (подключать карту, расставлять) — только мастер.
	/// Маркеры карты — заметки мастера: игрокам отдаются только отмеченные "видно игрокам".
	/// </summary>
	public class BattleMapPlacementService : IBattleMapPlacementService
	{
		private readonly IBattleRepository _battleRepository;
		private readonly IBattleMapRepository _battleMapRepository;
		private readonly ICreatureTemplateRepository _creatureTemplateRepository;
		private readonly IGameAccessGuard _gameAccessGuard;
		private readonly IBattleNotifier _battleNotifier;
		private readonly IUserContext _userContext;
		private readonly IMapper _mapper;

		public BattleMapPlacementService(
			IBattleRepository battleRepository,
			IBattleMapRepository battleMapRepository,
			ICreatureTemplateRepository creatureTemplateRepository,
			IGameAccessGuard gameAccessGuard,
			IBattleNotifier battleNotifier,
			IUserContext userContext,
			IMapper mapper)
		{
			_battleRepository = battleRepository;
			_battleMapRepository = battleMapRepository;
			_creatureTemplateRepository = creatureTemplateRepository;
			_gameAccessGuard = gameAccessGuard;
			_battleNotifier = battleNotifier;
			_userContext = userContext;
			_mapper = mapper;
		}

		public async Task<BattleMapViewDto> GetMapViewAsync(long battleId)
		{
			// Скоуп BattleRepository сам отсекает чужих и игроков до начала боя — для них это 404.
			var battle = await _battleRepository.GetByIdAsync(battleId);
			NotFoundException.ThrowIfNull(battle, ErrorCode.BattleNotFound, nameof(Battle), nameof(Battle.Id), battleId.ToString());

			var isGm = await _gameAccessGuard.IsOwnerAsync(battle.GameId);

			// Репозиторий карт скоупит по мастеру — игроку карта нужна через бой, отсюда unscoped.
			var battleMap = battle.BattleMapId is not { } mapId ? null
				: isGm ? await _battleMapRepository.GetByIdAsync(mapId)
				: await _battleMapRepository.GetByIdUnscopedAsync(mapId);

			return await ToViewAsync(battle, battleMap, isGm);
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

			return await ToViewAsync(battle, battleMap, isGm: true);
		}

		private async Task<BattleMapViewDto> ToViewAsync(Battle battle, BattleMap? battleMap, bool isGm)
		{
			// Картинки существ живут в шаблонах — одним запросом на все шаблоны, встречающиеся в бою
			// (unscoped: аватарки существ видят и игроки).
			var templateIds = battle.Creatures.Select(c => c.CreatureTemplateId).Distinct().ToList();
			var imageKeyByTemplateId = templateIds.Count == 0
				? []
				: await _creatureTemplateRepository.GetImageKeysUnscopedAsync(templateIds);
			var currentUserId = _userContext.CurrentUserId;

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
					ControlledByCurrentUser = isGm,
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
					ControlledByCurrentUser = bc.Character.UserId == currentUserId,
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

			// Игрокам — только маркеры, которые мастер явно открыл (MarkerVisibleToPlayers); остальные —
			// его заметки, их текст не должен уходить с сервера.
			var mapDto = battleMap is null ? null : _mapper.Map<BattleMapDto>(battleMap);
			if (mapDto is not null && !isGm)
			{
				foreach (var hex in mapDto.Hexes.Where(h => !h.MarkerVisibleToPlayers))
				{
					hex.MarkerText = null;
				}
			}

			return new BattleMapViewDto
			{
				BattleId = battle.Id,
				BattleName = battle.Name,
				Status = battle.Status,
				CurrentInitiative = battle.CurrentInitiative,
				CanEdit = isGm,
				Map = mapDto,
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
