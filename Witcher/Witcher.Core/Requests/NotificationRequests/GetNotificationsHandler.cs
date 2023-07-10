using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.NotificationRequests;
using Witcher.Core.ExtensionMethods;
using Witcher.Core.Notifications;

namespace Witcher.Core.Requests.NotificationRequests
{
	public class GetNotificationsHandler : BaseHandler<GetNotificationsQuery, IEnumerable<Notification>>
	{
		private readonly IUserContext _userContext;

		public GetNotificationsHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IUserContext userContext)
			: base(appDbContext, authorizationService)
		{
			_userContext = userContext;
		}

		public async override Task<IEnumerable<Notification>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
		{
			var filter = _appDbContext.Users.Include(u => u.Notifications)
				.Where(u => u.Id == _userContext.CurrentUserId)
				.SelectMany(u => u.Notifications
				.Where(n => !request.OnlyNotReaded || !n.IsReaded));

			return await filter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.ToListAsync(cancellationToken);
		}
	}
}
