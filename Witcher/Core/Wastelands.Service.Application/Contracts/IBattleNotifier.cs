namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// Пуш уведомления "бой обновился" всем клиентам, подписанным на этот бой. Реализация (SignalR)
	/// живёт в MVC-слое — Application/Domain о конкретном транспорте не знают.
	/// </summary>
	public interface IBattleNotifier
	{
		Task NotifyBattleUpdatedAsync(long battleId);
	}
}
