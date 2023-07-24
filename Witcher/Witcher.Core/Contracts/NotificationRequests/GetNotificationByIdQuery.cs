using MediatR;
using System;
using Witcher.Core.Notifications;

namespace Witcher.Core.Contracts.NotificationRequests
{
	public sealed class GetNotificationByIdQuery : IRequest<Notification>
	{
		public Guid Id { get; set; }
	}
}
