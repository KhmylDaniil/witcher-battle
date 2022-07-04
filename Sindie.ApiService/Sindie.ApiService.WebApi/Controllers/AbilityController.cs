using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.AbilityRequests.CreateAbility;
using Sindie.ApiService.Core.Requests.AbilityRequests.CreateAbility;
using Sindie.ApiService.WebApi.Versioning;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.WebApi.Controllers
{
	/// <summary>
	/// Контроллер способности
	/// </summary>
	[ApiVersion(ApiVersions.V1)]
	[Authorize]
	public class AbilityController: ApiControllerBase
	{
		/// <summary>
		/// Медиатор
		/// </summary>
		private readonly IMediator _mediator;

		/// <summary>
		/// Конструктор контроллера способности
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		public AbilityController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Создать способность
		/// </summary>
		/// <param name="request">Запрос на создание способности</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPost]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task MonsterAttackAsync([FromBody] CreateAbilityRequest request, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new CreateAbilityCommand(
					gameId: request.GameId,
					name: request.Name,
					description: request.Description,
					attackParameterId: request.AttackParameterId,
					attackDiceQuantity: request.AttackDiceQuantity,
					damageModifier: request.DamageModifier,
					attackSpeed: request.AttackSpeed,
					accuracy: request.Accuracy,
					defensiveParameters: request.DefensiveParameters,
					damageTypes: request.DamageTypes,
					appliedConditions: request.AppliedConditions), cancellationToken);
		}
	}
}
