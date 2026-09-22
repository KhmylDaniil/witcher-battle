namespace Wastelands.Service.Domain.Drafts
{
	/// <summary>
	/// Правила дополнительного действия персонажа за ход (см. BattleCharacter.HasActedThisTurn) — общие
	/// для любого действия, которое может быть взято вторым: атаки (BattleAttack.IsBonusAction) и
	/// попытки снять состояние (BattleCombatService.AttemptRemoveConditionAsync). У существ
	/// дополнительных действий нет вовсе.
	/// </summary>
	public static class BonusActionRules
	{
		public const int StaminaCost = 3;

		public const int RollPenalty = 3;
	}
}
