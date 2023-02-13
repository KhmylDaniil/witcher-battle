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

			var response = await _mediator.Send(request, cancellationToken);

			return View(response);
		}

		[Route("[controller]/{gameId}/{id}")]
		public async Task<IActionResult> Details(GetAbilityByIdQuery query, CancellationToken cancellationToken)
		{
			var response = await _mediator.Send(query, cancellationToken);

			return View(response);
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
				var draft = await _mediator.Send(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { GameId = draft.GameId, Id = draft.Id });
			}
			catch
			{
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
				await _mediator.Send(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { GameId = command.GameId, Id = command.Id });
			}
			catch
			{
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
				await _mediator.Send(command, cancellationToken);
				return RedirectToAction(nameof(Index), new GetAbilityQuery { GameId = command.GameId });
			}
			catch
			{
				return View();
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
				await _mediator.Send(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { GameId = command.GameId, Id = command.AbilityId });
			}
			catch
			{
				return View(command);
			}
		}

		[Route("[controller]/[action]/{gameId}/{abilityId}")]
		public async Task<IActionResult> DeleteAppliedCondition(DeleteAppliedCondionCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { GameId = command.GameId, Id = command.AbilityId });
			}
			catch
			{
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
				await _mediator.Send(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { GameId = command.GameId, Id = command.AbilityId });
			}
			catch
			{
				return View(command);
			}
		}

		[Route("[controller]/[action]/{gameId}/{abilityId}")]
		public async Task<IActionResult> DeleteDefensiveSkill(DeleteDefensiveSkillCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { GameId = command.GameId, Id = command.AbilityId });
			}
			catch
			{
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { GameId = command.GameId, Id = command.AbilityId });
			}
		}
	}
}
