using MediatR;
using System.Collections.Generic;
using Witcher.Core.Contracts.BaseRequests;
using Witcher.Core.Notifications;

namespace Witcher.Core.Contracts.NotificationRequests
{
	public sealed class GetNotificationsQuery : GetBaseQuery, IRequest<IEnumerable<Notification>>
	{
		public bool OnlyNotReaded { get; set; }
	}
}
