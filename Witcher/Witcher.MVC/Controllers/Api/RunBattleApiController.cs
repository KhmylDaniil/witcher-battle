using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.MVC.Controllers.Api.Dto;
using Witcher.MVC.Hubs;

namespace Witcher.MVC.Controllers.Api
{
	/// <summary>
	/// Зеркалит Witcher.MVC/Controllers/RunBattleController.cs — пошаговое проведение боя.
	/// После мутирующих действий (Attack/Heal/EndTurn/PassInMultiAttack) уведомляет участников боя через
	/// тот же SignalR хаб, что и Razor-версия; клиент (React) на "UpdateBattleLog" делает точечный
	/// refetch вместо location.reload().
	/// </summary>
	[Route("api/games/{gameId:guid}/battles/{battleId:guid}")]
	public class RunBattleApiController : ApiControllerBase
	{
		private readonly IHubContext<MessageHub> _messageHubContext;
		private readonly IMemoryCache _memoryCache;

		public RunBattleApiController(
			IMediator mediator, IGameIdService gameIdService, IHubContext<MessageHub> messageHub, IMemoryCache memoryCache)
			: base(mediator, gameIdService)
		{
			_messageHubContext = messageHub;
			_memoryCache = memoryCache;
		}

		/// <summary>Снимок состояния боя + начало хода следующего по инициативе существа</summary>
		[HttpGet("run")]
		public async Task<RunBattleDto> Run(Guid gameId, Guid battleId, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			var response = await _mediator.Send(new RunBattleCommand { BattleId = battleId }, cancellationToken);
			return RunBattleDto.From(response);
		}

		/// <summary>Доступные действия хода для конкретного существа (цели/способности/оружие/состояние хода)</summary>
		[HttpGet("turn")]
		public async Task<MakeTurnResponse> MakeTurn(
			Guid gameId, Guid battleId, [FromQuery] Guid creatureId, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			return await _mediator.Send(new MakeTurnCommand { BattleId = battleId, CreatureId = creatureId }, cancellationToken);
		}

		/// <summary>Данные для формы атаки: доступные части тела цели и защитные навыки</summary>
		[HttpGet("form-attack")]
		public async Task<FormAttackResponse> FormAttack(Guid gameId, Guid battleId, [FromQuery] FormAttackCommand command, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			return await _mediator.Send(command, cancellationToken);
		}

		[HttpPost("attack")]
		public async Task<IActionResult> Attack(Guid gameId, Guid battleId, AttackCommand command, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			command.BattleId = battleId;
			await _mediator.Send(command, cancellationToken);
			await NotifyBattleUpdated(gameId, battleId, cancellationToken);
			return Ok();
		}

		/// <summary>Данные для формы лечения/снятия эффекта: текущие эффекты на цели</summary>
		[HttpGet("form-heal")]
		public async Task<FormHealResponse> FormHeal(Guid gameId, Guid battleId, [FromQuery] Guid targetCreatureId, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			return await _mediator.Send(new FormHealCommand { TargetCreatureId = targetCreatureId }, cancellationToken);
		}

		[HttpPost("heal")]
		public async Task<IActionResult> Heal(Guid gameId, Guid battleId, HealEffectCommand command, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			command.BattleId = battleId;
			await _mediator.Send(command, cancellationToken);
			await NotifyBattleUpdated(gameId, battleId, cancellationToken);
			return Ok();
		}

		/// <summary>Пропуск оставшейся части мультиатаки (доступно только ГМ)</summary>
		[HttpPost("pass-in-multiattack")]
		public async Task<IActionResult> PassInMultiAttack(
			Guid gameId, Guid battleId, [FromBody] Guid creatureId, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			await _mediator.Send(new PassInMultiattackCommand { BattleId = battleId, CreatureId = creatureId }, cancellationToken);
			await NotifyBattleUpdated(gameId, battleId, cancellationToken);
			return Ok();
		}

		/// <summary>Завершение хода — продвигает инициативу к следующему существу (кнопка Pass)</summary>
		[HttpPost("end-turn")]
		public async Task<IActionResult> EndTurn(Guid gameId, Guid battleId, [FromBody] Guid creatureId, CancellationToken cancellationToken)
		{
			_gameIdService.Set(gameId);
			await _mediator.Send(new EndTurnCommand { BattleId = battleId, CreatureId = creatureId }, cancellationToken);
			await NotifyBattleUpdated(gameId, battleId, cancellationToken);
			return Ok();
		}

		private async Task NotifyBattleUpdated(Guid gameId, Guid battleId, CancellationToken cancellationToken)
			=> await _messageHubContext.SendUpdateBattleMessage(await GetUserIdListAsync(gameId, battleId, cancellationToken));

		/// <summary>Список userId участников боя для таргетированной SignalR рассылки — кэшируется как и в Razor-версии</summary>
		private async Task<IReadOnlyList<string>> GetUserIdListAsync(Guid gameId, Guid battleId, CancellationToken cancellationToken)
		{
			if (_memoryCache.TryGetValue(battleId, out IReadOnlyList<string> userIdListFromCache))
				return userIdListFromCache;

			var userIdList = await _mediator.Send(new GetUserIdListForBattleQuery { GameId = gameId, BattleId = battleId }, cancellationToken);
			_memoryCache.Set(battleId, userIdList);
			return userIdList;
		}
	}
}
