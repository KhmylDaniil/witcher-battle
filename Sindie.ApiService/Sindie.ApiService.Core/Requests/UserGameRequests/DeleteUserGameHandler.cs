using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.UserGameRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.UserGameRequests
{
	public class DeleteUserGameHandler : BaseHandler<DeleteUserGameCommand, Unit>
	{
		/// <summary>
		/// Контекст пользователя
		/// </summary>
		private readonly IUserContext _userContext;

		/// <summary>
		/// Конструктор обработчика команды удаления пользователя игры
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
				.ThenInclude(ug => ug.User)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionNoAccessToEntity<Game>();
			
			var userGame = game.UserGames.FirstOrDefault(x => x.UserId == request.UserId)
				?? throw new ExceptionEntityNotFound<UserGame>(request.UserId);

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
