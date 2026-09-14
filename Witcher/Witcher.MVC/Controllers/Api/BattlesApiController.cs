using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.MVC.Controllers.Api.Dto;

namespace Witcher.MVC.Controllers.Api
{
	/// <summary>
	/// Зеркалит Witcher.MVC/Controllers/BattleController.cs (без IMemoryCache-словарей — те были нужны
	/// только для Razor select-дропдаунов). Вложено под /api/games/{gameId}.
	/// </summary>
	[Route("api/games/{gameId:guid}/battles")]
	public class BattlesApiController : ApiControllerBase
	{
		public BattlesApiController(IMediator mediator, IGameIdService gameIdService)
			: base(mediator, gameIdService)
		{
		}

		[HttpGet]
		public async Task<IEnumerable<GetBattleResponseItem>> Index(
			Guid gameId, [FromQuery] GetBattleQuery query, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			return await _mediator.Send(query, cancellationToken);
		}

		[HttpGet("{id:guid}")]
		public async Task<BattleDetailsDto> Get(Guid gameId, Guid id, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			var response = await _mediator.Send(new GetBattleByIdQuery { Id = id }, cancellationToken);
			return BattleDetailsDto.From(response);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Guid gameId, CreateBattleCommand command, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			var result = await _mediator.Send(command, cancellationToken);
			return Ok(new { id = result.Id });
		}

		[HttpPut("{id:guid}")]
		public async Task<IActionResult> Edit(Guid gameId, Guid id, ChangeBattleCommand command, CancellationToken cancellationToken)
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
			await _mediator.Send(new DeleteBattleCommand { Id = id, Name = name }, cancellationToken);
			return NoContent();
		}

		[HttpGet("{battleId:guid}/creatures/{id:guid}")]
		public async Task<GetCreatureByIdResponse> GetCreature(Guid gameId, Guid battleId, Guid id, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			return await _mediator.Send(new GetCreatureByIdQuery { BattleId = battleId, Id = id }, cancellationToken);
		}

		[HttpPost("{battleId:guid}/creatures")]
		public async Task<IActionResult> CreateCreature(
			Guid gameId, Guid battleId, CreateCreatureCommand command, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			command.BattleId = battleId;
			await _mediator.Send(command, cancellationToken);
			return Ok();
		}

		[HttpPut("{battleId:guid}/creatures/{id:guid}")]
		public async Task<IActionResult> EditCreature(
			Guid gameId, Guid battleId, Guid id, ChangeCreatureCommand command, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			command.BattleId = battleId;
			command.Id = id;
			await _mediator.Send(command, cancellationToken);
			return NoContent();
		}

		[HttpDelete("{battleId:guid}/creatures/{id:guid}")]
		public async Task<IActionResult> DeleteCreature(
			Guid gameId, Guid battleId, Guid id, [FromQuery] string name, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			await _mediator.Send(new DeleteCreatureCommand { BattleId = battleId, Id = id, Name = name }, cancellationToken);
			return NoContent();
		}

		/// <summary>Добавление персонажа игрока в бой</summary>
		[HttpPost("{battleId:guid}/characters")]
		public async Task<IActionResult> AddCharacter(
			Guid gameId, Guid battleId, AddCharacterToBattleCommand command, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			command.BattleId = battleId;
			await _mediator.Send(command, cancellationToken);
			return Ok();
		}
	}
}
