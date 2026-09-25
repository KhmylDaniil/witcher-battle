using Wastelands.Core.Contracts.Constants;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Services
{
	/// <summary>
	/// Создание боя, состав участников и их состояния, старт боя. Процесс самих атак — в
	/// <see cref="BattleCombatService"/>; обе стороны используют один и тот же <see cref="IBattleDtoMapper"/>,
	/// чтобы BattleDto (включая вложенный Attack) выглядел одинаково независимо от того, какой из
	/// сервисов его вернул.
	/// </summary>
	public class BattleService : IBattleService
	{
		private readonly IBattleRepository _battleRepository;
		private readonly ICreatureTemplateRepository _creatureTemplateRepository;
		private readonly ICharacterRepository _characterRepository;
		private readonly IGameAccessGuard _gameAccessGuard;
		private readonly IBattleNotifier _battleNotifier;
		private readonly IBattleDtoMapper _dtoMapper;
		private readonly IBattleTurnProcessor _turnProcessor;

		public BattleService(
			IBattleRepository battleRepository,
			ICreatureTemplateRepository creatureTemplateRepository,
			ICharacterRepository characterRepository,
			IGameAccessGuard gameAccessGuard,
			IBattleNotifier battleNotifier,
			IBattleDtoMapper dtoMapper,
			IBattleTurnProcessor turnProcessor)
		{
			_battleRepository = battleRepository;
			_creatureTemplateRepository = creatureTemplateRepository;
			_characterRepository = characterRepository;
			_gameAccessGuard = gameAccessGuard;
			_battleNotifier = battleNotifier;
			_dtoMapper = dtoMapper;
			_turnProcessor = turnProcessor;
		}

		public async Task<BattleDto> GetBattleByIdAsync(long id)
		{
			var battle = await GetByIdAsync(id);
			return await _dtoMapper.MapAsync(battle);
		}

		public async Task<List<BattleDto>> GetBattlesAsync(BattleFilter filter)
		{
			var battles = await _battleRepository.GetListByFilterAsync(filter);
			var dtos = new List<BattleDto>();
			foreach (var battle in battles)
			{
				dtos.Add(await _dtoMapper.MapAsync(battle));
			}

			return dtos;
		}

		public async Task<BattleDto> CreateBattleAsync(CreateBattleRequest request)
		{
			var game = await _gameAccessGuard.GetGameOwnedByCurrentUserAsync(request.GameId);
			var battle = new Battle(game.Id, request.Name);

			await _battleRepository.CreateAsync(battle);

			return await _dtoMapper.MapAsync(battle);
		}

		/// <summary>
		/// Удаление боя — единственный момент, когда участие персонажа в начавшемся бою завершается
		/// (RemoveCharacterAsync разрешён только для черновика — см. ThrowIfNotDraft), поэтому здесь же
		/// синхронизируем накопленное в бою CurrentHP обратно на персонажей-участников.
		/// </summary>
		public async Task DeleteBattleAsync(long id)
		{
			var battle = await GetByIdForGmAsync(id);

			foreach (var battleCharacter in battle.Characters)
			{
				var character = await _characterRepository.GetByIdUnscopedAsync(battleCharacter.CharacterId);
				if (character is not null)
				{
					character.SyncCurrentHpFromBattle(battleCharacter.CurrentHP);
					await _characterRepository.UpdateAsync(character);
				}
			}

			await _battleRepository.DeleteAsync(battle);
		}

		/// <summary>Добавить существо можно и в уже идущий бой — без броска инициативы, в конец очереди (см. Battle.AddCreature).</summary>
		public async Task<BattleDto> AddCreatureAsync(AddCreatureToBattleRequest request)
		{
			var battle = await GetByIdForGmAsync(request.BattleId);

			var creatureTemplate = await _creatureTemplateRepository.GetByIdAsync(request.CreatureTemplateId);
			NotFoundException.ThrowIfNull(
				creatureTemplate, ErrorCode.CreatureTemplateNotFound, nameof(CreatureTemplate), nameof(CreatureTemplate.Id), request.CreatureTemplateId.ToString());

			if (creatureTemplate.GameId != battle.GameId)
			{
				throw new InvalidArgumentException(ErrorCode.CreatureTemplateBelongsToAnotherGame, "Выбранный шаблон существа принадлежит другой игре.");
			}

			var creature = new Creature(battle.Id, creatureTemplate, request.Name);
			var startsTurnNow = battle.AddCreature(creature);
			await OnParticipantJoinedAsync(battle, creature.Name, creature.Initiative, startsTurnNow);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> UpdateCreatureAsync(UpdateBattleCreatureRequest request)
		{
			var battle = await GetByIdForGmAsync(request.BattleId);
			var creature = BattleParticipants.GetCreature(battle, request.CreatureId);

			creature.UpdateState(request.Name, request.CurrentHP, request.CurrentSta);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> RemoveCreatureAsync(long battleId, long creatureId)
		{
			var battle = await GetByIdForGmAsync(battleId);
			ThrowIfNotDraft(battle);
			var creature = BattleParticipants.GetCreature(battle, creatureId);

			battle.Creatures.Remove(creature);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> AddCreatureConditionAsync(long battleId, long creatureId, Condition condition)
		{
			var battle = await GetByIdForGmAsync(battleId);
			BattleParticipants.GetCreature(battle, creatureId).AddCondition(condition);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> RemoveCreatureConditionAsync(long battleId, long creatureId, Condition condition)
		{
			var battle = await GetByIdForGmAsync(battleId);
			BattleParticipants.GetCreature(battle, creatureId).RemoveCondition(condition);

			return await SaveAndNotifyAsync(battle);
		}

		/// <summary>Добавить персонажа можно и в уже идущий бой — без броска инициативы, в конец очереди (см. Battle.AddCharacter).</summary>
		public async Task<BattleDto> AddCharacterAsync(AddCharacterToBattleRequest request)
		{
			var battle = await GetByIdForGmAsync(request.BattleId);

			if (battle.Characters.Any(x => x.CharacterId == request.CharacterId))
			{
				throw new InvalidArgumentException(
					ErrorCode.BattleCharacterAlreadyExisted,
					string.Format(ExceptionMessages.ValueMustBeUnique, nameof(request.CharacterId)));
			}

			var character = await _characterRepository.GetByIdUnscopedAsync(request.CharacterId);
			NotFoundException.ThrowIfNull(character, ErrorCode.CharacterNotFound, nameof(Character), nameof(Character.Id), request.CharacterId.ToString());

			if (character.GameId != battle.GameId)
			{
				throw new InvalidArgumentException(ErrorCode.CharacterBelongsToAnotherGame, "Выбранный персонаж принадлежит другой игре.");
			}

			var battleCharacter = new BattleCharacter(battle.Id, character);
			var startsTurnNow = battle.AddCharacter(battleCharacter);
			await OnParticipantJoinedAsync(battle, character.Name, battleCharacter.Initiative, startsTurnNow);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> RemoveCharacterAsync(long battleId, long characterId)
		{
			var battle = await GetByIdForGmAsync(battleId);
			ThrowIfNotDraft(battle);
			var battleCharacter = BattleParticipants.GetBattleCharacter(battle, characterId);

			battle.Characters.Remove(battleCharacter);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> AddCharacterConditionAsync(long battleId, long characterId, Condition condition)
		{
			var battle = await GetByIdForGmAsync(battleId);
			BattleParticipants.GetBattleCharacter(battle, characterId).AddCondition(condition);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> RemoveCharacterConditionAsync(long battleId, long characterId, Condition condition)
		{
			var battle = await GetByIdForGmAsync(battleId);
			BattleParticipants.GetBattleCharacter(battle, characterId).RemoveCondition(condition);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> StartBattleAsync(long battleId)
		{
			var battle = await GetByIdForGmAsync(battleId);

			if (battle.Status != BattleStatus.Draft)
			{
				throw new InvalidArgumentException(ErrorCode.BattleAlreadyStarted, "Бой уже начат.");
			}

			if (battle.Creatures.Count == 0 && battle.Characters.Count == 0)
			{
				throw new InvalidArgumentException(ErrorCode.BattleHasNoParticipants, "В бою нет ни одного участника.");
			}

			BattleInitiativeRoller.RollInitiative(battle);
			battle.MarkStarted();
			await _turnProcessor.OnBattleStartedAsync(battle);

			return await SaveAndNotifyAsync(battle);
		}

		/// <summary>
		/// Вступление в уже идущий бой видно в логе; если бой шёл без участников и ход сразу перешёл к
		/// новому — обрабатываем начало его хода (эффекты состояний и т.п.), как при обычной передаче хода.
		/// </summary>
		private async Task OnParticipantJoinedAsync(Battle battle, string name, int? initiative, bool startsTurnNow)
		{
			if (battle.Status != BattleStatus.InProgress)
			{
				return;
			}

			battle.AddLogEntry($"{name} вступает в бой (инициатива {initiative}, без броска).");

			if (startsTurnNow)
			{
				await _turnProcessor.ProcessCurrentTurnAsync(battle);
			}
		}

		private static void ThrowIfNotDraft(Battle battle)
		{
			if (battle.Status != BattleStatus.Draft)
			{
				throw new InvalidArgumentException(ErrorCode.BattleAlreadyStarted, "Убирать участников из боя можно только до его начала.");
			}
		}

		private async Task<Battle> GetByIdAsync(long id)
		{
			var battle = await _battleRepository.GetByIdAsync(id);
			NotFoundException.ThrowIfNull(battle, ErrorCode.BattleNotFound, nameof(Battle), nameof(Battle.Id), id.ToString());

			return battle;
		}

		private async Task<Battle> GetByIdForGmAsync(long id)
		{
			var battle = await GetByIdAsync(id);
			await _gameAccessGuard.GetGameOwnedByCurrentUserAsync(battle.GameId);

			return battle;
		}

		private async Task<BattleDto> SaveAndNotifyAsync(Battle battle)
		{
			BattleParticipants.RemoveDeadCreatures(battle);
			await _battleRepository.UpdateAsync(battle);
			await _battleNotifier.NotifyBattleUpdatedAsync(battle.Id);
			return await _dtoMapper.MapAsync(battle);
		}
	}
}
