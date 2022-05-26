using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.ParameterRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.ParameterRequest
{
	/// <summary>
	/// Обработчик команды создания параметра
	/// </summary>
	public class CreateParameterHandler : IRequestHandler<CreateParameterCommand, CreateParameterResponse>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Интерфейс получения данных пользователя из веба
		/// </summary>
		private readonly IUserContext _userContext;

		/// <summary>
		/// Конструктор обработчика команды регистрации параметра
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="passwordHasher">Хеширование пароля</param>
		public CreateParameterHandler(IAppDbContext appDbContext, IUserContext userContext)
		{
			_appDbContext = appDbContext;
			_userContext = userContext;
		}

		/// <summary>
		/// Обработать запрос создания параметра
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns>Ответ</returns>
		public async Task<CreateParameterResponse> Handle
			(CreateParameterCommand request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException($"Пришел пустой запрос {typeof(CreateParameterCommand)}");
			if (request.GameId == null)
				throw new ExceptionRequestFieldNull<CreateParameterCommand>(nameof(request.GameId));
			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ExceptionRequestFieldIncorrectData<CreateParameterCommand>(nameof(request.Name));
			if (request.MaxValueParameter < request.MinValueParameter)
				throw new ExceptionRequestFieldIncorrectData<CreateParameterCommand>(nameof(request.Name));

			var existingGame = await _appDbContext.Games
				.Include(x => x.Parameters)
				.FirstOrDefaultAsync(x => x.Id == request.GameId, cancellationToken);

			var existingParameter = existingGame.Parameters
				.FirstOrDefault(x => x.Name == request.Name);
			if (existingParameter != null)
				throw new ExceptionRequestNotUniq<Parameter>("Такой параметр уже есть.");
			/*
			existingParameter = new Parameter(
				name: request.Name,
				description: request.Description,
				minValueParameter: request.MinValueParameter,
				maxValueParameter: request.MaxValueParameter,
				game: existingGame);
			*/
			await _appDbContext.Parameters.AddAsync(existingParameter, cancellationToken);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return new CreateParameterResponse()
			{ ParameterId = existingParameter.Id };
		}
	}
}