using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.ParameterRequests.ChangeParameter
{
	/// <summary>
	/// Обработчик изменения параметра
	/// </summary>
	public class ChangeParameterHandler: IRequest<ChangeParameterCommand>
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
		/// Конструктор обработчика изменения параметра
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public ChangeParameterHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Изменение параметра параметра
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns>Ответ</returns>
		public async Task<Unit> Handle
			(ChangeParameterCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(x => x.Parameters)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			if (game.Parameters.Any(x => x.Name == request.Name && x.Id != request.Id))
				throw new ExceptionRequestNameNotUniq<ChangeParameterCommand>(nameof(request.Name));

			var parameter = game.Parameters.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<Parameter>(request.Id);

			parameter.ChangeParameter(
				game: game,
				name: request.Name,
				description: request.Description,
				minValueParameter: request.MinValueParameter,
				maxValueParameter: request.MaxValueParameter);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
