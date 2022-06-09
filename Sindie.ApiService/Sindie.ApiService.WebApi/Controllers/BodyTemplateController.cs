using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.ChangeBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.CreateBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.DeleteBodyTemplateById;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplateById;
using Sindie.ApiService.Core.Requests.BodyTemplateRequests.ChangeBodyTemplate;
using Sindie.ApiService.Core.Requests.BodyTemplateRequests.CreateBodyTemplate;
using Sindie.ApiService.Core.Requests.BodyTemplateRequests.GetBodyTemplate;
using Sindie.ApiService.WebApi.Versioning;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Контроллер шаблона тела
	/// </summary>
	[ApiVersion(ApiVersions.V1)]
	[Authorize]
	public class BodyTemplateController: ApiControllerBase
	{
		/// <summary>
		/// Медиатор
		/// </summary>
		private readonly IMediator _mediator;

		/// <summary>
		/// Конструктор контроллера шаблона тела
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		public BodyTemplateController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Обработчик создания шаблона тела
		/// </summary>
		/// <param name="request">Запрос на создание шаблона тела</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPost]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task CreateBodyTemplateAsync([FromBody] CreateBodyTemplateRequest request, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new CreateBodyTemplateCommand(
					gameId: request.GameId,
					name: request.Name,
					description: request.Description,
					bodyTemplateParts: request.BodyTemplateParts), cancellationToken);
		}

		/// <summary>
		/// Обработчик изменения шаблона тела
		/// </summary>
		/// <param name="request">Запрос на изменение шаблона тела</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPut]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task ChangeBodyTemplateAsync([FromBody] ChangeBodyTemplateRequest request, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new ChangeBodyTemplateCommand(
					id: request.Id,
					gameId: request.GameId,
					name: request.Name,
					description: request.Description,
					bodyTemplateParts: request.BodyTemplateParts), cancellationToken);
		}

		/// <summary>
		/// Обработчик предоставления списка шаблонов тела
		/// </summary>
		/// <param name="request">Запрос на предоставление списка шаблонов тела</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Ответ на запрос предоставления списка шаблонов тела</returns>
		[HttpGet("GetBodyTemplate")]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<GetBodyTemplateResponse> GetBodyTemplateAsync([FromQuery] GetBodyTemplateQuery request, CancellationToken cancellationToken)
		{
			return await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new GetBodyTemplateCommand(
					gameId: request.GameId,
					name: request.Name,
					userName: request.UserName,
					creationMaxTime: request.CreationMaxTime,
					creationMinTime: request.CreationMinTime,
					modificationMaxTime: request.ModificationMaxTime,
					modificationMinTime: request.ModificationMinTime,
					bodyPartName: request.BodyPartName,
					pageNumber: request.PageNumber,
					pageSize: request.PageSize,
					orderBy: request.OrderBy,
					isAscending: request.IsAscending), cancellationToken);
		}

		/// <summary>
		/// Обработчик предоставления шаблона тела по айди
		/// </summary>
		/// <param name="request">Запрос на предоставление шаблона тела по айди</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Ответ на запрос предоставления шаблона тела по айди</returns>
		[HttpGet("GetBodyTemplateById")]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<GetBodyTemplateByIdResponse> GetBodyTemplateByIdAsync([FromQuery] GetBodyTemplateByIdQuery request, CancellationToken cancellationToken)
		{
			return await _mediator.Send(request == null
				? throw new ArgumentNullException(nameof(request)) : request, cancellationToken);
		}

		/// <summary>
		/// Обработчик удаления шаблона тела по айди
		/// </summary>
		/// <param name="request">Запрос на удаление шаблона тела по айди</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpDelete]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task DeleteBodyTemplateByIdAsync([FromQuery] DeleteBodyTemplateByIdCommand request, CancellationToken cancellationToken)
		{
			await _mediator.Send(request == null
				? throw new ArgumentNullException(nameof(request)) : request, cancellationToken);
		}
	}
}
