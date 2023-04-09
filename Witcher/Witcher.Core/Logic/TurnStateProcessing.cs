using Witcher.Core.Abstractions;
using Witcher.Core.Entities;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Logic
{
	public static class TurnStateProcessing
	{
		internal static void EndOfAttackProcessing(AttackData attackData)
		{
			CheckAttackerTurnStateWithMultiAttacks(attackData.Attacker, attackData.AttackFormula, attackData.isStrongWeaponAttack);
			MarkAttackAsDone(attackData.Attacker);
		}
		
		private static void CheckAttackerTurnStateWithMultiAttacks(Creature attacker, IAttackFormula attackFormula, bool? isStrongAttack)
		{
			if (attackFormula is Weapon && (isStrongAttack is null || isStrongAttack.Value))
				return;

			int attackSpeed = 2;

			if (attackFormula is Ability ability)
				if (ability.AttackSpeed > 1)
					attackSpeed = ability.AttackSpeed;
				else
					return;

			if (attacker.Turn.MuitiattackAttackFormulaId is null)
			{
				attacker.Turn.MuitiattackAttackFormulaId = attackFormula.Id;
				attacker.Turn.MultiattackRemainsQuantity = attackSpeed;
				attacker.Turn.TurnState++;
			}

			attacker.Turn.MultiattackRemainsQuantity--;

			if (attacker.Turn.MultiattackRemainsQuantity > 0)
				return;

			attacker.Turn.MuitiattackAttackFormulaId = null;
		}

		private static void MarkAttackAsDone(Creature attacker)
		{
			if (attacker.Turn.MuitiattackAttackFormulaId is null)
				attacker.Turn.TurnState = attacker.Turn.TurnState == TurnState.ReadyForAction
					? TurnState.BaseActionIsDone
					: TurnState.TurnIsDone;
		}
	}
}
