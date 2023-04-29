using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.GameRequests;
using Witcher.Core.Notifications;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;

namespace Witcher.Core.Requests.GameRequests
{
	public class AskForJoinHandler : BaseHandler<JoinGameRequest, JoinGameRequestNotification>
	{
		public AskForJoinHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<JoinGameRequestNotification> Handle(JoinGameRequest request, CancellationToken cancellationToken)
		{
			var game = await _appDbContext.Games
				.Include(g => g.UserGames.Where(ug => ug.GameRoleId == GameRoles.MainMasterRoleId))
				.ThenInclude(ug => ug.User)
				.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
				?? throw new EntityNotFoundException<Game>(request.Id);

			var sender = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId)
				?? throw new EntityNotFoundException<User>(request.UserId);

			var receiver = game.UserGames.First(x => x.GameRoleId == GameRoles.MainMasterRoleId).User;

			var notification = new JoinGameRequestNotification(sender, receiver, game, request.Message);

			receiver.Notifications.Add(notification);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return notification;
		}
	}
}
