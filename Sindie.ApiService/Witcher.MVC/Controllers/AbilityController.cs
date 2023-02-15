using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.AbilityRequests;
using Sindie.ApiService.Core.Contracts.AbilityRequests.ChangeAbility;
using Sindie.ApiService.Core.Contracts.AbilityRequests.CreateAbility;
using Sindie.ApiService.Core.Contracts.AbilityRequests.DeleteAbilitybyId;
using Sindie.ApiService.Core.Contracts.AbilityRequests.DeleteAppliedCondition;
using Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbility;
using Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbilityById;
using Sindie.ApiService.Core.Contracts.AbilityRequests.UpdateAppliedCondition;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.ExtensionMethods;

namespace Witcher.MVC.Controllers
{
	/// <summary>
	/// Контроллер способностей
	/// </summary>
	[Authorize]
	public class AbilityController : BaseController
	{
		public AbilityController(IMediator mediator) : base(mediator) { }

		[Route("[controller]/{gameId}")]
		public async Task<IActionResult> Index(GetAbilityQuery request, CancellationToken cancellationToken)
		{
			ViewData["GameId"] = request.GameId;

			try
			{
				var response = await _mediator.SendValidated(request, cancellationToken);

				return View(response);
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;

				return View(await _mediator.SendValidated(new GetAbilityQuery() { GameId = request.GameId }, cancellationToken));
			}
		}

		[Route("[controller]/{gameId}/{id}")]
		public async Task<IActionResult> Details(GetAbilityByIdQuery query, CancellationToken cancellationToken)
		{
			try
			{
				return View(await _mediator.SendValidated(query, cancellationToken));
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;

				return View(await _mediator.SendValidated(new GetAbilityQuery() { GameId = query.GameId }, cancellationToken));
			}
		}

		[Route("[controller]/[action]/{gameId}")]
		public ActionResult Create(CreateAbilityCommand command)
		{
			return View(command);
		}

		[Route("[controller]/[action]/{gameId}")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateAbilityCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var draft = await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { GameId = draft.GameId, Id = draft.Id });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{gameId}/{id}")]
		public ActionResult Edit(ChangeAbilityCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{gameId}/{id}")]
		public async Task<IActionResult> Edit(ChangeAbilityCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { GameId = command.GameId, Id = command.Id });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{gameId}/{id}")]
		public ActionResult Delete(DeleteAbilityByIdCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{gameId}/{id}")]
		public async Task<IActionResult> Delete(DeleteAbilityByIdCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Index), new GetAbilityQuery { GameId = command.GameId });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{gameId}/{abilityId}")]
		public ActionResult UpdateAppliedCondition(UpdateAppliedCondionCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{gameId}/{abilityId}")]
		public async Task<IActionResult> UpdateAppliedCondition(UpdateAppliedCondionCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { GameId = command.GameId, Id = command.AbilityId });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{gameId}/{abilityId}")]
		public async Task<IActionResult> DeleteAppliedCondition(DeleteAppliedCondionCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { GameId = command.GameId, Id = command.AbilityId });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{gameId}/{abilityId}")]
		public ActionResult CreateDefensiveSkill(CreateDefensiveSkillCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{gameId}/{abilityId}")]
		public async Task<IActionResult> CreateDefensiveSkill(CreateDefensiveSkillCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { GameId = command.GameId, Id = command.AbilityId });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{gameId}/{abilityId}")]
		public async Task<IActionResult> DeleteDefensiveSkill(DeleteDefensiveSkillCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { GameId = command.GameId, Id = command.AbilityId });
			}
			catch
			{
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { GameId = command.GameId, Id = command.AbilityId });
			}
		}
	}
}
