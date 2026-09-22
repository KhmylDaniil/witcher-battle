using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Domain.Drafts;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Services
{
	/// <summary>
	/// Процесс атаки внутри уже идущего боя — выбор способности/цели, подтверждения сторон, расчёт
	/// попадания (<see cref="IBattleHitResolver"/>) и урона (математика — в
	/// <see cref="BattleCombatCalculator"/>), продолжение той же способности (multiattack) или
	/// завершение хода. Ростер/старт боя — в <see cref="BattleService"/>.
	/// </summary>
	public class BattleCombatService : IBattleCombatService
	{
		private readonly IBattleRepository _battleRepository;
		private readonly ICharacterRepository _characterRepository;
		private readonly IBattleParticipantAuthorizer _authorizer;
		private readonly IBattleCombatContextProvider _contextProvider;
		private readonly IBattleHitResolver _hitResolver;
		private readonly IBattleNotifier _battleNotifier;
		private readonly IBattleDtoMapper _dtoMapper;
		private readonly IBattleTurnProcessor _turnProcessor;

		public BattleCombatService(
			IBattleRepository battleRepository,
			ICharacterRepository characterRepository,
			IBattleParticipantAuthorizer authorizer,
			IBattleCombatContextProvider contextProvider,
			IBattleHitResolver hitResolver,
			IBattleNotifier battleNotifier,
			IBattleDtoMapper dtoMapper,
			IBattleTurnProcessor turnProcessor)
		{
			_battleRepository = battleRepository;
			_characterRepository = characterRepository;
			_authorizer = authorizer;
			_contextProvider = contextProvider;
			_hitResolver = hitResolver;
			_battleNotifier = battleNotifier;
			_dtoMapper = dtoMapper;
			_turnProcessor = turnProcessor;
		}

		public async Task<BattleDto> StartAttackAsync(StartAttackRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			BattleParticipants.EnsureInProgress(battle);

			var (attackerKind, attackerId) = BattleParticipants.GetActive(battle);
			await _authorizer.EnsureControllerAsync(battle, attackerKind, attackerId, ErrorCode.CurrentUserNotAttackController);
			EnsureNotStunned(battle, attackerKind, attackerId);

			BattleParticipants.EnsureExists(battle, request.DefenderKind, request.DefenderId);

			var attackerContext = await _contextProvider.GetContextAsync(battle, attackerKind, attackerId);
			var ability = attackerContext.Abilities.FirstOrDefault(a => a.Id == request.AbilityId);
			if (ability is null)
			{
				throw new InvalidArgumentException(ErrorCode.AbilityDoesNotBelongToAttacker, "У атакующего нет такой способности.");
			}

			var isBonusAction = TryChargeBonusAction(battle, attackerKind, attackerId);

			var attack = new BattleAttack(
				battle.Id, attackerKind, attackerId, ability.Id, ability.AttacksPerTurn, request.DefenderKind, request.DefenderId, isBonusAction);
			battle.StartAttack(attack);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> SetAttackerChoicesAsync(SetAttackerChoicesRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			if (request.TargetedCreaturePartId is { } partId)
			{
				var defenderContext = await _contextProvider.GetContextAsync(battle, attack.DefenderKind, attack.DefenderId);
				BattleParticipants.EnsureTargetedPartValid(defenderContext, attack.DefenderKind, partId);
			}

			if (request.TargetedHumanBodyPart is not null)
			{
				BattleParticipants.EnsureTargetedHumanBodyPartValid(attack.DefenderKind);
			}

			attack.SetTargetPart(request.TargetedCreaturePartId);
			attack.SetTargetHumanBodyPart(request.TargetedHumanBodyPart);
			attack.SetAttackRoll(request.AttackRoll);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> ConfirmAttackerAsync(long battleId)
		{
			var battle = await GetByIdAsync(battleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			attack.ConfirmAttacker();
			await _hitResolver.ResolveIfBothConfirmedAsync(battle, attack);

			return await SaveAndNotifyAsync(battle);
		}

		private static void EnsureNotStunned(Battle battle, ParticipantKind kind, long participantId)
		{
			if (BattleParticipants.HasCondition(battle, kind, participantId, Condition.Stun))
			{
				throw new InvalidArgumentException(
					ErrorCode.ParticipantIsStunned, "Участник оглушён и может в свой ход только пройти проверку Оглушения.");
			}
		}

		/// <summary>
		/// Если участник — персонаж, уже действовавший в этот ход, списывает стамину за дополнительное
		/// действие (BonusActionRules) и возвращает true (действие берётся как доп.). Существа доп.
		/// действий не имеют. Если персонаж ещё не действовал в этот ход — это его обычное (первое)
		/// действие, стамина не списывается, возвращается false.
		/// </summary>
		private static bool TryChargeBonusAction(Battle battle, ParticipantKind attackerKind, long attackerId)
		{
			if (attackerKind != ParticipantKind.Character)
			{
				return false;
			}

			var battleCharacter = BattleParticipants.GetBattleCharacter(battle, attackerId);
			if (!battleCharacter.HasActedThisTurn)
			{
				return false;
			}

			if (battleCharacter.CurrentSta < BonusActionRules.StaminaCost)
			{
				throw new InvalidArgumentException(
					ErrorCode.NotEnoughStaminaForBonusAction,
					$"Недостаточно выносливости для дополнительного действия (нужно {BonusActionRules.StaminaCost}).");
			}

			battleCharacter.SpendStamina(BonusActionRules.StaminaCost);
			return true;
		}

		public async Task<BattleDto> SetDefenderChoiceAsync(SetDefenderChoiceRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.DefenderKind, attack.DefenderId, ErrorCode.CurrentUserNotDefenderController);

			if (request.IsParry)
			{
				var defenderContext = await _contextProvider.GetContextAsync(battle, attack.DefenderKind, attack.DefenderId);
				var parrySkill = BattleParticipants.GetEquippedMeleeWeaponSkill(attack.DefenderKind, defenderContext);
				if (parrySkill is null)
				{
					throw new InvalidArgumentException(
						ErrorCode.ParryNotAvailable, "Парирование недоступно — нет экипированного оружия ближнего боя.");
				}

				attack.SetDefenderChoice(parrySkill.Value, request.DefenseRoll, isParry: true);
			}
			else
			{
				var availableSkills = await _hitResolver.GetAvailableDefensiveSkillsAsync(battle, attack);
				if (!availableSkills.Contains(request.DefensiveSkill))
				{
					throw new InvalidArgumentException(ErrorCode.InvalidDefensiveSkillChoice, "Недопустимый защитный навык для этой способности.");
				}

				attack.SetDefenderChoice(request.DefensiveSkill, request.DefenseRoll);
			}

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> ConfirmDefenderAsync(long battleId)
		{
			var battle = await GetByIdAsync(battleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.DefenderKind, attack.DefenderId, ErrorCode.CurrentUserNotDefenderController);

			var defenderIsStunned = BattleParticipants.HasCondition(battle, attack.DefenderKind, attack.DefenderId, Condition.Stun);
			attack.ConfirmDefender(defenderIsStunned);
			await _hitResolver.ResolveIfBothConfirmedAsync(battle, attack);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> SetDamageRollAsync(SetDamageRollRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			if (request.DamageRoll is { } roll)
			{
				var attackerContext = await _contextProvider.GetContextAsync(battle, attack.AttackerKind, attack.AttackerId);
				var ability = attackerContext.Abilities.First(a => a.Id == attack.AbilityId);
				BattleCombatCalculator.ValidateDamageRoll(roll, ability);
			}

			attack.SetDamageRoll(request.DamageRoll);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> ContinueDamageAsync(long battleId)
		{
			var battle = await GetByIdAsync(battleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			if (attack.Phase != BattleAttackPhase.AwaitingDamageRoll)
			{
				throw new InvalidArgumentException(ErrorCode.AttackNotInExpectedPhase, "Сейчас не ожидается расчёт урона.");
			}

			var attackerContext = await _contextProvider.GetContextAsync(battle, attack.AttackerKind, attack.AttackerId);
			var ability = attackerContext.Abilities.First(a => a.Id == attack.AbilityId);
			var defenderContext = await _contextProvider.GetContextAsync(battle, attack.DefenderKind, attack.DefenderId);

			var damage = BattleCombatCalculator.CalculateDamage(attackerContext, defenderContext, attack.DefenderKind, ability, attack);

			// Проверка состояний — только если удар нанёс хоть какой-то урон, независимо от того, кто защищается.
			// Оглушение — особый случай: прохождение этой проверки (ApplyChance) лишь означает, что
			// СТОИТ попытаться оглушить, а не что оно наложено — это ещё предстоит решить stun save'ом
			// (см. SetStunSaveRollAsync/ResolveStunSaveAsync), поэтому её сразу не накладываем.
			var rolledConditions = damage.FinalDamage >= 1
				? BattleCombatCalculator.RollAppliedConditions(ability)
				: [];
			var appliedConditions = rolledConditions.Where(c => c != Condition.Stun).ToList();

			// Критический эффект — тоже только если удар нанёс хоть какой-то урон. Как и провалившийся по
			// ApplyChance Stun, критическая проверка Оглушения (см. ниже) идёт через тот же stun save, не
			// накладывается автоматически.
			var crit = damage.FinalDamage >= 1
				? BattleCombatCalculator.TryResolveCriticalHit(attack, defenderContext, ability.DamageType)
				: null;
			var damageWithCrit = crit is { } c ? damage with { FinalDamage = damage.FinalDamage + c.BonusDamage } : damage;
			var requiresStunSave = rolledConditions.Contains(Condition.Stun) || crit is not null;

			BattleParticipants.ApplyDamage(battle, attack, attack.DefenderKind, attack.DefenderId, damageWithCrit, appliedConditions);
			if (crit is { } appliedCrit)
			{
				BattleParticipants.ApplyCriticalWound(battle, attack.DefenderKind, attack.DefenderId, appliedCrit.SlotKey, appliedCrit.Wound);
			}

			// Износ конкретного экземпляра брони живёт на Character, а не на Battle — сохраняем отдельно.
			if (damage.WornArmorItemId is { } wornItemId)
			{
				defenderContext.Character!.Items.First(i => i.Id == wornItemId).WearArmor(attack.ResolvedHumanBodyPart!.Value);
				await _characterRepository.UpdateAsync(defenderContext.Character);
			}

			var attackerName = BattleParticipants.GetName(battle, attack.AttackerKind, attack.AttackerId);
			var defenderName = BattleParticipants.GetName(battle, attack.DefenderKind, attack.DefenderId);
			battle.AddLogEntry(BattleCombatLogFormatter.FormatHit(attackerName, ability.Name, defenderName, attack, damageWithCrit, appliedConditions, crit));

			attack.MarkDamageResolved(requiresStunSave);

			return await SaveAndNotifyAsync(battle);
		}

		/// <summary>Ручной ввод д10 для stun save защитника — параллель SetDamageRollAsync, но со стороны защитника.</summary>
		public async Task<BattleDto> SetStunSaveRollAsync(SetStunSaveRollRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.DefenderKind, attack.DefenderId, ErrorCode.CurrentUserNotDefenderController);

			attack.SetStunSaveRoll(request.Roll);

			return await SaveAndNotifyAsync(battle);
		}

		/// <summary>
		/// Разрешает stun save: чистый д10 (введённый вручную или брошенный сервером) против Устойчивости
		/// защитника — результат ≥ значения Устойчивости означает, что Оглушение наложено.
		/// </summary>
		public async Task<BattleDto> ResolveStunSaveAsync(long battleId)
		{
			var battle = await GetByIdAsync(battleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.DefenderKind, attack.DefenderId, ErrorCode.CurrentUserNotDefenderController);

			if (attack.Phase != BattleAttackPhase.AwaitingStunSave)
			{
				throw new InvalidArgumentException(ErrorCode.AttackNotInExpectedPhase, "Сейчас не ожидается проверка Оглушения.");
			}

			var rollUsed = attack.StunSaveRoll ?? BattleCombatCalculator.RollDie(10);
			var defenderContext = await _contextProvider.GetContextAsync(battle, attack.DefenderKind, attack.DefenderId);
			var stunValue = attack.DefenderKind == ParticipantKind.Creature ? defenderContext.Creature!.Stun : defenderContext.Character!.Stun;
			var succeeded = rollUsed >= stunValue;

			if (succeeded)
			{
				BattleParticipants.AddCondition(battle, attack.DefenderKind, attack.DefenderId, Condition.Stun);
			}

			attack.ResolveStunSave(rollUsed, succeeded);

			var defenderName = BattleParticipants.GetName(battle, attack.DefenderKind, attack.DefenderId);
			battle.AddLogEntry(
				$"Проверка Оглушения для {defenderName}: бросок {rollUsed} против Устойчивости {stunValue} — "
					+ (succeeded ? "Оглушение наложено." : "Оглушение не наложено."));

			return await SaveAndNotifyAsync(battle);
		}

		/// <summary>
		/// Оглушённый участник в свой ход не может ничего, кроме этой проверки — независимо от её
		/// исхода, ход после неё завершается. Успех (бросок ≥ Устойчивости) — Оглушение остаётся,
		/// провал — снимается (участник придёт в себя и сможет действовать со следующего своего хода).
		/// </summary>
		public async Task<BattleDto> RollOwnStunSaveAsync(RollOwnStunSaveRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			BattleParticipants.EnsureInProgress(battle);

			if (battle.Attack is not null)
			{
				throw new InvalidArgumentException(ErrorCode.AttackAlreadyInProgress, "Нельзя пройти проверку Оглушения во время незавершённой атаки.");
			}

			var (activeKind, activeId) = BattleParticipants.GetActive(battle);
			await _authorizer.EnsureControllerAsync(battle, activeKind, activeId, ErrorCode.NotYourTurn);

			if (!BattleParticipants.HasCondition(battle, activeKind, activeId, Condition.Stun))
			{
				throw new InvalidArgumentException(ErrorCode.ParticipantNotStunned, "Участник не находится в состоянии Оглушения.");
			}

			var rollUsed = request.Roll ?? BattleCombatCalculator.RollDie(10);
			var context = await _contextProvider.GetContextAsync(battle, activeKind, activeId);
			var stunValue = activeKind == ParticipantKind.Creature ? context.Creature!.Stun : context.Character!.Stun;
			var staysStunned = rollUsed >= stunValue;

			if (!staysStunned)
			{
				BattleParticipants.RemoveCondition(battle, activeKind, activeId, Condition.Stun);
			}

			var name = BattleParticipants.GetName(battle, activeKind, activeId);
			battle.AddLogEntry(
				$"{name} проходит проверку Оглушения: бросок {rollUsed} против Устойчивости {stunValue} — "
					+ (staysStunned ? "остаётся оглушён." : "приходит в себя."));

			await _turnProcessor.AdvanceTurnAsync(battle);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> NextSwingAsync(NextSwingRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			BattleParticipants.EnsureExists(battle, request.DefenderKind, request.DefenderId);
			attack.PrepareNextSwing(request.DefenderKind, request.DefenderId);

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> EndActivationAsync(long battleId)
		{
			var battle = await GetByIdAsync(battleId);
			var attack = BattleParticipants.GetActiveAttack(battle);
			await _authorizer.EnsureControllerAsync(battle, attack.AttackerKind, attack.AttackerId, ErrorCode.CurrentUserNotAttackController);

			if (attack.Phase != BattleAttackPhase.SwingResolved)
			{
				throw new InvalidArgumentException(ErrorCode.AttackNotInExpectedPhase, "Текущий выпад ещё не разрешён.");
			}

			battle.ClearAttack();

			// Персонаж после основного (бесплатного) действия может взять ещё одно за выносливость (см.
			// StartAttackAsync) — поэтому ход передаётся дальше только после дополнительного действия
			// или отказа от него (SkipTurnAsync). У существ такой возможности нет — их ход всегда
			// заканчивается сразу.
			if (attack.AttackerKind == ParticipantKind.Character && !attack.IsBonusAction)
			{
				BattleParticipants.GetBattleCharacter(battle, attack.AttackerId).MarkActedThisTurn();
			}
			else
			{
				await _turnProcessor.AdvanceTurnAsync(battle);
			}

			return await SaveAndNotifyAsync(battle);
		}

		public async Task<BattleDto> SkipTurnAsync(long battleId)
		{
			var battle = await GetByIdAsync(battleId);
			BattleParticipants.EnsureInProgress(battle);

			if (battle.Attack is not null)
			{
				throw new InvalidArgumentException(ErrorCode.AttackAlreadyInProgress, "Нельзя пропустить ход во время незавершённой атаки.");
			}

			var (activeKind, activeId) = BattleParticipants.GetActive(battle);
			await _authorizer.EnsureControllerAsync(battle, activeKind, activeId, ErrorCode.NotYourTurn);
			EnsureNotStunned(battle, activeKind, activeId);

			await _turnProcessor.AdvanceTurnAsync(battle);

			return await SaveAndNotifyAsync(battle);
		}

		/// <summary>
		/// Попытка снять состояние (Кровотечение/Отравление) броском навыка — отдельное действие хода, не
		/// атака. Для персонажа это обычное или дополнительное действие (та же стамина/штраф −3, что и у
		/// дополнительной атаки — см. TryChargeBonusAction); для существа любая такая попытка считается
		/// действием и сразу заканчивает ход. Self-only правило (Endurance против Отравления) разрешает
		/// целиться только в себя — иначе (FirstAid) можно выбрать любую цель, включая себя.
		/// </summary>
		public async Task<BattleDto> AttemptRemoveConditionAsync(AttemptRemoveConditionRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			BattleParticipants.EnsureInProgress(battle);

			if (battle.Attack is not null)
			{
				throw new InvalidArgumentException(ErrorCode.AttackAlreadyInProgress, "Нельзя снимать состояние во время незавершённой атаки.");
			}

			var (activeKind, activeId) = BattleParticipants.GetActive(battle);
			await _authorizer.EnsureControllerAsync(battle, activeKind, activeId, ErrorCode.NotYourTurn);
			EnsureNotStunned(battle, activeKind, activeId);

			var rule = ConditionRemovalCatalog.FindRule(request.Condition, request.Skill);
			if (rule is null)
			{
				throw new InvalidArgumentException(ErrorCode.ConditionRemovalRuleNotFound, "Этим навыком нельзя снять это состояние.");
			}

			if (rule.SelfOnly && (request.TargetKind != activeKind || request.TargetId != activeId))
			{
				throw new InvalidArgumentException(ErrorCode.ConditionRemovalTargetInvalid, "Этим навыком можно снять состояние только с себя.");
			}

			BattleParticipants.EnsureExists(battle, request.TargetKind, request.TargetId);
			if (!BattleParticipants.HasCondition(battle, request.TargetKind, request.TargetId, request.Condition))
			{
				throw new InvalidArgumentException(ErrorCode.ConditionNotPresentOnTarget, "У цели нет этого состояния.");
			}

			var isBonusAction = TryChargeBonusAction(battle, activeKind, activeId);

			var rollUsed = request.Roll ?? BattleCombatCalculator.RollDie(10);
			var activeContext = await _contextProvider.GetContextAsync(battle, activeKind, activeId);
			var penalty = isBonusAction ? BonusActionRules.RollPenalty : 0;
			var total = activeContext.GetSkillValue(request.Skill) + rollUsed - penalty;
			var succeeded = total >= rule.Difficulty;

			if (succeeded)
			{
				BattleParticipants.RemoveCondition(battle, request.TargetKind, request.TargetId, request.Condition);
			}

			var activeName = BattleParticipants.GetName(battle, activeKind, activeId);
			var targetName = BattleParticipants.GetName(battle, request.TargetKind, request.TargetId);
			battle.AddLogEntry(
				$"{activeName} пытается снять состояние {request.Condition} с {targetName} ({request.Skill} {total} против сложности {rule.Difficulty}) — "
					+ (succeeded ? "успех, состояние снято." : "провал."));

			if (activeKind == ParticipantKind.Character && !isBonusAction)
			{
				BattleParticipants.GetBattleCharacter(battle, activeId).MarkActedThisTurn();
			}
			else
			{
				await _turnProcessor.AdvanceTurnAsync(battle);
			}

			return await SaveAndNotifyAsync(battle);
		}

		/// <summary>
		/// Снятие состояния действием без броска — всегда успешно и только с себя (Огонь, Падение — см.
		/// ConditionRemovalCatalog.IsAutoClearable). Расход действия — та же механика, что и у
		/// AttemptRemoveConditionAsync: обычное или дополнительное действие персонажа, у существа —
		/// всегда весь ход целиком.
		/// </summary>
		public async Task<BattleDto> ClearConditionAsync(ClearConditionRequest request)
		{
			var battle = await GetByIdAsync(request.BattleId);
			BattleParticipants.EnsureInProgress(battle);

			if (battle.Attack is not null)
			{
				throw new InvalidArgumentException(ErrorCode.AttackAlreadyInProgress, "Нельзя снимать состояние во время незавершённой атаки.");
			}

			var (activeKind, activeId) = BattleParticipants.GetActive(battle);
			await _authorizer.EnsureControllerAsync(battle, activeKind, activeId, ErrorCode.NotYourTurn);
			EnsureNotStunned(battle, activeKind, activeId);

			if (!ConditionRemovalCatalog.IsAutoClearable(request.Condition))
			{
				throw new InvalidArgumentException(ErrorCode.ConditionRemovalRuleNotFound, "Это состояние нельзя снять таким действием.");
			}

			if (!BattleParticipants.HasCondition(battle, activeKind, activeId, request.Condition))
			{
				throw new InvalidArgumentException(ErrorCode.ConditionNotPresentOnTarget, "У вас нет этого состояния.");
			}

			var isBonusAction = TryChargeBonusAction(battle, activeKind, activeId);
			BattleParticipants.RemoveCondition(battle, activeKind, activeId, request.Condition);

			var activeName = BattleParticipants.GetName(battle, activeKind, activeId);
			battle.AddLogEntry($"{activeName} снимает с себя состояние {request.Condition} действием.");

			if (activeKind == ParticipantKind.Character && !isBonusAction)
			{
				BattleParticipants.GetBattleCharacter(battle, activeId).MarkActedThisTurn();
			}
			else
			{
				await _turnProcessor.AdvanceTurnAsync(battle);
			}

			return await SaveAndNotifyAsync(battle);
		}

		private async Task<Battle> GetByIdAsync(long id)
		{
			var battle = await _battleRepository.GetByIdAsync(id);
			NotFoundException.ThrowIfNull(battle, ErrorCode.BattleNotFound, nameof(Battle), nameof(Battle.Id), id.ToString());

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
