using MediatR;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.UserGameRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sindie.ApiService.Core.Requests.UserGameRequests
{
	public class ChangeUserGameHandler : BaseHandler<ChangeUserGameCommand, Unit>
	{
		/// <summary>
		/// Конструктор обработчика команды изменения пользователя игры
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public ChangeUserGameHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService)
		{
		}

		/// <summary>
		/// Обработка команды иизменения пользователя игры
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Юнит</returns>
		public override async Task<Unit> Handle(ChangeUserGameCommand request, CancellationToken cancellationToken)
		{
			var userGame = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games, GameRoles.MainMasterRoleId)
				.SelectMany(g => g.UserGames)
				.FirstOrDefaultAsync(ug => ug.UserId == request.UserId, cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			if (userGame.GameRoleId == GameRoles.MainMasterRoleId)
				throw new RequestValidationException("Роль главного мастера не может быть изменена");

			var requestedGameRoleId = userGame.GameRoleId == GameRoles.MasterRoleId
				? GameRoles.PlayerRoleId
				: GameRoles.MasterRoleId;

			userGame.GameRole = await _appDbContext.GameRoles.FirstOrDefaultAsync(g => g.Id == requestedGameRoleId, cancellationToken)
				?? throw new ExceptionEntityNotFound<GameRole>(requestedGameRoleId);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
