using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Exceptions;
using Witcher.Core.ExtensionMethods;
using Witcher.MVC.Hubs;
using Witcher.MVC.ViewModels.RunBattle;

namespace Witcher.MVC.Controllers
{
	[Authorize]
	public class RunBattleController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IHubContext<MessageHub> _messageHubContext;
		
		public RunBattleController(IMediator mediator, IGameIdService gameIdService, IMapper mapper,
			IHubContext<MessageHub> messageHub)
			: base(mediator, gameIdService)
		{
			_mapper = mapper;
			_messageHubContext = messageHub;
		}

		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> Run(RunBattleCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.SendValidated(command, cancellationToken);
				return View(response);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;

				var response = await _mediator.SendValidated(new GetBattleByIdQuery() { Id = command.BattleId}, cancellationToken);
				return View(response);
			}
			catch (Exception ex) { return RedirectToErrorPage<RunBattleController>(ex); }
		}

		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> MakeTurn(MakeTurnCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.SendValidated(command, cancellationToken);

				var vm = _mapper.Map<MakeTurnViewModel>(response);

				return View(vm);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;

				return RedirectToAction(nameof(Run), new RunBattleCommand() { BattleId = command.BattleId });
			}
			catch (Exception ex) { return RedirectToErrorPage<RunBattleController>(ex); }
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
				await _mediator.SendValidated(command, cancellationToken);

				await _messageHubContext.SendUpdateBattleMessage();
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
			}
			catch (Exception ex) { return RedirectToErrorPage<RunBattleController>(ex); }

			return RedirectToAction(nameof(Run), new RunBattleCommand() { BattleId = command.BattleId });
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
				await _mediator.SendValidated(command, cancellationToken);

				await _messageHubContext.SendUpdateBattleMessage();
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
			}
			catch (Exception ex) { return RedirectToErrorPage<RunBattleController>(ex); }

			return RedirectToAction(nameof(Run), new RunBattleCommand() { BattleId = command.BattleId });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]")]
		public async Task<IActionResult> Pass(EndTurnCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				await _messageHubContext.SendUpdateBattleMessage();
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
			}
			catch (Exception ex) { return RedirectToErrorPage<RunBattleController>(ex); }

			return RedirectToAction(nameof(Run), new RunBattleCommand() { BattleId = command.BattleId });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> PassInMultiAttack(PassInMultiattackCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				await _messageHubContext.SendUpdateBattleMessage();
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
			}
			catch (Exception ex) { return RedirectToErrorPage<RunBattleController>(ex); }

			return RedirectToAction(nameof(Run), new RunBattleCommand() { BattleId = command.BattleId });
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
			var formAttackResponse = await _mediator.SendValidated(formAttackCommand, cancellationToken);

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
			var formHealResponse = await _mediator.SendValidated(formHealCommand, cancellationToken);

			vm.EffectsOnTarget = formHealResponse.EffectsOnTarget;

			return vm;
		}
	}
}
