using MediatR;
using System;

namespace Witcher.Core.Contracts.NotificationRequests
{
	public sealed class DeleteNotificationCommand : IRequest<Unit>
	{
		public Guid Id { get; set; }
	}
}
