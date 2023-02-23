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
	public class CreateUserGameHandler : IRequestHandler<CreateUserGameCommand, Unit>
	{

		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Сервис авторизации
		/// </summary>
		private readonly IAuthorizationService _authorizationService;

		/// <summary>
		/// Конструктор обработчика команды создания пользователя игры
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public CreateUserGameHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Обработка команды создания пользователя игры
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Юнит</returns>
		public async Task<Unit> Handle(CreateUserGameCommand request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new RequestNullException<CreateUserGameCommand>();

			var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == request.AssignedUserId, cancellationToken)
				?? throw new ExceptionEntityNotFound<User>(request.AssignedUserId);

			var role = await _appDbContext.GameRoles
				.FirstOrDefaultAsync(u => u.Id == request.AssingedRoleId, cancellationToken)
				?? throw new ExceptionEntityNotFound<GameRole>(request.AssingedRoleId);

			if (await _appDbContext.UserGames.
				Where(x => x.UserId == request.AssignedUserId
				&& x.GameRoleId == request.AssingedRoleId).AnyAsync())
				throw new RequestNotUniqException<CreateUserGameCommand>();

			var @interface = await _appDbContext.Interfaces
				.FirstOrDefaultAsync(u => u.Id == SystemInterfaces.GameDarkId, cancellationToken)
				?? throw new ExceptionEntityNotFound<Interface>(SystemInterfaces.GameDarkId);

			if (request.AssingedRoleId == GameRoles.MainMasterRoleId
				|| request.AssingedRoleId == GameRoles.MasterRoleId)
			{
				var game = await _authorizationService.AuthorizedGameFilter(
					_appDbContext.Games, GameRoles.MainMasterRoleId)
					.Include(x => x.UserGames)
					.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionEntityNotFound<Game>(request.GameId);

				game.UserGames.Add(new UserGame(user, game, @interface, role));
			}
			else
			{
				var game = await _authorizationService.AuthorizedGameFilter(
					_appDbContext.Games)
					.Include(x => x.UserGames)
					.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionEntityNotFound<Game>(request.GameId);

				game.UserGames.Add(new UserGame(user, game, @interface, role));
			}

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
