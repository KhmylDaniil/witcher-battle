
namespace Sindie.ApiService.Core.Abstractions
{
	public interface IRollService
	{
		/// <summary>
		/// Бросок атаки
		/// </summary>
		/// <param name="attackBase">База атаки</param>
		/// <param name="defenseValue">Значение защиты</param>
		/// <param name="attackerFumble">Провал атаки</param>
		/// <returns>Успешность атаки</returns>
		public int RollAttack(int attackBase, int defenseValue, out int attackerFumble);

		/// <summary>
		/// Встречный бросок
		/// </summary>
		/// <param name="attackBase">База атаки</param>
		/// <param name="defenseBase">База защиты</param>
		/// <param name="attackerFumble">Провал атаки</param>
		/// <param name="defenderFumble">Провал защиты</param>
		/// <returns>Успешность атаки</returns>
		public int ContestRoll(int attackBase, int defenseBase, out int attackerFumble, out int defenderFumble);
	}
}
