using MediatR;
using System;

namespace Witcher.Core.Contracts.NotificationRequests
{
	public sealed class CreateNotificationCommand : IRequest<Unit>
	{
		public Guid ReceiverId { get; set; }
		
		public string Message { get; set; }
	}
}
