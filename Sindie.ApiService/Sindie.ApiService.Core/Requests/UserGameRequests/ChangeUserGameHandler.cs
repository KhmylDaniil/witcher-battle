using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.UserGameRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.UserGameRequests
{
	/// <summary>
	/// Обработчик команды изменения пользователя игры
	/// </summary>
	public class ChangeUserGameHandler : IRequestHandler<ChangeUserGameCommand, Unit>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Контекст пользователя
		/// </summary>
		private readonly IUserContext _userContext;

		/// <summary>
		/// Конструктор обработчика команды изменения пользователя игры
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="userContext">Контекст пользователя</param>
		public ChangeUserGameHandler(
			IAppDbContext appDbContext,
			IUserContext userContext)
		{
			_appDbContext = appDbContext;
			_userContext = userContext;
		}

		/// <summary>
		/// Обработка команды изменения пользователя игры
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Юнит</returns>
		public async Task<Unit> Handle(ChangeUserGameCommand request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new RequestNullException<ChangeUserGameCommand>();
			
			var checkedUserGame = await _appDbContext.UserGames.
				Where(x => x.Id == request.UserGameId
				&& x.UserId == _userContext.CurrentUserId)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionNoAccessToEntity<UserGame>();

			var @interface = await _appDbContext.Interfaces
				.FirstOrDefaultAsync(u => u.Id == request.InterfaceId, cancellationToken)
				?? throw new ExceptionEntityNotFound<Interface>(request.InterfaceId);

			checkedUserGame.Interface = @interface;

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
