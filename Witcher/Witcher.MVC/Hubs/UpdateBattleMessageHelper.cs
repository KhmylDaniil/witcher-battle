using Microsoft.AspNetCore.SignalR;

namespace Witcher.MVC.Hubs
{
	/// <summary>
	/// Update battle log command for battle participants
	/// </summary>
	public static class UpdateBattleMessageHelper
	{
		private const string UpdateBattleLogCommandName = "UpdateBattleLog";

		public static async Task SendUpdateBattleMessage(this IHubContext<MessageHub> hubContext, IReadOnlyList<string> ids)
		{
			await hubContext.Clients.Users(ids).SendAsync(UpdateBattleLogCommandName);
		}
	}
}
