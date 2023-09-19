﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.UserGameRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.UserGameRequests
{
	/// <summary>
	/// Обработчик команды создания пользователя игры
	/// </summary>
	public sealed class CreateUserGameHandler : BaseHandler<CreateUserGameCommand, Unit>
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
					?? throw new RequestValidationException("Необходимо зайти в игру.");

			var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken)
				?? throw new EntityNotFoundException<User>(request.UserId);

			var role = await _appDbContext.GameRoles
				.FirstOrDefaultAsync(u => u.Id == request.RoleId, cancellationToken)
					?? throw new EntityNotFoundException<GameRole>(request.RoleId);

			var @interface = await _appDbContext.Interfaces
				.FirstOrDefaultAsync(u => u.Id == SystemInterfaces.GameDarkId, cancellationToken)
				?? throw new EntityNotFoundException<Interface>(SystemInterfaces.GameDarkId);

			if (request.RoleId == GameRoles.MasterRoleId
				&& !game.UserGames.Exists(x => x.UserId == _userContext.CurrentUserId
					&& x.GameRoleId == GameRoles.MainMasterRoleId))
				throw new RequestValidationException("Нет прав для присвоения роли");

			if (game.UserGames.Exists(x => x.UserId == request.UserId))
				throw new RequestValidationException("Данный пользователь уже есть в игре");

			game.UserGames.Add(new UserGame(user, game, @interface, role));
			
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
