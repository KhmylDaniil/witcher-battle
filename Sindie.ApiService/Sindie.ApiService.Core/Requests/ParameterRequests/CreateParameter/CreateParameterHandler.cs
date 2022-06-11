using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.ParameterRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.ParameterRequests.CreateParameter
{
	/// <summary>
	/// Обработчик команды создания параметра
	/// </summary>
	public class CreateParameterHandler : IRequestHandler<CreateParameterCommand>
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
		/// Конструктор обработчика создания параметра
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public CreateParameterHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Создание параметра параметра
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns>Ответ</returns>
		public async Task<Unit> Handle
			(CreateParameterCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(x => x.Parameters)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			if (game.Parameters.Any(x => x.Name == request.Name))
				throw new ExceptionRequestNameNotUniq<CreateParameterCommand>(nameof(request.Name));

			var parameter = new Parameter(
				game: game,
				name: request.Name,
				description: request.Description,
				minValueParameter: request.MinValueParameter,
				maxValueParameter: request.MaxValueParameter);

			_appDbContext.Parameters.Add(parameter);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}