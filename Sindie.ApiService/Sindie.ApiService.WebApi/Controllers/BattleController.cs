using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.RunBattleRequests;
using Sindie.ApiService.Core.ExtensionMethods;
using Sindie.ApiService.WebApi.Versioning;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Контроллер боя
	/// </summary>
	[ApiVersion(ApiVersions.V1)]
	[Authorize]
	public class BattleController: ApiControllerBase
	{
		/// <summary>
		/// Медиатор
		/// </summary>
		private readonly IMediator _mediator;

		/// <summary>
		/// Конструктор контроллера боя
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		public BattleController(IMediator mediator)
		{
			_mediator = mediator;
		}

		///// <summary>
		///// Создать бой
		///// </summary>
		///// <param name="request">Запрос на создание боя</param>
		///// <param name="cancellationToken">Токен отмены</param>
		///// <returns></returns>
		//[HttpPost]
		//[SwaggerResponse(StatusCodes.Status200OK)]
		//[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		//public async Task CreateBattleAsync([FromBody] Core.Contracts.BattleRequests.CreateBattleCommand request, CancellationToken cancellationToken)
		//{
		//	await _mediator.Send(
		//		request == null
		//		? throw new ArgumentNullException(nameof(request))
		//		: new Core.Requests.BattleRequests.CreateBattle.CreateBattleCommand(
		//			gameId: request.GameId,
		//			imgFileId: request.ImgFileId,
		//			name: request.Name,
		//			description: request.Description,
		//			creatures: request.Creatures), cancellationToken);
		//}



		/// <summary>
		/// Атака
		/// </summary>
		/// <param name="command">Запрос на атаку</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPut("Attack")]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<TurnResult> AttackAsync([FromQuery] AttackCommand command, CancellationToken cancellationToken)
		{
			return await _mediator.SendValidated(command, cancellationToken);
		}

		/// <summary>
		/// Попытка снять эффект
		/// </summary>
		/// <param name="request">Запрос на обработку попытки снять эффект</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPut("TreatEffectById")]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<TurnResult> TreatEffectByIdAsync([FromQuery] HealEffectCommand request, CancellationToken cancellationToken)
		{
			return await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: request, cancellationToken);
		}
	}
}

