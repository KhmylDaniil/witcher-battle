
namespace Sindie.ApiService.Core.Abstractions
{
	public interface IRollService
	{
		/// <summary>
		/// Бросок против сложности
		/// </summary>
		/// <param name="skillBase">База навыка</param>
		/// <param name="difficuty">Сложность для переброса</param>
		/// <param name="fumble">Провал броска</param>
		/// <returns>Успешность броска</returns>
		public int BeatDifficulty(int skillBase, int difficuty, out int fumble);

		/// <summary>
		/// Бросок против сложности
		/// </summary>
		/// <param name="skillBase">База навыка</param>
		/// <param name="difficulty">Сложность для переброса</param>
		/// <returns>Успешность броска</returns>
		public bool BeatDifficulty(int skillBase, int difficulty);

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
