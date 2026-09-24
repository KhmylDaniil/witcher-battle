namespace Wastelands.Service.Domain.Drafts
{
	/// <summary>
	/// Критический провал (fumble) броска атаки/защиты в бою — см. BattleFumbleResolver. Срабатывает
	/// только на явно введённый игроком бросок: RollDie сервера (BattleCombatCalculator) никогда не
	/// даёт отрицательных значений — только "взрывающийся" вручную введённый д10 (см. комментарий у
	/// BattleAttack.SetAttackRoll) может уйти в минус настолько, чтобы считаться fumble.
	/// </summary>
	public static class FumbleRules
	{
		/// <summary>Бросок должен быть -5 или ниже, чтобы вообще считаться fumble.</summary>
		public const int Threshold = -5;

		/// <summary>
		/// Тир последствий — |бросок|, начиная с 6 (таблицы последствий начинаются с тира 6). При
		/// броске ровно -5 (Threshold) fumble фиксируется в логе, но ни один тир не применяется —
		/// возвращает null.
		/// </summary>
		public static int? GetTier(int roll)
		{
			if (roll > Threshold)
			{
				return null;
			}

			var magnitude = Math.Abs(roll);
			return magnitude >= 6 ? magnitude : null;
		}
	}
}
