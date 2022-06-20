using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.ChangeCreatureTemplate;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.CreateCreatureTemplate;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.DeleteCreatureTemplateById;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplate;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplateById;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.ChangeCreatureTemplate;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.CreateCreatureTemplate;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.GetCreatureTemplate;
using Sindie.ApiService.WebApi.Versioning;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Контроллер шаблона существа
	/// </summary>
	[ApiVersion(ApiVersions.V1)]
	[Authorize]
	public class CreatureTemplateController : ApiControllerBase
	{
		/// <summary>
		/// Медиатор
		/// </summary>
		private readonly IMediator _mediator;

		/// <summary>
		/// Конструктор контроллера шаблона тела
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		public CreatureTemplateController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Cоздать шаблон существа
		/// </summary>
		/// <param name="request">Запрос на создание шаблона существа</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPost]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task CreateCreatureTemplateAsync([FromBody] CreateCreatureTemplateRequest request, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new CreateCreatureTemplateCommand(
					gameId: request.GameId,
					imgFileId: request.ImgFileId,
					bodyTemplateId: request.BodyTemplateId,
					name: request.Name,
					description: request.Description,
					type: request.Type,
					hp: request.HP,
					sta: request.Sta,
					@int: request.Int,
					@ref: request.Ref,
					dex: request.Dex,
					body: request.Body,
					emp: request.Emp,
					cra: request.Cra,
					will: request.Will,
					speed: request.Speed,
					luck: request.Luck,
					armorList: request.ArmorList,
					abilities: request.Abilities,
					creatureTemplateParameters: request.CreatureTemplateParameters), cancellationToken);
		}

		/// <summary>
		/// Изменить шаблон существа
		/// </summary>
		/// <param name="request">Запрос на изменение шаблона существа</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPut]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task ChangeCreatureTemplateAsync([FromBody] ChangeCreatureTemplateRequest request, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new ChangeCreatureTemplateCommand(
					id: request.Id,
					gameId: request.GameId,
					imgFileId: request.ImgFileId,
					bodyTemplateId: request.BodyTemplateId,
					name: request.Name,
					description: request.Description,
					type: request.Type,
					hp: request.HP,
					sta: request.Sta,
					@int: request.Int,
					@ref: request.Ref,
					dex: request.Dex,
					body: request.Body,
					emp: request.Emp,
					cra: request.Cra,
					will: request.Will,
					speed: request.Speed,
					luck: request.Luck,
					armorList: request.ArmorList,
					abilities: request.Abilities,
					creatureTemplateParameters: request.CreatureTemplateParameters), cancellationToken);
		}

		/// <summary>
		/// Предоставить список шаблонов существа
		/// </summary>
		/// <param name="request">Запрос на предоставление списка шаблонов существа</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Ответ на запрос предоставления списка шаблонов существа</returns>
		[HttpGet("GetCreatureTemplate")]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<GetCreatureTemplateResponse> GetCreatureTemplateAsync([FromQuery] GetCreatureTemplateQuery request, CancellationToken cancellationToken)
		{
			return await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new GetCreatureTemplateCommand(
					gameId: request.GameId,
					name: request.Name,
					type: request.Type,
					userName: request.UserName,
					creationMaxTime: request.CreationMaxTime,
					creationMinTime: request.CreationMinTime,
					modificationMaxTime: request.ModificationMaxTime,
					modificationMinTime: request.ModificationMinTime,
					bodyTemplateName: request.BodyTemplateName,
					bodyPartTypeId: request.BodyPartTypeId,
					conditionName: request.ConditionName,
					pageNumber: request.PageNumber,
					pageSize: request.PageSize,
					orderBy: request.OrderBy,
					isAscending: request.IsAscending), cancellationToken);
		}

		/// <summary>
		/// Предоставить шаблон существа по айди
		/// </summary>
		/// <param name="request">Запрос на предоставление шаблона существа</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Ответ на запрос предоставления шаблона существа</returns>
		[HttpGet("GetCreatureTemplateById")]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<GetCreatureTemplateByIdResponse> GetCreatureTemplateByIdAsync([FromQuery] GetCreatureTemplateByIdQuery request, CancellationToken cancellationToken)
		{
			return await _mediator.Send(request == null
				? throw new ArgumentNullException(nameof(request)) : request, cancellationToken);
		}

		/// <summary>
		/// Удалить шаблон тела по айди
		/// </summary>
		/// <param name="request">Запрос на удаление шаблона тела по айди</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpDelete]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task DeleteCreatureTemplateByIdAsync([FromQuery] DeleteCreatureTemplateByIdCommand request, CancellationToken cancellationToken)
		{
			await _mediator.Send(request == null
				? throw new ArgumentNullException(nameof(request)) : request, cancellationToken);
		}
	}
}
