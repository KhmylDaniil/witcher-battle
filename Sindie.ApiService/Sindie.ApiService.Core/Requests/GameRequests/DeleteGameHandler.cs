using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.GameRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.GameRequests
{
	public class DeleteGameHandler : BaseHandler<DeleteGameCommand, Unit>
	{
		public DeleteGameHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<Unit> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games, BaseData.GameRoles.MainMasterRoleId)
				.FirstOrDefaultAsync(cancellationToken: cancellationToken)
				?? throw new ExceptionNoAccessToEntity<Game>();

			_appDbContext.Games.Remove(game);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
