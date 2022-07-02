﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.BattleRequests.HeroAttack;
using Sindie.ApiService.Core.Contracts.BattleRequests.MonsterAttack;
using Sindie.ApiService.Core.Contracts.BattleRequests.MonsterSuffer;
using Sindie.ApiService.Core.Requests.BattleRequests.HeroAttack;
using Sindie.ApiService.Core.Requests.BattleRequests.MonsterAttack;
using Sindie.ApiService.Core.Requests.BattleRequests.MonsterSuffer;
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
		/// <param name="request">Запрос на атаку монстра</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPut]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<MonsterAttackResponse> MonsterAttackAsync([FromQuery] MonsterAttackRequest request, CancellationToken cancellationToken)
		{
			return await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new MonsterAttackCommand(
					instanceId: request.InstanceId,
					id: request.Id,
					abilityId: request.AbilityId,
					targetCreatureId: request.TargetCreatureId,
					creaturePartId: request.CreaturePartId,
					defenseValue: request.DefenseValue,
					specialToHit: request.SpecialToHit,
					specialToDamage: request.SpecialToDamage), cancellationToken);
		}

		/// <summary>
		/// Получение монстром урона
		/// </summary>
		/// <param name="request">Запрос на получение монстром урона</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPut]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<MonsterSufferResponse> MonsterSufferAsync([FromQuery] MonsterSufferRequest request, CancellationToken cancellationToken)
		{
			return await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new MonsterSufferCommand(
					instanceId: request.InstanceId,
					monsterId: request.MonsterId,
					damageValue: request.DamageValue,
					successValue: request.SuccessValue,
					creaturePartId: request.CreaturePartId,
					isResistant: request.IsResistant,
					isVulnerable: request.IsVulnerable), cancellationToken);
		}

		/// <summary>
		/// Атака героя
		/// </summary>
		/// <param name="request">Запрос на атаку героя</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		[HttpPut]
		[SwaggerResponse(StatusCodes.Status200OK)]
		[SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
		public async Task<CreatureAttackResponse> HeroAttackAsync([FromQuery] CreatureAttackRequest request, CancellationToken cancellationToken)
		{
			return await _mediator.Send(
				request == null
				? throw new ArgumentNullException(nameof(request))
				: new HeroAttackCommand(
					instanceId: request.InstanceId,
					id: request.Id,
					abilityId: request.AbilityId,
					targetCreatureId: request.TargetCreatureId,
					bodyTemplatePartId: request.BodyTemplatePartId,
					attackValue: request.AttackValue,
					damageValue: request.DamageValue,
					specialToHit: request.SpecialToHit,
					specialToDamage: request.SpecialToDamage), cancellationToken);
		}
	}
}
