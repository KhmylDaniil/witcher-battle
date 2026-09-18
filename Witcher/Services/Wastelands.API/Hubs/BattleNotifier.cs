using Microsoft.AspNetCore.SignalR;
using Wastelands.Service.Application.Contracts;

namespace Wastelands.API.Hubs
{
	public class BattleNotifier : IBattleNotifier
	{
		private readonly IHubContext<BattleHub> _hubContext;

		public BattleNotifier(IHubContext<BattleHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public async Task NotifyBattleUpdatedAsync(long battleId)
		{
			await _hubContext.Clients.Group(BattleHub.GroupName(battleId)).SendAsync("BattleUpdated");
		}
	}
}
