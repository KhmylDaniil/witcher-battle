using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.ModifierRequests.DeleteModifier;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.ModifierRequests.DeleteModifier
{
	/// <summary>
	/// Обработчик команды удаления модификатора
	/// </summary>
	public class DeleteModifierHandler: IRequestHandler<DeleteModifierCommand, Unit>
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
		/// Конструктор обработчика удаления модификатора
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public DeleteModifierHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Обработка запроса удаления модификатора
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Юнит</returns>
		public async Task<Unit> Handle(DeleteModifierCommand request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ExceptionRequestNull<DeleteModifierCommand>();

			var modifier = await _authorizationService.RoleGameFilter(
				_appDbContext.Games, request.GameId, GameRoles.MasterRoleId)
				.SelectMany(x => x.Modifiers)
				.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
				??	throw new ExceptionEntityNotFound<Modifier>();

			_appDbContext.Modifiers.Remove(modifier);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
