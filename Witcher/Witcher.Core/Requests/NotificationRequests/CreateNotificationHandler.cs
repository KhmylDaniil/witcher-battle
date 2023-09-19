using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.NotificationRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Notifications;

namespace Witcher.Core.Requests.NotificationRequests
{
	public sealed class CreateNotificationHandler : BaseHandler<CreateNotificationCommand, Unit>
	{
		public CreateNotificationHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<Unit> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
		{
			var receiver = await _appDbContext.Users
				.Include(u => u.Notifications)
				.FirstOrDefaultAsync(x => x.Id == request.ReceiverId)
					?? throw new EntityNotFoundException<User>(request.ReceiverId);

		
			var notification = new Notification { User = receiver, Message = request.Message };

			receiver.Notifications.Add(notification);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
