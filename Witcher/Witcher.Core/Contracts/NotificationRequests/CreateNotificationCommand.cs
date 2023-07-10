using System;
using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Contracts.NotificationRequests
{
	public class CreateNotificationCommand : IValidatableCommand
	{
		public Guid ReceiverId { get; set; }
		
		public string Message { get; set; }

		public void Validate()
		{
			if (Message.Length > 100)
				throw new RequestFieldIncorrectDataException<CreateNotificationCommand>(nameof(Message), "Длина сообщения превышает 100 символов.");
		}
	}
}
