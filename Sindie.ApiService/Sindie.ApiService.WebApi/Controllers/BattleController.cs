using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.BattleRequests.MonsterAttack;
using Sindie.ApiService.Core.Requests.BattleRequests.MonsterAttack;
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
		/// Конструктор контроллера шаблона тела
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		public BattleController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Атака монстра
		/// </summary>
		/// <param name="request">Запрос на создание шаблона тела</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPost]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<MonsterAttackResponse> MonsterAttackAsync([FromBody] MonsterAttackRequest request, CancellationToken cancellationToken)
		{
			return await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new MonsterAttackCommand(
					instanceId: request.InstanceId,
					id: request.Id,
					abilityId: request.AbilityId,
					targetCreatureId: request.TargetCreatureId,
					bodyTemplatePartId: request.BodyTemplatePartId,
					defenseValue: request.DefenseValue,
					specialToHit: request.SpecialToHit,
					specialToDamage: request.SpecialToDamage), cancellationToken);
		}
	}
}
