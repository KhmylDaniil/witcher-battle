namespace Wastelands.Service.Domain.Drafts
{
	/// <summary>
	/// Парирование — альтернатива обычному защитному навыку, доступная защитнику с экипированным
	/// оружием ближнего боя (Item.WeaponKind == WeaponKind.Melee, см. BattleParticipants.GetEquippedMeleeWeaponSkill).
	/// Бросок делается тем навыком, которым обычно атакуют этим оружием, со штрафом RollPenalty.
	/// Успех (итог защиты ≥ итога атаки) — атака полностью отражена, урона нет, атакующий получает
	/// Condition.Staggered (см. BattleHitResolver.ResolveIfBothConfirmedAsync).
	/// </summary>
	public static class ParryRules
	{
		public const int RollPenalty = 3;
	}
}
