using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Contracts.BodyTemplateRequests;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions;
using Witcher.Core.ExtensionMethods;
using Witcher.MVC.Hubs;
using Witcher.MVC.ViewModels.RunBattle;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Witcher.MVC.Controllers
{
	[Authorize]
	public class RunBattleController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IMemoryCache _memoryCache;
		private readonly IHubContext<MessageHub> _messageHubContext;
		
		public RunBattleController(IMediator mediator, IGameIdService gameIdService, IMapper mapper,
			IHubContext<MessageHub> messageHub, IMemoryCache memoryCache)
			: base(mediator, gameIdService)
		{
			_mapper = mapper;
			_memoryCache = memoryCache;
			_messageHubContext = messageHub;
		}

		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> Run(RunBattleCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.Send(command, cancellationToken);
				return View(response);
			}
			catch (Exception ex) { return await HandleExceptionAsync<RunBattleController>(ex, async () =>
			{
				return View(await _mediator.Send(new GetBattleByIdQuery() { Id = command.BattleId }, cancellationToken));
			}); }
		}

		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> MakeTurn(MakeTurnCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.Send(command, cancellationToken);

				var vm = _mapper.Map<MakeTurnViewModel>(response);

				return View(vm);
			}
			catch (Exception ex) { return HandleException<RunBattleController>(ex, () => InnerRedirectToRunBattle(command.BattleId)); }
		}

		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> MakeAbilityAttack(AttackCommand command, CancellationToken cancellationToken)
		{
			var viewModel = await CreateVM(command, Core.BaseData.Enums.AttackType.Ability, cancellationToken);

			return View("MakeAttack", viewModel);
		}

		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> MakeWeaponAttack(AttackCommand command, CancellationToken cancellationToken)
		{
			var viewModel = await CreateVM(command, Core.BaseData.Enums.AttackType.Weapon, cancellationToken);

			return View("MakeAttack", viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> Attack(AttackCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(command, cancellationToken);

				await _messageHubContext.SendUpdateBattleMessage(await GetUserIdListAsync(command.BattleId, cancellationToken));

				return InnerRedirectToRunBattle(command.BattleId);
			}
			catch (Exception ex) { return HandleException<RunBattleController>(ex, () => InnerRedirectToRunBattle(command.BattleId)); }
		}

		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> MakeHeal(HealEffectCommand command, CancellationToken cancellationToken)
		{
			var viewModel = await CreateVM(command, cancellationToken);

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> Heal(HealEffectCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(command, cancellationToken);

				await _messageHubContext.SendUpdateBattleMessage(await GetUserIdListAsync(command.BattleId, cancellationToken));

				return InnerRedirectToRunBattle(command.BattleId);
			}
			catch (Exception ex) { return HandleException<RunBattleController>(ex, () => InnerRedirectToRunBattle(command.BattleId)); }
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]")]
		public async Task<IActionResult> Pass(EndTurnCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(command, cancellationToken);
				await _messageHubContext.SendUpdateBattleMessage(await GetUserIdListAsync(command.BattleId, cancellationToken));
				return InnerRedirectToRunBattle(command.BattleId);
			}
			catch (Exception ex) { return HandleException<RunBattleController>(ex, () => InnerRedirectToRunBattle(command.BattleId)); }
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> PassInMultiAttack(PassInMultiattackCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(command, cancellationToken);
				await _messageHubContext.SendUpdateBattleMessage(await GetUserIdListAsync(command.BattleId, cancellationToken));
				return InnerRedirectToRunBattle(command.BattleId);
			}
			catch (Exception ex) { return HandleException<RunBattleController>(ex, () => InnerRedirectToRunBattle(command.BattleId)); }
		}

		private async Task<MakeAttackViewModel> CreateVM(AttackCommand command, Core.BaseData.Enums.AttackType attackType, CancellationToken cancellationToken)
		{
			var vm = _mapper.Map<MakeAttackViewModel>(command);
			vm.AttackType = attackType;

			var formAttackCommand = new FormAttackCommand
			{
				AttackerId = command.Id,
				TargetId = command.TargetId,
				AttackFormulaId = command.AttackFormulaId,
				AttackType = attackType,
			};
			var formAttackResponse = await _mediator.Send(formAttackCommand, cancellationToken);

			vm.CreatureParts = formAttackResponse.CreatureParts;
			vm.DefensiveSkills = formAttackResponse.DefensiveSkills;

			return vm;
		}

		private async Task<MakeHealViewModel> CreateVM(HealEffectCommand command, CancellationToken cancellationToken)
		{
			var vm = _mapper.Map<MakeHealViewModel>(command);

			var formHealCommand = new FormHealCommand
			{
				TargetCreatureId = command.TargetId,
			};
			var formHealResponse = await _mediator.Send(formHealCommand, cancellationToken);

			vm.EffectsOnTarget = formHealResponse.EffectsOnTarget;

			return vm;
		}

		/// <summary>
		/// Service method getting battle participants userId list for SignalR update battle log command
		/// </summary>
		/// <returns></returns>
		private async Task<IReadOnlyList<string>> GetUserIdListAsync(Guid battleId, CancellationToken cancellationToken)
		{
			if (_memoryCache.TryGetValue(battleId, out IReadOnlyList<string> userIdListFromCache))
				return userIdListFromCache;
			else
			{
				var userIdList = await _mediator.Send(new GetUserIdListForBattleQuery
				{
					GameId = _gameIdService.GameId,
					BattleId = battleId
				}, cancellationToken);
				_memoryCache.Set(battleId, userIdList);
				return userIdList;
			}
		}

		private ActionResult InnerRedirectToRunBattle(Guid battleId) => RedirectToAction(nameof(Run), new RunBattleCommand() { BattleId = battleId });
	}
}
