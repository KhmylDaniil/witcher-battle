using System;
using Witcher.Core.Abstractions;
using Witcher.Core.Notifications;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Contracts.GameRequests
{
	public class JoinGameRequest : IValidatableCommand<JoinGameRequestNotification>
	{
		public Guid UserId { get; set; }

		public Guid Id { get; set; }

		public string Message { get; set; }

		public void Validate()
		{
			if (Message.Length > 100)
				throw new RequestFieldIncorrectDataException<JoinGameRequest>(Message, "Длина сообщения превышает 100 символов.");
		}
	}
}
