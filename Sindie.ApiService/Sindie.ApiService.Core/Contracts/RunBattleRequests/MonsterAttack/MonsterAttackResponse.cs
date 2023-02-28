
namespace Sindie.ApiService.Core.Contracts.BattleRequests.MonsterAttack
{
	/// <summary>
	/// Ответ на атаку монстра
	/// </summary>
	public sealed class MonsterAttackResponse
	{
		/// <summary>
		/// Сообщение о результате атаки
		/// </summary>
		public string Message { get; set; }
	}
}
