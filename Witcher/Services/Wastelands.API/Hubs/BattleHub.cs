using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;

namespace Wastelands.API.Hubs
{
	/// <summary>
	/// Хаб не передаёт данные боя — только голое событие "BattleUpdated", по которому клиент
	/// перезапрашивает GET /battles/{id} как обычно (см. BattleNotifier). Видимость боя (кто может
	/// подключиться к его группе) переиспользует тот же скоупинг, что и REST — GetBattleByIdAsync
	/// бросает NotFoundException для боя, недоступного текущему пользователю.
	/// </summary>
	[Authorize]
	public class BattleHub : Hub
	{
		private readonly IBattleService _battleService;

		public BattleHub(IBattleService battleService)
		{
			_battleService = battleService;
		}

		public async Task JoinBattle(long battleId)
		{
			try
			{
				await _battleService.GetBattleByIdAsync(battleId);
			}
			catch (NotFoundException)
			{
				return;
			}

			await Groups.AddToGroupAsync(Context.ConnectionId, GroupName(battleId));
		}

		public async Task LeaveBattle(long battleId)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName(battleId));
		}

		public static string GroupName(long battleId) => $"battle-{battleId}";
	}
}
