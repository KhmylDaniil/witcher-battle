using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.InstanceRequests.CreateInstance;
using Sindie.ApiService.Core.Requests.InstanceRequests.CreateInstance;
using Sindie.ApiService.WebApi.Versioning;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Контроллер инстанса
	/// </summary>
	[ApiVersion(ApiVersions.V1)]
	[Authorize]
	public class InstanceController: ApiControllerBase
	{
		/// <summary>
		/// Медиатор
		/// </summary>
		private readonly IMediator _mediator;

		/// <summary>
		/// Конструктор контроллера инстанса
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		public InstanceController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Создать инстанс
		/// </summary>
		/// <param name="request">Запрос на создание инстанса</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPost]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task CreateInstanceAsync([FromBody] CreateInstanceRequest request, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new CreateInstanceCommand(
					gameId: request.GameId,
					imgFileId: request.ImgFileId,
					name: request.Name,
					description: request.Description,
					creatures: request.Creatures), cancellationToken);
		}


	}
}
