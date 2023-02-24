using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.UserGameRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.UserGameRequests
{
	/// <summary>
	/// Обработчик команды создания пользователя игры
	/// </summary>
	public class CreateUserGameHandler : BaseHandler<CreateUserGameCommand, Unit>
	{
		/// <summary>
		/// Контекст пользователя
		/// </summary>
		private readonly IUserContext _userContext;

		public CreateUserGameHandler(
			IAppDbContext appDbContext, 
			IAuthorizationService authorizationService,
			IUserContext userContext)
			: base(appDbContext, authorizationService)
		{
			_userContext = userContext;
		}

		/// <summary>
		/// Обработка команды создания пользователя игры
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Юнит</returns>
		public override async Task<Unit> Handle(CreateUserGameCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.UserGames)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionNoAccessToEntity<Game>();

			var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == request.AssignedUserId, cancellationToken)
				?? throw new ExceptionEntityNotFound<User>(request.AssignedUserId);

			var role = await _appDbContext.GameRoles
				.FirstOrDefaultAsync(u => u.Id == request.AssingedRoleId, cancellationToken)
				?? throw new ExceptionEntityNotFound<GameRole>(request.AssingedRoleId);

			if (game.UserGames.Any(x => x.UserId == request.AssignedUserId
				&& x.GameRoleId == request.AssingedRoleId))
				throw new RequestNotUniqException<CreateUserGameCommand>();

			var @interface = await _appDbContext.Interfaces
				.FirstOrDefaultAsync(u => u.Id == SystemInterfaces.GameDarkId, cancellationToken)
				?? throw new ExceptionEntityNotFound<Interface>(SystemInterfaces.GameDarkId);

			if (request.AssingedRoleId == GameRoles.MasterRoleId
				&& !game.UserGames.Any(x => x.UserId == _userContext.CurrentUserId
					&& x.GameRoleId == GameRoles.MainMasterRoleId))
				throw new RequestValidationException("Нет прав для присвоения роли");
			
			game.UserGames.Add(new UserGame(user, game, @interface, role));
	
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
