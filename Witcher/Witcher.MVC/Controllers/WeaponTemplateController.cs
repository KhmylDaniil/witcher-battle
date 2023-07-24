using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.WeaponTemplateRequests;

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
				var response = await _mediator.Send(request, cancellationToken);

				return View(response);
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync<WeaponTemplateController>(ex, async () =>
				{
					return View(await _mediator.Send(new GetWeaponTemplateQuery(), cancellationToken));
				});
			}
		}

		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Details(GetWeaponTemplateByIdQuery query, CancellationToken cancellationToken)
		{
			try
			{
				return View(await _mediator.Send(query, cancellationToken));
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync<WeaponTemplateController>(ex, async () =>
				{
					return View(await _mediator.Send(new GetWeaponTemplateQuery(), cancellationToken));
				});
			}
		}

		[Route("[controller]/[action]")]
		public ActionResult Create() => View();

		[Route("[controller]/[action]")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateWeaponTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var id = await _mediator.Send(command, cancellationToken);

				return RedirectToAction(nameof(Details), new GetWeaponTemplateByIdQuery() { Id = id });
			}
			catch (Exception ex) { return HandleException<WeaponTemplateController>(ex, () => View(command)); }
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
				await _mediator.Send(command, cancellationToken);

				return RedirectToAction(nameof(Details), new GetWeaponTemplateByIdQuery() { Id = command.Id });
			}
			catch (Exception ex) { return HandleException<WeaponTemplateController>(ex, () => View(command)); }
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
				await _mediator.Send(command, cancellationToken);

				return RedirectToAction(nameof(Index), new GetWeaponTemplateQuery());
			}
			catch (Exception ex) { return HandleException<WeaponTemplateController>(ex, () => View(command)); }
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
				await _mediator.Send(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetWeaponTemplateByIdQuery() { Id = command.WeaponTemplateId });
			}
			catch (Exception ex) { return HandleException<WeaponTemplateController>(ex, () => View(command)); }
		}

		[Route("[controller]/[action]/{weaponTemplateId}")]
		public async Task<IActionResult> DeleteAppliedCondition(DeleteAppliedConditionForWeaponTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetWeaponTemplateByIdQuery() { Id = command.WeaponTemplateId });
			}
			catch (Exception ex) { return HandleException<WeaponTemplateController>(ex, () => View(command)); }
		}
	}
}
