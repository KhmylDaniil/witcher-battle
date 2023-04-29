using System;
using System.Collections.Generic;
using Witcher.Core.Abstractions;
using Witcher.Core.Notifications;

namespace Witcher.Core.Contracts.NotificationRequests
{
	public class GetNotificationsQuery : GetBaseQuery, IValidatableCommand<IEnumerable<Notification>>
	{
		public bool OnlyNotReaded { get; set; }

		public void Validate()
		{
			throw new NotImplementedException();
		}
	}
}
