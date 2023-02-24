using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.AbilityRequests;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.ExtensionMethods;
using Sindie.ApiService.Core.Services.Authorization;

namespace Witcher.MVC.Controllers
{
	/// <summary>
	/// Контроллер способностей
	/// </summary>
	[Authorize]
	public class AbilityController : BaseController
	{
		public AbilityController(IMediator mediator, IGameIdService gameIdService) : base(mediator, gameIdService) { }

		[Route("[controller]/[action]")]
		public async Task<IActionResult> Index(GetAbilityQuery request, CancellationToken cancellationToken)
		{
			ViewData["GameId"] = _gameIdService.GameId;
			
			try
			{
				var response = await _mediator.SendValidated(request, cancellationToken);

				return View(response);
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;

				return View(await _mediator.SendValidated(new GetAbilityQuery(), cancellationToken));
			}
		}

		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Details(GetAbilityByIdQuery query, CancellationToken cancellationToken)
		{
			try
			{
				return View(await _mediator.SendValidated(query, cancellationToken));
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;

				return View(await _mediator.SendValidated(new GetAbilityQuery(), cancellationToken));
			}
		}

		[Route("[controller]/[action]")]
		public ActionResult Create(CreateAbilityCommand command)
		{
			return View(command);
		}

		[Route("[controller]/[action]")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateAbilityCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var draft = await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { Id = draft.Id });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{id}")]
		public ActionResult Edit(ChangeAbilityCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Edit(ChangeAbilityCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() {Id = command.Id });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{id}")]
		public ActionResult Delete(DeleteAbilityByIdCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Delete(DeleteAbilityByIdCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Index), new GetAbilityQuery());
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{abilityId}")]
		public ActionResult UpdateAppliedCondition(UpdateAppliedCondionCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{abilityId}")]
		public async Task<IActionResult> UpdateAppliedCondition(UpdateAppliedCondionCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { Id = command.AbilityId });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{abilityId}")]
		public async Task<IActionResult> DeleteAppliedCondition(DeleteAppliedCondionCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { Id = command.AbilityId });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{abilityId}")]
		public ActionResult CreateDefensiveSkill(CreateDefensiveSkillCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{abilityId}")]
		public async Task<IActionResult> CreateDefensiveSkill(CreateDefensiveSkillCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { Id = command.AbilityId });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{abilityId}")]
		public async Task<IActionResult> DeleteDefensiveSkill(DeleteDefensiveSkillCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { Id = command.AbilityId });
			}
			catch
			{
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { Id = command.AbilityId });
			}
		}
	}
}
