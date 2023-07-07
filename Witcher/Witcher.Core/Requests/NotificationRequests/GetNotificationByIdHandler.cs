using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Contracts.NotificationRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Notifications;

namespace Witcher.Core.Requests.NotificationRequests
{
	public class GetNotificationByIdHandler : BaseHandler<GetNotificationByIdQuery, Notification>
	{
		private readonly IUserContext _userContext;
		public GetNotificationByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IUserContext userContext)
			: base(appDbContext, authorizationService)
		{
			_userContext = userContext;
		}

		public async override Task<Notification> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
		{
			var filter = _appDbContext.Users.Include(u => u.Notifications)
				.Where(u => u.Id == _userContext.CurrentUserId)
				.SelectMany(u => u.Notifications);

			var notification =  await filter.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
				?? throw new EntityNotFoundException<Notification>(request.Id);

			if (!notification.IsReaded)
			{
				notification.IsReaded = true;
				await _appDbContext.SaveChangesAsync(cancellationToken);
			}

			return notification;
		}
	}
}
