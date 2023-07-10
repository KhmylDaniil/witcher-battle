using System;
using System.Collections.Generic;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BaseRequests;
using Witcher.Core.Notifications;

namespace Witcher.Core.Contracts.NotificationRequests
{
	public class GetNotificationsQuery : GetBaseQuery, IValidatableCommand<IEnumerable<Notification>>
	{
		public bool OnlyNotReaded { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
