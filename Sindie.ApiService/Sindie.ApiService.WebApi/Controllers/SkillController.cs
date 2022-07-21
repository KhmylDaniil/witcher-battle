using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.SkillRequests;
using Sindie.ApiService.Core.Requests.SkillRequests.ChangeSkill;
using Sindie.ApiService.Core.Requests.SkillRequests.CreateSkill;
using Sindie.ApiService.WebApi.Versioning;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Контроллер навыков
	/// </summary>
	[ApiVersion(ApiVersions.V1)]
	[Authorize]
	public class SkillController: ApiControllerBase
	{
		private readonly IMediator _mediator;

		/// <summary>
		/// Конструктор контроллера навыков
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		public SkillController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Создать навык
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPost]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task CreateSkillAsync([FromQuery] CreateSkillRequest request, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new CreateSkillCommand(
					gameId: request.GameId,
					name: request.Name,
					description: request.Description,
					statName: request.StatName,
					minValueSkill: request.MinValueSkill,
					maxValueSkill: request.MaxValueSkill), cancellationToken);
		}

		/// <summary>
		/// Изменить навык
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPut]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task ChangeSkillAsync([FromQuery] ChangeSkillRequest request, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new ChangeSkillCommand(
					gameId: request.GameId,
					id: request.Id,
					name: request.Name,
					description: request.Description,
					statName: request.StatName,
					minValueSkill: request.MinValueSkill,
					maxValueSkill: request.MaxValueSkill), cancellationToken);
		}
	}
}