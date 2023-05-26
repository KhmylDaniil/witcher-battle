using System.ComponentModel.Design;
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
			int baseAttackSpeed = 0;

			if (attackFormula is WeaponTemplate && isStrongAttack is not null && !isStrongAttack.Value)
				baseAttackSpeed = 2;
			else if (attackFormula is Ability ability)
					baseAttackSpeed = ability.AttackSpeed;

			if (attacker.Turn.MuitiattackAttackFormulaId is null && baseAttackSpeed > 1)
			{
				attacker.Turn.MuitiattackAttackFormulaId = attackFormula.Id;
				attacker.Turn.MultiattackRemainsQuantity = baseAttackSpeed;
				attacker.Turn.TurnState++;
			}

			if (--attacker.Turn.MultiattackRemainsQuantity > 0)
				return;

			attacker.Turn.MuitiattackAttackFormulaId = null;
		}

		private static void MarkAttackAsDone(Creature attacker)
		{
			if (attacker.Turn.MuitiattackAttackFormulaId is null)
				attacker.Turn.TurnState = attacker.Turn.TurnState < TurnState.BaseActionIsDone
					? TurnState.BaseActionIsDone
					: TurnState.TurnIsDone;
		}
	}
}
