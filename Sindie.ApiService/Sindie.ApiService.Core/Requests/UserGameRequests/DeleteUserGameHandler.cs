﻿using MediatR;
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
	public class DeleteUserGameHandler : BaseHandler<DeleteUserGameCommand, Unit>
	{
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
			: base(appDbContext, authorizationService)
		{
			_userContext = userContext;
		}

		/// <summary>
		/// Обработка команды удаления пользователя игры
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Юнит</returns>
		public override async Task<Unit> Handle(DeleteUserGameCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.UserGameFilter(_appDbContext.Games)
				.Include(x => x.UserGames)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionEntityNotFound<Game>();
			
			var userGame = game.UserGames.FirstOrDefault(x => x.Id == request.UserGameId)
				?? throw new ExceptionEntityNotFound<UserGame>(request.UserGameId);

			if (userGame.GameRoleId == GameRoles.MainMasterRoleId)
				throw new RequestValidationException("Роль главмастера не может быть удалена без удаления игры");

			bool IsMainMaster = game.UserGames.Any(x => x.GameRoleId == GameRoles.MainMasterRoleId
				&& x.UserId == _userContext.CurrentUserId);

			if (userGame.UserId == _userContext.CurrentUserId)
				game.UserGames.Remove(userGame);
			else if (IsMainMaster)
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
