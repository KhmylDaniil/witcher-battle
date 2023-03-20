using Microsoft.AspNetCore.SignalR;

namespace Witcher.MVC.Hubs
{
	public static class UpdateBattleMessageHelper
	{
		private const string ReceiveMessageCommandName = "ReceiveMessage";

		public static async Task SendUpdateBattleMessage(this IHubContext<MessageHub> hubContext)
		{
			await hubContext.Clients.All.SendAsync(ReceiveMessageCommandName);
		}
	}
}
