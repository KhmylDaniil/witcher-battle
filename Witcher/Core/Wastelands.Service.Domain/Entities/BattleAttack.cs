using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	/// <summary>
	/// Атака в процессе разрешения — не более одной на бой одновременно. Хранит и "рамку активации"
	/// (способность, сколько атак ею разрешено/уже сделано), и состояние текущего выпада (цель,
	/// выбор атакующего/защитника, подтверждения, результат). Между повторными атаками одной и той
	/// же способностью (multiattack) сущность не пересоздаётся — поля выпада сбрасываются на месте
	/// через <see cref="PrepareNextSwing"/>, рамка активации остаётся.
	/// </summary>
	public class BattleAttack : Entity
	{
		public long BattleId { get; private set; }

		public ParticipantKind AttackerKind { get; private set; }

		public long AttackerId { get; private set; }

		public long AbilityId { get; private set; }

		/// <summary>Снимок ability.AttacksPerTurn на момент начала активации.</summary>
		public int AttacksAllowed { get; private set; }

		/// <summary>Сколько выпадов этой активации уже разрешено (не считая текущий, ещё не разрешённый).</summary>
		public int AttacksUsed { get; private set; }

		public ParticipantKind DefenderKind { get; private set; }

		public long DefenderId { get; private set; }

		/// <summary>Часть тела защитника, выбранная атакующим. Null — при разрешении атаки выбирается случайно.</summary>
		public long? TargetedCreaturePartId { get; private set; }

		/// <summary>Ручной ввод броска атакующего. Null — при разрешении атаки бросает сервер.</summary>
		public int? AttackRoll { get; private set; }

		public bool AttackerConfirmed { get; private set; }

		/// <summary>Итог встречного броска атакующего (характеристика+навык[+модификатор части тела]+кубик) — заполняется при разрешении попадания, для лога боя.</summary>
		public int AttackTotal { get; private set; }

		public Skill? DefensiveSkill { get; private set; }

		/// <summary>Ручной ввод броска защитника. Null — при разрешении атаки бросает сервер.</summary>
		public int? DefenseRoll { get; private set; }

		public bool DefenderConfirmed { get; private set; }

		/// <summary>Итог встречного броска защитника (характеристика+навык+кубик) — заполняется при разрешении попадания, для лога боя.</summary>
		public int DefenseTotal { get; private set; }

		public BattleAttackPhase Phase { get; private set; }

		public bool? LastHitSucceeded { get; private set; }

		/// <summary>
		/// Часть тела, в которую фактически попала атака (явный выбор атакующего либо случайно
		/// определённая сервисом при разрешении попадания). Null, пока попадание не разрешено, или
		/// если защитник — персонаж (у него нет частей тела).
		/// </summary>
		public long? ResolvedCreaturePartId { get; private set; }

		/// <summary>
		/// Часть тела, в которую попала атака, если защитник — персонаж (у персонажей фиксированная
		/// анатомия, см. HumanBodyPartCatalog, а не редактируемый шаблон, поэтому не long-ссылка, как у
		/// существ, а сам enum).
		/// </summary>
		public HumanBodyPart? ResolvedHumanBodyPart { get; private set; }

		/// <summary>Часть тела персонажа-защитника, выбранная атакующим. Null — при разрешении атаки выбирается случайно.</summary>
		public HumanBodyPart? TargetedHumanBodyPart { get; private set; }

		/// <summary>Ручной ввод суммы броска урона. Null — при разрешении урона бросает сервер.</summary>
		public int? DamageRoll { get; private set; }

		/// <summary>
		/// Ручной ввод чистого д10 для stun save защитника — заполняется только когда этой атакой
		/// прошла попытка наложить Condition.Stun (см. BattleCombatService.ContinueDamageAsync). Null,
		/// пока не задан — при разрешении сервер бросает сам.
		/// </summary>
		public int? StunSaveRoll { get; private set; }

		/// <summary>true — Оглушение наложено (StunSaveRoll >= Stun защитника), false — не наложено.</summary>
		public bool? StunSaveSucceeded { get; private set; }

		/// <summary>
		/// true — это дополнительное действие персонажа за BonusActionRules.StaminaCost выносливости,
		/// взятое после уже потраченного в этот ход основного действия (см. BattleCombatService.StartAttackAsync/
		/// EndActivationAsync). К атаке применяется штраф BonusActionRules.RollPenalty — на каждый выпад,
		/// включая оба удара мультиатаки, т.к. значение не сбрасывается в PrepareNextSwing. У существ
		/// дополнительных действий нет — для них всегда false.
		/// </summary>
		public bool IsBonusAction { get; private set; }

		/// <summary>
		/// true — защитник выбрал парирование вместо обычного защитного навыка (см. ParryRules,
		/// BattleParticipants.GetEquippedMeleeWeaponSkill). DefensiveSkill в этом случае — навык
		/// атаки экипированного оружия ближнего боя защитника, а не выбор из Ability.DefensiveSkills.
		/// </summary>
		public bool IsParry { get; private set; }

		private BattleAttack()
		{
		}

		public BattleAttack(
			long battleId,
			ParticipantKind attackerKind,
			long attackerId,
			long abilityId,
			int attacksAllowed,
			ParticipantKind defenderKind,
			long defenderId,
			bool isBonusAction)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(battleId, nameof(battleId));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(attackerId, nameof(attackerId));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(abilityId, nameof(abilityId));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(attacksAllowed, nameof(attacksAllowed));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(defenderId, nameof(defenderId));

			BattleId = battleId;
			AttackerKind = attackerKind;
			AttackerId = attackerId;
			AbilityId = abilityId;
			AttacksAllowed = attacksAllowed;
			DefenderKind = defenderKind;
			DefenderId = defenderId;
			IsBonusAction = isBonusAction;
			Phase = BattleAttackPhase.AwaitingChoices;
		}

		public void SetTargetPart(long? creaturePartId)
		{
			EnsureAwaitingChoices();
			if (AttackerConfirmed)
			{
				throw new InvalidArgumentException(ErrorCode.AttackerAlreadyConfirmed, "Атакующий уже подтвердил свой выбор.");
			}

			TargetedCreaturePartId = creaturePartId;
		}

		public void SetTargetHumanBodyPart(HumanBodyPart? part)
		{
			EnsureAwaitingChoices();
			if (AttackerConfirmed)
			{
				throw new InvalidArgumentException(ErrorCode.AttackerAlreadyConfirmed, "Атакующий уже подтвердил свой выбор.");
			}

			TargetedHumanBodyPart = part;
		}

		// Бросок d10 в этой системе может "взрываться" (на 10 — бросить ещё и прибавить, на 1 — бросить
		// ещё и вычесть, рекурсивно), поэтому итоговое значение не ограничено диапазоном 1..10.
		public void SetAttackRoll(int? roll)
		{
			EnsureAwaitingChoices();
			if (AttackerConfirmed)
			{
				throw new InvalidArgumentException(ErrorCode.AttackerAlreadyConfirmed, "Атакующий уже подтвердил свой выбор.");
			}

			AttackRoll = roll;
		}

		public void ConfirmAttacker()
		{
			EnsureAwaitingChoices();
			if (AttackerConfirmed)
			{
				throw new InvalidArgumentException(ErrorCode.AttackerAlreadyConfirmed, "Атакующий уже подтвердил свой выбор.");
			}

			AttackerConfirmed = true;
		}

		public void SetDefenderChoice(Skill defensiveSkill, int? defenseRoll, bool isParry = false)
		{
			EnsureAwaitingChoices();
			if (DefenderConfirmed)
			{
				throw new InvalidArgumentException(ErrorCode.DefenderAlreadyConfirmed, "Защитник уже подтвердил свой выбор.");
			}

			DefensiveSkill = defensiveSkill;
			DefenseRoll = defenseRoll;
			IsParry = isParry;
		}

		/// <summary>
		/// Оглушённый защитник не выбирает защитный навык — его защита фиксированно равна 10 (см.
		/// BattleCombatCalculator.ResolveHit), поэтому проверка на выбранный навык для него снимается.
		/// </summary>
		public void ConfirmDefender(bool defenderIsStunned = false)
		{
			EnsureAwaitingChoices();
			if (DefenderConfirmed)
			{
				throw new InvalidArgumentException(ErrorCode.DefenderAlreadyConfirmed, "Защитник уже подтвердил свой выбор.");
			}

			if (DefensiveSkill is null && !defenderIsStunned)
			{
				throw new InvalidArgumentException(ErrorCode.InvalidDefensiveSkillChoice, "Защитник должен выбрать защитный навык.");
			}

			DefenderConfirmed = true;
		}

		/// <summary>
		/// Вызывается сервисом, когда обе стороны подтвердили выбор и попадание посчитано. Заодно
		/// фиксирует фактически использованные значения бросков (свои — если введены вручную,
		/// иначе — брошенные сервером) и итоги встречного броска, чтобы лог боя мог их показать.
		/// </summary>
		public void MarkHitResolved(
			bool succeeded,
			long? resolvedCreaturePartId,
			HumanBodyPart? resolvedHumanBodyPart,
			int attackRollUsed,
			int attackTotal,
			int defenseRollUsed,
			int defenseTotal)
		{
			EnsureAwaitingChoices();
			if (!AttackerConfirmed || !DefenderConfirmed)
			{
				throw new InvalidArgumentException(ErrorCode.AttackNotInExpectedPhase, "Обе стороны должны подтвердить выбор перед разрешением попадания.");
			}

			AttackRoll = attackRollUsed;
			AttackTotal = attackTotal;
			DefenseRoll = defenseRollUsed;
			DefenseTotal = defenseTotal;
			LastHitSucceeded = succeeded;
			ResolvedCreaturePartId = resolvedCreaturePartId;
			ResolvedHumanBodyPart = resolvedHumanBodyPart;
			Phase = succeeded ? BattleAttackPhase.AwaitingDamageRoll : BattleAttackPhase.SwingResolved;
			if (!succeeded)
			{
				AttacksUsed++;
			}
		}

		public void SetDamageRoll(int? roll)
		{
			if (Phase != BattleAttackPhase.AwaitingDamageRoll)
			{
				throw new InvalidArgumentException(ErrorCode.AttackNotInExpectedPhase, "Сейчас не ожидается ввод броска урона.");
			}

			DamageRoll = roll;
		}

		/// <summary>
		/// requiresStunSave — true, если этим попаданием прошла попытка наложить Condition.Stun (роль
		/// ApplyChance-проверки уже сыграна в BattleCombatCalculator.RollAppliedConditions) — тогда
		/// выпад не считается разрешённым, пока защитник не пройдёт stun save (см. SetStunSaveRoll/
		/// ResolveStunSave ниже), а не сразу переходит в SwingResolved.
		/// </summary>
		public void MarkDamageResolved(bool requiresStunSave)
		{
			if (Phase != BattleAttackPhase.AwaitingDamageRoll)
			{
				throw new InvalidArgumentException(ErrorCode.AttackNotInExpectedPhase, "Сейчас не ожидается расчёт урона.");
			}

			Phase = requiresStunSave ? BattleAttackPhase.AwaitingStunSave : BattleAttackPhase.SwingResolved;
			AttacksUsed++;
		}

		/// <summary>Бросок д10 в этой системе может "взрываться" — см. комментарий у SetAttackRoll.</summary>
		public void SetStunSaveRoll(int? roll)
		{
			if (Phase != BattleAttackPhase.AwaitingStunSave)
			{
				throw new InvalidArgumentException(ErrorCode.AttackNotInExpectedPhase, "Сейчас не ожидается проверка Оглушения.");
			}

			StunSaveRoll = roll;
		}

		public void ResolveStunSave(int rollUsed, bool succeeded)
		{
			if (Phase != BattleAttackPhase.AwaitingStunSave)
			{
				throw new InvalidArgumentException(ErrorCode.AttackNotInExpectedPhase, "Сейчас не ожидается проверка Оглушения.");
			}

			StunSaveRoll = rollUsed;
			StunSaveSucceeded = succeeded;
			Phase = BattleAttackPhase.SwingResolved;
		}

		public void PrepareNextSwing(ParticipantKind defenderKind, long defenderId)
		{
			if (Phase != BattleAttackPhase.SwingResolved)
			{
				throw new InvalidArgumentException(ErrorCode.AttackNotInExpectedPhase, "Предыдущий выпад ещё не разрешён.");
			}

			if (AttacksUsed >= AttacksAllowed)
			{
				throw new InvalidArgumentException(ErrorCode.NoAttacksRemaining, "Атаки этой способностью в этот ход исчерпаны.");
			}

			InvalidArgumentException.ThrowIfLessOrEqualToZero(defenderId, nameof(defenderId));

			DefenderKind = defenderKind;
			DefenderId = defenderId;
			TargetedCreaturePartId = null;
			TargetedHumanBodyPart = null;
			AttackRoll = null;
			AttackTotal = 0;
			AttackerConfirmed = false;
			DefensiveSkill = null;
			DefenseRoll = null;
			DefenseTotal = 0;
			DefenderConfirmed = false;
			IsParry = false;
			Phase = BattleAttackPhase.AwaitingChoices;
			LastHitSucceeded = null;
			ResolvedCreaturePartId = null;
			ResolvedHumanBodyPart = null;
			DamageRoll = null;
			StunSaveRoll = null;
			StunSaveSucceeded = null;
		}

		private void EnsureAwaitingChoices()
		{
			if (Phase != BattleAttackPhase.AwaitingChoices)
			{
				throw new InvalidArgumentException(ErrorCode.AttackNotInExpectedPhase, "Сейчас не ожидается выбор атакующего/защитника.");
			}
		}
	}
}
