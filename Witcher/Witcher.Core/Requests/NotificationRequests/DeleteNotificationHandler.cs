using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.NotificationRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;

namespace Witcher.Core.Requests.NotificationRequests
{
	public sealed class DeleteNotificationHandler : BaseHandler<DeleteNotificationCommand, Unit>
	{
		private readonly IUserContext _userContext;

		public DeleteNotificationHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IUserContext userContext)
			: base(appDbContext, authorizationService)
		{
			_userContext = userContext;
		}

		public async override Task<Unit> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
		{
			var user = await _appDbContext.Users.Include(u => u.Notifications)
				.FirstOrDefaultAsync(x => x.Id == _userContext.CurrentUserId)
				?? throw new EntityNotFoundException<User>(_userContext.CurrentUserId);

			user.Notifications.RemoveAll(x => x.Id == request.Id);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
