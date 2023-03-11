using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests;
using Sindie.ApiService.Core.Contracts.RunBattleRequests;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.ExtensionMethods;
using Witcher.MVC.ViewModels.RunBattle;

namespace Witcher.MVC.Controllers
{
	[Authorize]
	public class RunBattleController : BaseController
	{
		private readonly IMapper _mapper;
		
		public RunBattleController(IMediator mediator, IGameIdService gameIdService, IMapper mapper) : base(mediator, gameIdService)
		{
			_mapper = mapper;
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

				return RedirectToAction(nameof(Run), new GetBattleByIdQuery() { Id = command.BattleId });
			}
		}

		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> MakeAttack(AttackCommand command, CancellationToken cancellationToken)
		{
			var viewModel = await CreateVM(command, cancellationToken);
			
			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> Attack(AttackCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var attackResponse = await _mediator.SendValidated(command, cancellationToken);

				var battle = await _mediator.SendValidated(new RunBattleCommand() { BattleId = command.BattleId }, cancellationToken);

				battle.Message = attackResponse.Message + battle.Message; 
				return View("Run", battle);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
			}
			return RedirectToAction(nameof(Run), new GetBattleByIdQuery() { Id = command.BattleId });
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
				var response = await _mediator.SendValidated(command, cancellationToken);

				var battle = await _mediator.SendValidated(new RunBattleCommand() { BattleId = command.BattleId }, cancellationToken);

				battle.Message = response.Message + battle.Message;
				return View("Run", battle);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
			}
			return RedirectToAction(nameof(Run), new GetBattleByIdQuery() { Id = command.BattleId });
		}

		private async Task<MakeAttackViewModel> CreateVM(AttackCommand command, CancellationToken cancellationToken)
		{
			var vm = _mapper.Map<MakeAttackViewModel>(command);

			var formAttackCommand = new FormAttackCommand
			{
				AttackerId = command.Id,
				TargetId = command.TargetCreatureId,
				AbilityId = command.AbilityId,
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
				TargetCreatureId = command.TargetCreatureId,
			};
			var formHealResponse = await _mediator.SendValidated(formHealCommand, cancellationToken);

			vm.EffectsOnTarget = formHealResponse.EffectsOnTarget;

			return vm;
		}
	}
}
