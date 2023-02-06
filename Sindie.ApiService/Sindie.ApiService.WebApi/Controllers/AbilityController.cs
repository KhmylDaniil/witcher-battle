using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.AbilityRequests.ChangeAbility;
using Sindie.ApiService.Core.Contracts.AbilityRequests.CreateAbility;
using Sindie.ApiService.Core.Contracts.AbilityRequests.DeleteAbilitybyId;
using Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbility;
using Sindie.ApiService.Core.Requests.AbilityRequests.ChangeAbility;
using Sindie.ApiService.Core.Requests.AbilityRequests.CreateAbility;
using Sindie.ApiService.Core.Requests.AbilityRequests.GetAbility;
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
		public async Task CreateAbilityAsync([FromBody] CreateAbilityRequest request, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new CreateAbilityCommand(
					gameId: request.GameId,
					name: request.Name,
					description: request.Description,
					attackSkill: request.AttackSkill,
					attackDiceQuantity: request.AttackDiceQuantity,
					damageModifier: request.DamageModifier,
					attackSpeed: request.AttackSpeed,
					accuracy: request.Accuracy,
					defensiveSkills: request.DefensiveSkills,
					damageType: request.DamageType,
					appliedConditions: request.AppliedConditions), cancellationToken);
		}

		/// <summary>
		/// Изменить способность
		/// </summary>
		/// <param name="request">Запрос на изменение способности</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPut]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task ChangeAbilityAsync([FromBody] ChangeAbilityRequest request, CancellationToken cancellationToken)
		{
			await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new ChangeAbilityCommand(
					id: request.Id,
					gameId: request.GameId,
					name: request.Name,
					description: request.Description,
					attackSkill: request.AttackSkill,
					attackDiceQuantity: request.AttackDiceQuantity,
					damageModifier: request.DamageModifier,
					attackSpeed: request.AttackSpeed,
					accuracy: request.Accuracy,
					defensiveSkills: request.DefensiveSkills,
					damageType: request.DamageType,
					appliedConditions: request.AppliedConditions), cancellationToken);
		}

		/// <summary>
		/// Предоставить список способностей
		/// </summary>
		/// <param name="request">Запрос на предоставление списка способности</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpGet("GetAbilities")]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<GetAbilityResponse> GetAbilityAsync([FromQuery] GetAbilityQuery request, CancellationToken cancellationToken)
		{
			return await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new GetAbilityCommand(
					gameId: request.GameId,
					name: request.Name,
					attackSkillName: request.AttackSkillName,
					damageType: request?.DamageType,
					conditionName: request.ConditionName,
					minAttackDiceQuantity: request.MinAttackDiceQuantity,
					maxAttackDiceQuantity: request.MaxAttackDiceQuantity,
					userName: request.UserName,				
					creationMaxTime: request.CreationMaxTime,
					creationMinTime: request.CreationMinTime,
					modificationMaxTime: request.ModificationMaxTime,
					modificationMinTime: request.ModificationMinTime,
					pageNumber: request.PageNumber,
					pageSize: request.PageSize,
					orderBy: request.OrderBy,
					isAscending: request.IsAscending), cancellationToken);
		}

		/// <summary>
		/// Удалить способность
		/// </summary>
		/// <param name="request">Запрос на удаление способности</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpDelete]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task DeleteAbilityByIdAsync([FromQuery] DeleteAbilityByIdCommand request, CancellationToken cancellationToken)
		{
			await _mediator.Send(request == null
				? throw new ArgumentNullException(nameof(request)) : request, cancellationToken);
		}
	}
}
