using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CreatureTemplateRequests;

namespace Witcher.MVC.Controllers.Api
{
	/// <summary>
	/// Зеркалит Witcher.MVC/Controllers/CreatureTemplateController.cs — эталонный полный CRUD с вложенными
	/// подсущностями (части тела/навыки/модификаторы урона), каждая правится одним апдейтом за раз,
	/// как и в Razor-версии (EditParts/EditSkill/EditDamageTypeModifier).
	/// Вложено под /api/games/{gameId} — вся авторизация в Core завязана на ambient _gameIdService.GameId.
	/// </summary>
	[Route("api/games/{gameId:guid}/creature-templates")]
	public class CreatureTemplatesApiController : ApiControllerBase
	{
		public CreatureTemplatesApiController(IMediator mediator, IGameIdService gameIdService)
			: base(mediator, gameIdService)
		{
		}

		[HttpGet]
		public async Task<IEnumerable<GetCreatureTemplateResponseItem>> Index(
			Guid gameId, [FromQuery] GetCreatureTemplateQuery query, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			return await _mediator.Send(query, cancellationToken);
		}

		[HttpGet("{id:guid}")]
		public async Task<GetCreatureTemplateByIdResponse> Get(Guid gameId, Guid id, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			return await _mediator.Send(new GetCreatureTemplateByIdQuery { Id = id }, cancellationToken);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Guid gameId, CreateCreatureTemplateCommand command, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			var result = await _mediator.Send(command, cancellationToken);
			return Ok(new { id = result.Id });
		}

		[HttpPut("{id:guid}")]
		public async Task<IActionResult> Edit(Guid gameId, Guid id, ChangeCreatureTemplateCommand command, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			command.Id = id;
			await _mediator.Send(command, cancellationToken);
			return NoContent();
		}

		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> Delete(Guid gameId, Guid id, [FromQuery] string name, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			await _mediator.Send(new DeleteCreatureTemplateByIdCommand { Id = id, Name = name }, cancellationToken);
			return NoContent();
		}

		/// <summary>Изменение брони одной части тела, либо всех сразу если command.Id не задан</summary>
		[HttpPut("{creatureTemplateId:guid}/parts")]
		public async Task<IActionResult> EditParts(
			Guid gameId, Guid creatureTemplateId, ChangeCreatureTemplatePartCommand command, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			command.CreatureTemplateId = creatureTemplateId;
			await _mediator.Send(command, cancellationToken);
			return NoContent();
		}

		/// <summary>Создание/изменение навыка (upsert — command.Id == null создаёт новый)</summary>
		[HttpPut("{creatureTemplateId:guid}/skills")]
		public async Task<IActionResult> EditSkill(
			Guid gameId, Guid creatureTemplateId, UpdateCreatureTemplateSkillCommand command, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			command.CreatureTemplateId = creatureTemplateId;
			await _mediator.Send(command, cancellationToken);
			return NoContent();
		}

		[HttpDelete("{creatureTemplateId:guid}/skills/{skillId:guid}")]
		public async Task<IActionResult> DeleteSkill(Guid gameId, Guid creatureTemplateId, Guid skillId, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			await _mediator.Send(
				new DeleteCreatureTemplateSkillCommand { CreatureTemplateId = creatureTemplateId, Id = skillId }, cancellationToken);
			return NoContent();
		}

		/// <summary>Изменение/сброс модификатора урона по типу</summary>
		[HttpPut("{creatureTemplateId:guid}/damage-modifiers")]
		public async Task<IActionResult> EditDamageTypeModifier(
			Guid gameId, Guid creatureTemplateId, ChangeDamageTypeModifierForCreatureTemplateCommand command, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			command.CreatureTemplateId = creatureTemplateId;
			await _mediator.Send(command, cancellationToken);
			return NoContent();
		}
	}
}
