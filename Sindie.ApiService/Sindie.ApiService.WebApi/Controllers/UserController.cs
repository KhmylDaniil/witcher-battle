using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.UserRequests.GetUsers;
using Sindie.ApiService.Core.Contracts.UserRequests.LoginUser;
using Sindie.ApiService.Core.Contracts.UserRequests.RegisterUser;
using Sindie.ApiService.Core.Requests.UserRequests.RegisterUser;
using Sindie.ApiService.WebApi.Versioning;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Контроллер регистрации пользователя
	/// </summary>
	[ApiVersion(ApiVersions.V1)]
	public class UserController : ApiControllerBase
	{
		private readonly IMediator _mediator;

		/// <summary>
		/// Конструктор контроллера
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		public UserController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Регистрация
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Ответ</returns>
		[HttpPost("Register")]
		[SwaggerResponse(StatusCodes.Status200OK, type: typeof(RegisterUserCommandResponse))]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<RegisterUserCommandResponse> Register(
			[FromQuery] RegisterUserRequest request,
			CancellationToken cancellationToken)
		{
			return await _mediator.Send(
				request == null
				? new RegisterUserCommand()
				: new RegisterUserCommand(request)
				{
					Name = request.Name,
					Email = request.Email,
					Phone = request.Phone,
					Login = request.Login,
					Password = request.Password
				},
				cancellationToken);
		}

		/// <summary>
		/// Аутентификация
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Ответ</returns>
		[HttpPost("Login")]
		public async Task<LoginUserCommandResponse> Login([FromBody] LoginUserCommand request)
		{
			return await _mediator.Send(request);
		}

		/// <summary>
		/// Запрос списка пользователей
		/// </summary>
		/// <param name="request">запрос</param>
		/// <returns></returns>
		[HttpGet]
		public async Task<GetUsersQueryResponse> Get([FromQuery] GetUsersQuery request)
		{
			return await _mediator.Send(request);
		}
	}
}
