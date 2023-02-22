using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.UserGameRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.UserGameRequests
{
	public class DeleteUserGameHandler : IRequestHandler<DeleteUserGameCommand, Unit>

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
		/// Контекст пользователя
		/// </summary>
		private readonly IUserContext _userContext;

		/// <summary>
		/// Конструктор обработчика команды изменения пользователя игры
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		/// <param name="userContext">Контекст пользователя</param>
		public DeleteUserGameHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService,
			IUserContext userContext)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
			_userContext = userContext;
		}

		/// <summary>
		/// Обработка команды удаления пользователя игры
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Юнит</returns>
		public async Task<Unit> Handle(DeleteUserGameCommand request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ExceptionRequestNull<DeleteUserGameCommand>();

			var game = await _authorizationService.RoleGameFilter(
				_appDbContext.Games, request.GameId, GameRoles.MasterRoleId)
				.Include(x => x.UserGames)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionEntityNotFound<Game>(request.GameId);
			
			var userGame = game.UserGames.FirstOrDefault(x => x.Id == request.UserGameId)
				?? throw new ExceptionEntityNotFound<UserGame>(request.UserGameId);

			if (game.UserGames.Where(x => x.GameRoleId == GameRoles.MainMasterRoleId).Count() == 1
				&& userGame.GameRoleId == GameRoles.MainMasterRoleId)
				throw new ExceptionNoAccessToEntity<UserGame>();

			bool IsMainMaster = game.UserGames.Any(x => x.GameRoleId == GameRoles.MainMasterRoleId
				&& x.UserId == _userContext.CurrentUserId);

			if (userGame.UserId == _userContext.CurrentUserId)
				game.UserGames.Remove(userGame);
			else if (IsMainMaster && userGame.GameRoleId != GameRoles.MainMasterRoleId)
				game.UserGames.Remove(userGame);
			else if (userGame.GameRoleId == GameRoles.PlayerRoleId)
				game.UserGames.Remove(userGame);
			else
				throw new ExceptionNoAccessToEntity<UserGame>();

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
