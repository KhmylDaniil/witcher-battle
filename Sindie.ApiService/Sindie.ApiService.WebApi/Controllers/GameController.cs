using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.GameRequests.ChangeGame;
using Sindie.ApiService.Core.Contracts.GameRequests.CreateGame;
using Sindie.ApiService.WebApi.Versioning;
using System.Threading.Tasks;

namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Контроллер игры
	/// </summary>
	[ApiVersion(ApiVersions.V1)]
	public class GameController : ApiControllerBase
	{
		/// <summary>
		/// Медиатор
		/// </summary>
		private readonly IMediator _mediator;

		/// <summary>
		/// Конструктор контроллера игры
		/// </summary>
		/// <param name="mediator"></param>
		public GameController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Создание игры
		/// </summary>
		/// <param name="request">запрос создания игры</param>
		/// <returns>Юнит</returns>
		[Authorize]
		[HttpPost("CreateGame")]
		public async Task<Unit> CreateGame([FromQuery] CreateGameCommand request)
		{
			return await _mediator.Send(request);
		}

		/// <summary>
		/// Изменение игры
		/// </summary>
		/// <param name="request">запрос изменения игры</param>
		/// <returns>Юнит</returns>
		[Authorize]
		[HttpPut("ChangeGame")]
		public async Task<Unit> ChangeGame([FromQuery] ChangeGameCommand request)
		{
			return await _mediator.Send(request);
		}
	}
}
