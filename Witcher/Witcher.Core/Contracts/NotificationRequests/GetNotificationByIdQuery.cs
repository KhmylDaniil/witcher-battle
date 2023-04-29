using System;

using Witcher.Core.Abstractions;
using Witcher.Core.Notifications;

namespace Witcher.Core.Contracts.NotificationRequests
{
	public class GetNotificationByIdQuery : IValidatableCommand<Notification>
	{
		public Guid Id { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
