using Microsoft.AspNetCore.SignalR;

namespace Witcher.MVC.Hubs
{
	public class MessageHub : Hub
	{
		public async Task SendMessage(string user, string message)
		{
			await Clients.All.SendAsync("ReceiveMessage", user, message);
		}
	}
}
