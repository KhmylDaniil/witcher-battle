using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Contracts.WeaponTemplateRequests;
using Witcher.Core.Exceptions;
using Witcher.Core.ExtensionMethods;

namespace Witcher.MVC.Controllers
{
	[Authorize]
	public class WeaponTemplateController : BaseController
	{
		public WeaponTemplateController(IMediator mediator, IGameIdService gameIdService) : base(mediator, gameIdService)
		{
		}

		[Route("[controller]/[action]")]
		public async Task<IActionResult> Index(GetWeaponTemplateQuery request, CancellationToken cancellationToken)
		{
			ViewData["GameId"] = _gameIdService.GameId;

			try
			{
				var response = await _mediator.SendValidated(request, cancellationToken);

				return View(response);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;

				return View(await _mediator.SendValidated(new GetWeaponTemplateQuery(), cancellationToken));
			}
			catch (Exception ex) { return RedirectToErrorPage<WeaponTemplateController>(ex); }
		}

		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Details(GetWeaponTemplateByIdQuery query, CancellationToken cancellationToken)
		{
			try
			{
				return View(await _mediator.SendValidated(query, cancellationToken));
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;

				return View(await _mediator.SendValidated(new GetWeaponTemplateQuery(), cancellationToken));
			}
			catch (Exception ex) { return RedirectToErrorPage<WeaponTemplateController>(ex); }
		}

		[Route("[controller]/[action]")]
		public ActionResult Create(CreateWeaponTemplateCommand command) => View(command);

		[Route("[controller]/[action]")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateWeaponTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var id = await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Details), new GetWeaponTemplateByIdQuery() { Id = id });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
			catch (Exception ex) { return RedirectToErrorPage<WeaponTemplateController>(ex); }
		}

		[Route("[controller]/[action]/{id}")]
		public ActionResult Edit(ChangeWeaponTemplateCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Edit(ChangeWeaponTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Details), new GetWeaponTemplateByIdQuery() { Id = command.Id });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
			catch (Exception ex) { return RedirectToErrorPage<WeaponTemplateController>(ex); }
		}

		[Route("[controller]/[action]/{id}")]
		public ActionResult Delete(DeleteWeaponTemplateCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Delete(DeleteWeaponTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Index), new GetWeaponTemplateQuery());
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
			catch (Exception ex) { return RedirectToErrorPage<WeaponTemplateController>(ex); }
		}

		[Route("[controller]/[action]/{weaponTemplateId}")]
		public ActionResult UpdateAppliedCondition(UpdateAppliedConditionForWeaponTemplateCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{weaponTemplateId}")]
		public async Task<IActionResult> UpdateAppliedCondition(UpdateAppliedConditionForWeaponTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetWeaponTemplateByIdQuery() { Id = command.WeaponTemplateId });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
			catch (Exception ex) { return RedirectToErrorPage<WeaponTemplateController>(ex); }
		}

		[Route("[controller]/[action]/{weaponTemplateId}")]
		public async Task<IActionResult> DeleteAppliedCondition(DeleteAppliedConditionForWeaponTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { Id = command.WeaponTemplateId });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
			catch (Exception ex) { return RedirectToErrorPage<WeaponTemplateController>(ex); }
		}
	}
}
