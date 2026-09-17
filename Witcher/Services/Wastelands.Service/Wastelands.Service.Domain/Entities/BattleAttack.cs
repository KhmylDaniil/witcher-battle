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

		public Skill? DefensiveSkill { get; private set; }

		/// <summary>Ручной ввод броска защитника. Null — при разрешении атаки бросает сервер.</summary>
		public int? DefenseRoll { get; private set; }

		public bool DefenderConfirmed { get; private set; }

		public BattleAttackPhase Phase { get; private set; }

		public bool? LastHitSucceeded { get; private set; }

		/// <summary>
		/// Часть тела, в которую фактически попала атака (явный выбор атакующего либо случайно
		/// определённая сервисом при разрешении попадания). Null, пока попадание не разрешено, или
		/// если защитник — персонаж (у него нет частей тела).
		/// </summary>
		public long? ResolvedCreaturePartId { get; private set; }

		/// <summary>Ручной ввод суммы броска урона. Null — при разрешении урона бросает сервер.</summary>
		public int? DamageRoll { get; private set; }

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
			long defenderId)
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

		public void SetAttackRoll(int? roll)
		{
			EnsureAwaitingChoices();
			if (AttackerConfirmed)
			{
				throw new InvalidArgumentException(ErrorCode.AttackerAlreadyConfirmed, "Атакующий уже подтвердил свой выбор.");
			}

			if (roll is not null)
			{
				InvalidArgumentException.ThrowIfNotInRange(roll.Value, 1, 10, nameof(roll));
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

		public void SetDefenderChoice(Skill defensiveSkill, int? defenseRoll)
		{
			EnsureAwaitingChoices();
			if (DefenderConfirmed)
			{
				throw new InvalidArgumentException(ErrorCode.DefenderAlreadyConfirmed, "Защитник уже подтвердил свой выбор.");
			}

			if (defenseRoll is not null)
			{
				InvalidArgumentException.ThrowIfNotInRange(defenseRoll.Value, 1, 10, nameof(defenseRoll));
			}

			DefensiveSkill = defensiveSkill;
			DefenseRoll = defenseRoll;
		}

		public void ConfirmDefender()
		{
			EnsureAwaitingChoices();
			if (DefenderConfirmed)
			{
				throw new InvalidArgumentException(ErrorCode.DefenderAlreadyConfirmed, "Защитник уже подтвердил свой выбор.");
			}

			if (DefensiveSkill is null)
			{
				throw new InvalidArgumentException(ErrorCode.InvalidDefensiveSkillChoice, "Защитник должен выбрать защитный навык.");
			}

			DefenderConfirmed = true;
		}

		/// <summary>Вызывается сервисом, когда обе стороны подтвердили выбор и попадание посчитано.</summary>
		public void MarkHitResolved(bool succeeded, long? resolvedCreaturePartId)
		{
			EnsureAwaitingChoices();
			if (!AttackerConfirmed || !DefenderConfirmed)
			{
				throw new InvalidArgumentException(ErrorCode.AttackNotInExpectedPhase, "Обе стороны должны подтвердить выбор перед разрешением попадания.");
			}

			LastHitSucceeded = succeeded;
			ResolvedCreaturePartId = resolvedCreaturePartId;
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

		public void MarkDamageResolved()
		{
			if (Phase != BattleAttackPhase.AwaitingDamageRoll)
			{
				throw new InvalidArgumentException(ErrorCode.AttackNotInExpectedPhase, "Сейчас не ожидается расчёт урона.");
			}

			Phase = BattleAttackPhase.SwingResolved;
			AttacksUsed++;
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
			AttackRoll = null;
			AttackerConfirmed = false;
			DefensiveSkill = null;
			DefenseRoll = null;
			DefenderConfirmed = false;
			Phase = BattleAttackPhase.AwaitingChoices;
			LastHitSucceeded = null;
			ResolvedCreaturePartId = null;
			DamageRoll = null;
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
