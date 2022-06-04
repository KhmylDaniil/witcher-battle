using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.ParameterRequests;
using Sindie.ApiService.Core.Requests.ParameterRequests.ChangeParameter;
using Sindie.ApiService.Core.Requests.ParameterRequests.CreateParameter;
using Sindie.ApiService.WebApi.Versioning;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Контроллер параметров
	/// </summary>
	[ApiVersion(ApiVersions.V1)]
	[Authorize]
	public class ParameterController: ApiControllerBase
	{
		private readonly IMediator _mediator;

		/// <summary>
		/// Конструктор контроллера параметров
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		public ParameterController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Создать параметр
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPost]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task CreateParameterAsync([FromQuery] CreateParameterRequest request, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new CreateParameterCommand(
					gameId: request.GameId,
					name: request.Name,
					description: request.Description,
					minValueParameter: request.MinValueParameter,
					maxValueParameter: request.MaxValueParameter), cancellationToken);
		}

		/// <summary>
		/// Изменить параметр
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPut]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task ChangeParameterAsync([FromQuery] ChangeParameterRequest request, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new ChangeParameterCommand(
					gameId: request.GameId,
					id: request.Id,
					name: request.Name,
					description: request.Description,
					minValueParameter: request.MinValueParameter,
					maxValueParameter: request.MaxValueParameter), cancellationToken);
		}
	}
}