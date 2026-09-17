using AutoMapper;
using Wastelands.Core.Contracts.Constants;
using Wastelands.Core.Contracts.Contracts;
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
		private static readonly Random Random = new();

		private readonly IBattleRepository _battleRepository;
		private readonly ICreatureTemplateRepository _creatureTemplateRepository;
		private readonly ICharacterRepository _characterRepository;
		private readonly IGameAccessGuard _gameAccessGuard;
		private readonly IUserContext _userContext;
		private readonly IBattleNotifier _battleNotifier;
		private readonly IMapper _mapper;

		public BattleService(
			IBattleRepository battleRepository,
			ICreatureTemplateRepository creatureTemplateRepository,
			ICharacterRepository characterRepository,
			IGameAccessGuard gameAccessGuard,
			IUserContext userContext,
			IBattleNotifier battleNotifier,
			IMapper mapper)
		{
			_battleRepository = battleRepository;
			_creatureTemplateRepository = creatureTemplateRepository;
			_characterRepository = characterRepository;
			_gameAccessGuard = gameAccessGuard;
			_userContext = userContext;
			_battleNotifier = battleNotifier;
			_mapper = mapper;
		}

		public async Task<BattleDto> GetBattleByIdAsync(long id)
		{
			var battle = await GetByIdAsync(id);
			return await MapToDtoAsync(battle);
		}

		public async Task<List<BattleDto>> GetBattlesAsync(BattleFilter filter)
		{
			var battles = await _battleRepository.GetListByFilterAsync(filter);
			var dtos = new List<BattleDto>();
			foreach (var battle in battles)
			{
				dtos.Add(await MapToDtoAsync(battle));
			}

			return dtos;
		}

		public async Task<BattleDto> CreateBattleAsync(CreateBattleRequest request)
		{
			var game = await _gameAccessGuard.GetGameOwnedByCurrentUserAsync(request.GameId);
			var battle = new Battle(game.Id, request.Name);

			await _battleRepository.CreateAsync(battle);

			return await MapToDtoAsync(battle);
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

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> UpdateCreatureAsync(UpdateBattleCreatureRequest request)
		{
			var battle = await GetByIdForGmAsync(request.BattleId);
			var creature = GetCreature(battle, request.CreatureId);

			creature.UpdateState(request.Name, request.CurrentHP, request.CurrentSta);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> RemoveCreatureAsync(long battleId, long creatureId)
		{
			var battle = await GetByIdForGmAsync(battleId);
			ThrowIfNotDraft(battle);
			var creature = GetCreature(battle, creatureId);

			battle.Creatures.Remove(creature);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> AddCreatureConditionAsync(long battleId, long creatureId, Condition condition)
		{
			var battle = await GetByIdForGmAsync(battleId);
			var creature = GetCreature(battle, creatureId);

			creature.AddCondition(condition);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> RemoveCreatureConditionAsync(long battleId, long creatureId, Condition condition)
		{
			var battle = await GetByIdForGmAsync(battleId);
			var creature = GetCreature(battle, creatureId);

			creature.RemoveCondition(condition);

			return await SaveAndNotifyAsync(battle);
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

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> RemoveCharacterAsync(long battleId, long characterId)
		{
			var battle = await GetByIdForGmAsync(battleId);
			ThrowIfNotDraft(battle);
			var battleCharacter = GetBattleCharacter(battle, characterId);

			battle.Characters.Remove(battleCharacter);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> AddCharacterConditionAsync(long battleId, long characterId, Condition condition)
		{
			var battle = await GetByIdForGmAsync(battleId);
			var battleCharacter = GetBattleCharacter(battle, characterId);

			battleCharacter.AddCondition(condition);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> RemoveCharacterConditionAsync(long battleId, long characterId, Condition condition)
		{
			var battle = await GetByIdForGmAsync(battleId);
			var battleCharacter = GetBattleCharacter(battle, characterId);

			battleCharacter.RemoveCondition(condition);

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

			// Инициатива = бросок (Ref/Rea + d10). При равенстве броска выигрывает более высокий Ref/Rea,
			// при дальнейшем равенстве — персонаж (а не существо), при дальнейшем — случайно (tiebreak).
			// Итоговый порядок всегда однозначен, поэтому в Initiative сохраняется не сырой бросок,
			// а порядковый номер (1, 2, 3...) — до конца боя он больше не меняется.
			var rolled = battle.Creatures
				.Select(c => (
					setInitiative: (Action<int>)(v => c.SetInitiative(v)),
					isCharacter: false,
					stat: c.Ref,
					roll: c.Ref + RollDie(10),
					tiebreak: Random.Next()))
				.Concat(battle.Characters.Select(bc => (
					setInitiative: (Action<int>)(v => bc.SetInitiative(v)),
					isCharacter: true,
					stat: bc.Character.Rea,
					roll: bc.Character.Rea + RollDie(10),
					tiebreak: Random.Next())))
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

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> StartAttackAsync(StartAttackRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			EnsureInProgress(battle);

			var (attackerKind, attackerId) = GetActiveParticipant(battle);
			await EnsureController(battle, attackerKind, attackerId, ErrorCode.CurrentUserNotAttackController);

			EnsureParticipantExists(battle, request.DefenderKind, request.DefenderId);

			var attackerContext = await GetCombatContextAsync(battle, attackerKind, attackerId);
			var ability = attackerContext.Abilities.FirstOrDefault(a => a.Id == request.AbilityId);
			if (ability is null)
			{
				throw new InvalidArgumentException(ErrorCode.AbilityDoesNotBelongToAttacker, "У атакующего нет такой способности.");
			}

			var attack = new BattleAttack(
				battle.Id, attackerKind, attackerId, ability.Id, ability.AttacksPerTurn, request.DefenderKind, request.DefenderId);
			battle.StartAttack(attack);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> SetAttackerChoicesAsync(SetAttackerChoicesRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			var attack = GetActiveAttack(battle);
			await EnsureController(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			if (request.TargetedCreaturePartId is { } partId)
			{
				if (attack.DefenderKind != ParticipantKind.Creature)
				{
					throw new InvalidArgumentException(ErrorCode.CreatureTemplatePartNotFound, "У защищающегося персонажа нет частей тела.");
				}

				var defenderContext = await GetCombatContextAsync(battle, attack.DefenderKind, attack.DefenderId);
				if (defenderContext.Template!.Parts.All(p => p.Id != partId))
				{
					throw new InvalidArgumentException(ErrorCode.CreatureTemplatePartNotFound, "Часть тела не найдена у защитника.");
				}
			}

			attack.SetTargetPart(request.TargetedCreaturePartId);
			attack.SetAttackRoll(request.AttackRoll);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> ConfirmAttackerAsync(long battleId)
		{
			var battle = await GetByIdAsync(battleId);
			var attack = GetActiveAttack(battle);
			await EnsureController(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			attack.ConfirmAttacker();
			await ResolveHitIfBothConfirmedAsync(battle, attack);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> SetDefenderChoiceAsync(SetDefenderChoiceRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			var attack = GetActiveAttack(battle);
			await EnsureController(battle, attack.DefenderKind, attack.DefenderId, ErrorCode.CurrentUserNotDefenderController);

			var availableSkills = await GetAvailableDefensiveSkillsAsync(battle, attack);
			if (!availableSkills.Contains(request.DefensiveSkill))
			{
				throw new InvalidArgumentException(ErrorCode.InvalidDefensiveSkillChoice, "Недопустимый защитный навык для этой способности.");
			}

			attack.SetDefenderChoice(request.DefensiveSkill, request.DefenseRoll);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> ConfirmDefenderAsync(long battleId)
		{
			var battle = await GetByIdAsync(battleId);
			var attack = GetActiveAttack(battle);
			await EnsureController(battle, attack.DefenderKind, attack.DefenderId, ErrorCode.CurrentUserNotDefenderController);

			attack.ConfirmDefender();
			await ResolveHitIfBothConfirmedAsync(battle, attack);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> SetDamageRollAsync(SetDamageRollRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			var attack = GetActiveAttack(battle);
			await EnsureController(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			attack.SetDamageRoll(request.DamageRoll);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> ContinueDamageAsync(long battleId)
		{
			var battle = await GetByIdAsync(battleId);
			var attack = GetActiveAttack(battle);
			await EnsureController(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			if (attack.Phase != BattleAttackPhase.AwaitingDamageRoll)
			{
				throw new InvalidArgumentException(ErrorCode.AttackNotInExpectedPhase, "Сейчас не ожидается расчёт урона.");
			}

			var attackerContext = await GetCombatContextAsync(battle, attack.AttackerKind, attack.AttackerId);
			var ability = attackerContext.Abilities.First(a => a.Id == attack.AbilityId);

			var damageRoll = attack.DamageRoll ?? Enumerable.Range(0, ability.DamageDiceCount).Sum(_ => RollDie(6));
			double raw = damageRoll + ability.DamageModifier;

			var attackerName = GetParticipantName(battle, attack.AttackerKind, attack.AttackerId);
			var defenderName = GetParticipantName(battle, attack.DefenderKind, attack.DefenderId);
			int finalDamage;
			string? partName = null;

			if (attack.DefenderKind == ParticipantKind.Creature)
			{
				var defenderContext = await GetCombatContextAsync(battle, attack.DefenderKind, attack.DefenderId);
				var part = defenderContext.Template!.Parts.First(p => p.Id == attack.ResolvedCreaturePartId);
				partName = part.Name;

				raw *= part.DamageModifier;
				if (defenderContext.Template.DamageTypeModifiers.TryGetValue(ability.DamageType, out var modifier))
				{
					raw = modifier switch
					{
						DamageTypeModifier.Vulnerability => raw * 2,
						DamageTypeModifier.Resistance => raw / 2,
						DamageTypeModifier.Immunity => 0,
						_ => raw,
					};
				}

				finalDamage = Math.Max(0, (int)Math.Round(raw) - part.Armor);
				GetCreature(battle, attack.DefenderId).ApplyDamage(finalDamage);
			}
			else
			{
				finalDamage = Math.Max(0, (int)Math.Round(raw));
				GetBattleCharacter(battle, attack.DefenderId).ApplyDamage(finalDamage);
			}

			battle.AddLogEntry(partName is null
				? $"{attackerName} ({ability.Name}) атакует {defenderName}: попадание, урон {finalDamage}."
				: $"{attackerName} ({ability.Name}) атакует {defenderName} ({partName}): попадание, урон {finalDamage}.");

			attack.MarkDamageResolved();

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> NextSwingAsync(NextSwingRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			var attack = GetActiveAttack(battle);
			await EnsureController(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			EnsureParticipantExists(battle, request.DefenderKind, request.DefenderId);

			attack.PrepareNextSwing(request.DefenderKind, request.DefenderId);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> EndActivationAsync(long battleId)
		{
			var battle = await GetByIdAsync(battleId);
			var attack = GetActiveAttack(battle);
			await EnsureController(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			if (attack.Phase != BattleAttackPhase.SwingResolved)
			{
				throw new InvalidArgumentException(ErrorCode.AttackNotInExpectedPhase, "Текущий выпад ещё не разрешён.");
			}

			battle.ClearAttack();
			battle.AdvanceTurn();

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> SkipTurnAsync(long battleId)
		{
			var battle = await GetByIdAsync(battleId);
			EnsureInProgress(battle);

			if (battle.Attack is not null)
			{
				throw new InvalidArgumentException(ErrorCode.AttackAlreadyInProgress, "Нельзя пропустить ход во время незавершённой атаки.");
			}

			var (activeKind, activeId) = GetActiveParticipant(battle);
			await EnsureController(battle, activeKind, activeId, ErrorCode.NotYourTurn);

			battle.AdvanceTurn();

			return await SaveAndNotifyAsync(battle);
		}

		private async Task ResolveHitIfBothConfirmedAsync(Battle battle, BattleAttack attack)
		{
			if (!attack.AttackerConfirmed || !attack.DefenderConfirmed)
			{
				return;
			}

			var attackerContext = await GetCombatContextAsync(battle, attack.AttackerKind, attack.AttackerId);
			var ability = attackerContext.Abilities.First(a => a.Id == attack.AbilityId);
			var defenderContext = await GetCombatContextAsync(battle, attack.DefenderKind, attack.DefenderId);

			long? resolvedPartId = null;
			var hitPenalty = 0;
			if (attack.DefenderKind == ParticipantKind.Creature)
			{
				var parts = defenderContext.Template!.Parts;
				if (attack.TargetedCreaturePartId is { } chosenPartId)
				{
					resolvedPartId = chosenPartId;
					hitPenalty = parts.First(p => p.Id == chosenPartId).HitPenalty;
				}
				else
				{
					var hitRoll = RollDie(10);
					var part = parts.FirstOrDefault(p => hitRoll >= p.MinToHit && hitRoll <= p.MaxToHit) ?? parts.First();
					resolvedPartId = part.Id;
				}
			}

			var attackValue = attackerContext.GetSkillValue(ability.AttackSkill) + hitPenalty;
			var attackRoll = attack.AttackRoll ?? RollDie(10);
			var attackTotal = attackValue + attackRoll;

			var defenseValue = defenderContext.GetSkillValue(attack.DefensiveSkill!.Value);
			var defenseRoll = attack.DefenseRoll ?? RollDie(10);
			var defenseTotal = defenseValue + defenseRoll;

			var succeeded = attackTotal > defenseTotal;
			attack.MarkHitResolved(succeeded, succeeded ? resolvedPartId : null);

			if (!succeeded)
			{
				var attackerName = GetParticipantName(battle, attack.AttackerKind, attack.AttackerId);
				var defenderName = GetParticipantName(battle, attack.DefenderKind, attack.DefenderId);
				battle.AddLogEntry($"{attackerName} ({ability.Name}) атакует {defenderName}: промах.");
			}
		}

		private async Task<List<Skill>> GetAvailableDefensiveSkillsAsync(Battle battle, BattleAttack attack)
		{
			var attackerContext = await GetCombatContextAsync(battle, attack.AttackerKind, attack.AttackerId);
			var ability = attackerContext.Abilities.First(a => a.Id == attack.AbilityId);
			return ability.DefensiveSkills.Count > 0
				? ability.DefensiveSkills.Select(x => x.Skill).ToList()
				: [Skill.Dodge];
		}

		private async Task EnrichAttackDtoAsync(BattleAttackDto dto, Battle battle)
		{
			dto.AttackerName = GetParticipantName(battle, dto.AttackerKind, dto.AttackerId);
			dto.DefenderName = GetParticipantName(battle, dto.DefenderKind, dto.DefenderId);

			var attackerContext = await GetCombatContextAsync(battle, dto.AttackerKind, dto.AttackerId);
			var ability = attackerContext.Abilities.First(a => a.Id == dto.AbilityId);
			dto.AbilityName = ability.Name;
			dto.AttackerSkillValue = attackerContext.GetSkillValue(ability.AttackSkill);
			dto.AvailableDefensiveSkills = ability.DefensiveSkills.Count > 0
				? ability.DefensiveSkills.Select(x => x.Skill).ToList()
				: [Skill.Dodge];

			if (dto.DefenderKind == ParticipantKind.Creature)
			{
				var defenderContext = await GetCombatContextAsync(battle, dto.DefenderKind, dto.DefenderId);
				dto.AvailableCreatureParts = defenderContext.Template!.Parts
					.Select(p => new CreaturePartOptionDto { Id = p.Id, Name = p.Name })
					.ToList();
			}
		}

		private sealed class ParticipantCombatContext
		{
			public required List<Ability> Abilities { get; init; }

			public required Func<Skill, int> GetSkillValue { get; init; }

			/// <summary>Заполнено только для участников-существ — нужен для частей тела/брони/модификаторов урона.</summary>
			public CreatureTemplate? Template { get; init; }
		}

		private async Task<ParticipantCombatContext> GetCombatContextAsync(Battle battle, ParticipantKind kind, long participantId)
		{
			if (kind == ParticipantKind.Creature)
			{
				var creature = GetCreature(battle, participantId);
				var template = await _creatureTemplateRepository.GetByIdUnscopedAsync(creature.CreatureTemplateId);
				NotFoundException.ThrowIfNull(
					template, ErrorCode.CreatureTemplateNotFound, nameof(CreatureTemplate), nameof(CreatureTemplate.Id), creature.CreatureTemplateId.ToString());

				return new ParticipantCombatContext
				{
					Abilities = template.Abilities,
					GetSkillValue = skill => SkillHelpers.GetCreatureTemplateSkillValue(template, skill),
					Template = template,
				};
			}
			else
			{
				var battleCharacter = GetBattleCharacter(battle, participantId);
				var character = await _characterRepository.GetByIdUnscopedAsync(battleCharacter.CharacterId);
				NotFoundException.ThrowIfNull(character, ErrorCode.CharacterNotFound, nameof(Character), nameof(Character.Id), battleCharacter.CharacterId.ToString());

				return new ParticipantCombatContext
				{
					Abilities = character.Abilities,
					GetSkillValue = skill => SkillHelpers.GetCharacterSkillValue(character, skill),
				};
			}
		}

		private static string GetParticipantName(Battle battle, ParticipantKind kind, long participantId)
		{
			return kind == ParticipantKind.Creature
				? GetCreature(battle, participantId).Name
				: GetBattleCharacter(battle, participantId).Character.Name;
		}

		private static void EnsureParticipantExists(Battle battle, ParticipantKind kind, long participantId)
		{
			if (kind == ParticipantKind.Creature)
			{
				GetCreature(battle, participantId);
			}
			else
			{
				GetBattleCharacter(battle, participantId);
			}
		}

		private static (ParticipantKind Kind, long Id) GetActiveParticipant(Battle battle)
		{
			if (battle.CurrentInitiative is null)
			{
				throw new InvalidArgumentException(ErrorCode.BattleNotInProgress, "Бой ещё не начат.");
			}

			var creature = battle.Creatures.FirstOrDefault(c => c.Initiative == battle.CurrentInitiative);
			if (creature is not null)
			{
				return (ParticipantKind.Creature, creature.Id);
			}

			var character = battle.Characters.FirstOrDefault(c => c.Initiative == battle.CurrentInitiative);
			if (character is not null)
			{
				return (ParticipantKind.Character, character.CharacterId);
			}

			throw new InvalidArgumentException(ErrorCode.NotYourTurn, "Не найден активный по инициативе участник боя.");
		}

		private static BattleAttack GetActiveAttack(Battle battle)
		{
			if (battle.Attack is null)
			{
				throw new InvalidArgumentException(ErrorCode.NoActiveAttack, "В бою сейчас нет незавершённой атаки.");
			}

			return battle.Attack;
		}

		private async Task EnsureController(Battle battle, ParticipantKind kind, long participantId, ErrorCode errorCode)
		{
			if (kind == ParticipantKind.Creature)
			{
				try
				{
					await _gameAccessGuard.GetGameOwnedByCurrentUserAsync(battle.GameId);
				}
				catch (InvalidArgumentException)
				{
					throw new InvalidArgumentException(errorCode, "Существом в бою распоряжается только мастер игры.");
				}
			}
			else
			{
				var battleCharacter = GetBattleCharacter(battle, participantId);
				if (battleCharacter.Character.UserId != _userContext.CurrentUserId)
				{
					throw new InvalidArgumentException(errorCode, "Этим персонажем в бою распоряжается только контролирующий его игрок.");
				}
			}
		}

		private static void EnsureInProgress(Battle battle)
		{
			if (battle.Status != BattleStatus.InProgress)
			{
				throw new InvalidArgumentException(ErrorCode.BattleNotInProgress, "Бой ещё не начат.");
			}
		}

		private static int RollDie(int sides) => Random.Next(1, sides + 1);

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

		private async Task<BattleDto> MapToDtoAsync(Battle battle)
		{
			var dto = _mapper.Map<BattleDto>(battle);
			if (dto.Attack is not null)
			{
				await EnrichAttackDtoAsync(dto.Attack, battle);
			}

			return dto;
		}

		private async Task<BattleDto> SaveAndNotifyAsync(Battle battle)
		{
			await _battleRepository.UpdateAsync(battle);
			await _battleNotifier.NotifyBattleUpdatedAsync(battle.Id);
			return await MapToDtoAsync(battle);
		}
	}
}
