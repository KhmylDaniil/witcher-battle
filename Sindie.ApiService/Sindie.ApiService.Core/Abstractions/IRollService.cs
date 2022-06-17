
namespace Sindie.ApiService.Core.Abstractions
{
	public interface IRollService
	{
		/// <summary>
		/// Бросок атаки
		/// </summary>
		/// <param name="attackValue">База атаки</param>
		/// <param name="defenseValue">База защиты</param>
		/// <returns></returns>
		public int RollAttack(int attackValue, int defenseValue);
	}
}
