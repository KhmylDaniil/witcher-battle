using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.UnitTest.TestSupport
{
	internal static class BattleAttackBuilder
	{
		public static BattleAttack Fresh(
			ParticipantKind defenderKind = ParticipantKind.Character,
			long defenderId = 2,
			bool isBonusAction = false,
			int attacksAllowed = 1)
			=> new(battleId: 1, ParticipantKind.Character, attackerId: 1, abilityId: 1, attacksAllowed, defenderKind, defenderId, isBonusAction);

		/// <summary>
		/// Гоняет атаку через фазы вплоть до AwaitingDamageRoll — нужно тестам CalculateDamage, которым
		/// требуется DamageRoll/ResolvedCreaturePartId/ResolvedHumanBodyPart, доступные только после
		/// MarkHitResolved(succeeded: true, ...) по официальному API BattleAttack.
		/// </summary>
		public static BattleAttack AwaitingDamageRoll(
			ParticipantKind defenderKind,
			long defenderId,
			long? targetCreaturePartId,
			HumanBodyPart? targetHumanBodyPart,
			int? damageRoll,
			int attackTotal = 0,
			int defenseTotal = 0)
		{
			var attack = Fresh(defenderKind, defenderId);
			attack.SetTargetPart(targetCreaturePartId);
			attack.SetTargetHumanBodyPart(targetHumanBodyPart);
			attack.SetAttackRoll(0);
			attack.ConfirmAttacker();
			attack.SetDefenderChoice(Skill.Dodge, 0);
			attack.ConfirmDefender();
			attack.MarkHitResolved(true, targetCreaturePartId, targetHumanBodyPart, 0, attackTotal, 0, defenseTotal);
			if (damageRoll is not null)
			{
				attack.SetDamageRoll(damageRoll);
			}

			return attack;
		}
	}
}
