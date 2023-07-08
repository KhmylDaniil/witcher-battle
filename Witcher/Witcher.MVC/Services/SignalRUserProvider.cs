using Microsoft.AspNetCore.SignalR;
using Witcher.Core.Exceptions.SystemExceptions;

namespace Witcher.MVC.Services
{
	/// <summary>
	/// UserId settings for SignalR hub
	/// </summary>
	public class SignalRUserProvider : IUserIdProvider
	{
		public string GetUserId(HubConnectionContext connection)
		{
			return connection.User?.Identity?.Name
				?? throw new ApplicationSystemNullException<SignalRUserProvider>(nameof(connection.User));
		}
	}
}
