using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.GameRequests.DeleteGame;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.GameRequests.DeleteGame
{
	public class DeleteGameHandler: IRequestHandler<DeleteGameCommand, Unit>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Сервис авторизации
		/// </summary>
		private readonly IAuthorizationService _authorizationService;

		public DeleteGameHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		public async Task<Unit> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.UserGameFilter(_appDbContext.Games, request.Id)
				.FirstAsync(cancellationToken: cancellationToken);

			_appDbContext.Games.Remove(game);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
