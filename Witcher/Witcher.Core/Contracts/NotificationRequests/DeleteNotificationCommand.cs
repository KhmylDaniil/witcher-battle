using MediatR;
using System;

namespace Witcher.Core.Contracts.NotificationRequests
{
	public sealed class DeleteNotificationCommand : IRequest
	{
		public Guid Id { get; set; }
	}
}
