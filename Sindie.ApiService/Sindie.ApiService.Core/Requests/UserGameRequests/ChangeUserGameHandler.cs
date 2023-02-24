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
	public class ChangeUserGameHandler : BaseHandler<ChangeUserGameCommand, Unit>
	{
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
			IAuthorizationService authorizationService,
			IUserContext userContext)
			: base(appDbContext, authorizationService)
		{
			_userContext = userContext;
		}

		/// <summary>
		/// Обработка команды изменения пользователя игры
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Юнит</returns>
		public override async Task<Unit> Handle(ChangeUserGameCommand request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new RequestNullException<ChangeUserGameCommand>();
			
			var userGames = await _authorizationService.UserGameFilter(_appDbContext.Games)
				.Include(g => g.UserGames)
				.SelectMany(g => g.UserGames)
				.Where(ug => ug.UserId == _userContext.CurrentUserId)
				.ToListAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<UserGame>();

			var @interface = await _appDbContext.Interfaces
				.FirstOrDefaultAsync(u => u.Id == request.InterfaceId, cancellationToken)
				?? throw new ExceptionEntityNotFound<Interface>(request.InterfaceId);

			foreach (var game in userGames)
				game.Interface = @interface;

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
