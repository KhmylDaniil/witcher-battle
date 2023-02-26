using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.UserGameRequests;
using Sindie.ApiService.WebApi.Versioning;
using System.Threading.Tasks;

namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Контроллер пользователя игры
	/// </summary>
	[ApiVersion(ApiVersions.V1)]
	public class UserGameController : ApiControllerBase
	{
		/// <summary>
		/// Медиатор
		/// </summary>
		private readonly IMediator _mediator;

		/// <summary>
		/// Конструктор контроллера пользователя игры
		/// </summary>
		/// <param name="mediator"></param>
		public UserGameController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Создание пользователя игры
		/// </summary>
		/// <param name="request">Запрос создания пользователя игры</param>
		/// <returns>Юнит</returns>
		[Authorize]
		[HttpPost]
		public async Task<Unit> CreateUserGame([FromQuery] CreateUserGameCommand request)
		{
			return await _mediator.Send(request);
		}

		/// <summary>
		/// Удаление пользователя игры
		/// </summary>
		/// <param name="request">Запрос удаления пользователя игры</param>
		/// <returns>Юнит</returns>
		[Authorize]
		[HttpDelete]
		public async Task<Unit> DeleteUserGame([FromQuery] DeleteUserGameCommand request)
		{
			return await _mediator.Send(request);
		}
	}
}
