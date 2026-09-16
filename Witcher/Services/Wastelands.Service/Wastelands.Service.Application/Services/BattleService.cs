using AutoMapper;
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
	public class BattleService : IBattleService
	{
		private readonly IBattleRepository _battleRepository;
		private readonly ICreatureTemplateRepository _creatureTemplateRepository;
		private readonly ICharacterRepository _characterRepository;
		private readonly IGameAccessGuard _gameAccessGuard;
		private readonly IMapper _mapper;

		public BattleService(
			IBattleRepository battleRepository,
			ICreatureTemplateRepository creatureTemplateRepository,
			ICharacterRepository characterRepository,
			IGameAccessGuard gameAccessGuard,
			IMapper mapper)
		{
			_battleRepository = battleRepository;
			_creatureTemplateRepository = creatureTemplateRepository;
			_characterRepository = characterRepository;
			_gameAccessGuard = gameAccessGuard;
			_mapper = mapper;
		}

		public async Task<BattleDto> GetBattleByIdAsync(long id)
		{
			var battle = await GetByIdAsync(id);
			return _mapper.Map<BattleDto>(battle);
		}

		public async Task<List<BattleDto>> GetBattlesAsync(BattleFilter filter)
		{
			var battles = await _battleRepository.GetListByFilterAsync(filter);
			return _mapper.Map<List<BattleDto>>(battles);
		}

		public async Task<BattleDto> CreateBattleAsync(CreateBattleRequest request)
		{
			var game = await _gameAccessGuard.GetGameOwnedByCurrentUserAsync(request.GameId);
			var battle = new Battle(game.Id, request.Name);

			await _battleRepository.CreateAsync(battle);

			return _mapper.Map<BattleDto>(battle);
		}

		public async Task DeleteBattleAsync(long id)
		{
			var battle = await GetByIdForGmAsync(id);
			await _battleRepository.DeleteAsync(battle);
		}

		public async Task<BattleDto> AddCreatureAsync(AddCreatureToBattleRequest request)
		{
			var battle = await GetByIdForGmAsync(request.BattleId);
			ThrowIfNotDraft(battle);

			var creatureTemplate = await _creatureTemplateRepository.GetByIdAsync(request.CreatureTemplateId);
			NotFoundException.ThrowIfNull(
				creatureTemplate, ErrorCode.CreatureTemplateNotFound, nameof(CreatureTemplate), nameof(CreatureTemplate.Id), request.CreatureTemplateId.ToString());

			if (creatureTemplate.GameId != battle.GameId)
			{
				throw new InvalidArgumentException(ErrorCode.CreatureTemplateBelongsToAnotherGame, "Выбранный шаблон существа принадлежит другой игре.");
			}

			var creature = new Creature(battle.Id, creatureTemplate, request.Name);
			battle.Creatures.Add(creature);
			await _battleRepository.UpdateAsync(battle);

			return _mapper.Map<BattleDto>(battle);
		}

		public async Task<BattleDto> UpdateCreatureAsync(UpdateBattleCreatureRequest request)
		{
			var battle = await GetByIdForGmAsync(request.BattleId);
			var creature = GetCreature(battle, request.CreatureId);

			creature.UpdateState(request.Name, request.CurrentHP, request.CurrentSta);
			await _battleRepository.UpdateAsync(battle);

			return _mapper.Map<BattleDto>(battle);
		}

		public async Task<BattleDto> RemoveCreatureAsync(long battleId, long creatureId)
		{
			var battle = await GetByIdForGmAsync(battleId);
			ThrowIfNotDraft(battle);
			var creature = GetCreature(battle, creatureId);

			battle.Creatures.Remove(creature);
			await _battleRepository.UpdateAsync(battle);

			return _mapper.Map<BattleDto>(battle);
		}

		public async Task<BattleDto> AddCreatureConditionAsync(long battleId, long creatureId, Condition condition)
		{
			var battle = await GetByIdForGmAsync(battleId);
			var creature = GetCreature(battle, creatureId);

			creature.AddCondition(condition);
			await _battleRepository.UpdateAsync(battle);

			return _mapper.Map<BattleDto>(battle);
		}

		public async Task<BattleDto> RemoveCreatureConditionAsync(long battleId, long creatureId, Condition condition)
		{
			var battle = await GetByIdForGmAsync(battleId);
			var creature = GetCreature(battle, creatureId);

			creature.RemoveCondition(condition);
			await _battleRepository.UpdateAsync(battle);

			return _mapper.Map<BattleDto>(battle);
		}

		public async Task<BattleDto> AddCharacterAsync(AddCharacterToBattleRequest request)
		{
			var battle = await GetByIdForGmAsync(request.BattleId);
			ThrowIfNotDraft(battle);

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
			battle.Characters.Add(battleCharacter);
			await _battleRepository.UpdateAsync(battle);

			return _mapper.Map<BattleDto>(battle);
		}

		public async Task<BattleDto> RemoveCharacterAsync(long battleId, long characterId)
		{
			var battle = await GetByIdForGmAsync(battleId);
			ThrowIfNotDraft(battle);
			var battleCharacter = GetBattleCharacter(battle, characterId);

			battle.Characters.Remove(battleCharacter);
			await _battleRepository.UpdateAsync(battle);

			return _mapper.Map<BattleDto>(battle);
		}

		public async Task<BattleDto> AddCharacterConditionAsync(long battleId, long characterId, Condition condition)
		{
			var battle = await GetByIdForGmAsync(battleId);
			var battleCharacter = GetBattleCharacter(battle, characterId);

			battleCharacter.AddCondition(condition);
			await _battleRepository.UpdateAsync(battle);

			return _mapper.Map<BattleDto>(battle);
		}

		public async Task<BattleDto> RemoveCharacterConditionAsync(long battleId, long characterId, Condition condition)
		{
			var battle = await GetByIdForGmAsync(battleId);
			var battleCharacter = GetBattleCharacter(battle, characterId);

			battleCharacter.RemoveCondition(condition);
			await _battleRepository.UpdateAsync(battle);

			return _mapper.Map<BattleDto>(battle);
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

			// Инициатива = бросок (Ref/Rea + d10). При равенстве броска выигрывает более высокий Ref/Rea,
			// при дальнейшем равенстве — персонаж (а не существо), при дальнейшем — случайно (tiebreak).
			// Итоговый порядок всегда однозначен, поэтому в Initiative сохраняется не сырой бросок,
			// а порядковый номер (1, 2, 3...) — до конца боя он больше не меняется.
			var random = new Random();

			var rolled = battle.Creatures
				.Select(c => (
					setInitiative: (Action<int>)(v => c.SetInitiative(v)),
					isCharacter: false,
					stat: c.Ref,
					roll: c.Ref + random.Next(1, 11),
					tiebreak: random.Next()))
				.Concat(battle.Characters.Select(bc => (
					setInitiative: (Action<int>)(v => bc.SetInitiative(v)),
					isCharacter: true,
					stat: bc.Character.Rea,
					roll: bc.Character.Rea + random.Next(1, 11),
					tiebreak: random.Next())))
				.OrderByDescending(x => x.roll)
				.ThenByDescending(x => x.stat)
				.ThenByDescending(x => x.isCharacter)
				.ThenBy(x => x.tiebreak)
				.ToList();

			for (var i = 0; i < rolled.Count; i++)
			{
				rolled[i].setInitiative(i + 1);
			}

			battle.MarkStarted();
			await _battleRepository.UpdateAsync(battle);

			return _mapper.Map<BattleDto>(battle);
		}

		private static void ThrowIfNotDraft(Battle battle)
		{
			if (battle.Status != BattleStatus.Draft)
			{
				throw new InvalidArgumentException(ErrorCode.BattleAlreadyStarted, "Изменять состав участников можно только до начала боя.");
			}
		}

		private static Creature GetCreature(Battle battle, long creatureId)
		{
			var creature = battle.Creatures.FirstOrDefault(x => x.Id == creatureId);

			NotFoundException.ThrowIfNull(
				creature, ErrorCode.CreatureNotFoundInBattle, nameof(Creature), nameof(Creature.Id), creatureId.ToString());

			return creature;
		}

		private static BattleCharacter GetBattleCharacter(Battle battle, long characterId)
		{
			var battleCharacter = battle.Characters.FirstOrDefault(x => x.CharacterId == characterId);

			NotFoundException.ThrowIfNull(
				battleCharacter, ErrorCode.BattleCharacterNotFound, nameof(BattleCharacter), nameof(BattleCharacter.CharacterId), characterId.ToString());

			return battleCharacter;
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
	}
}
