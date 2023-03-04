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

		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Run(GetBattleByIdQuery command, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.SendValidated(command, cancellationToken);
				return View(response);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;

				var response = await _mediator.SendValidated(new GetBattleByIdQuery() { Id = command.Id}, cancellationToken);
				return View(response);
			}
		}

		[Route("[controller]/[action]/{id}")]
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

				return RedirectToAction(nameof(Run), new GetBattleByIdQuery() { Id = command.Id });
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{battleId}/{id}")]
		public async Task<IActionResult> Attack(AttackCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.SendValidated(command, cancellationToken);

				return View(response);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
			}
			return RedirectToAction(nameof(Run), new GetBattleByIdQuery() { Id = command.BattleId });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{battleId}/{id}")]
		public async Task<IActionResult> Heal(TreatEffectCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.SendValidated(command, cancellationToken);

				return View(response);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
			}
			return RedirectToAction(nameof(Run), new GetBattleByIdQuery() { Id = command.BattleId });
		}
	}
}
