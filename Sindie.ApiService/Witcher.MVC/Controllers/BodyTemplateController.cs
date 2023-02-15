using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.BodyTemplatePartsRequests;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.ChangeBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.CreateBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.DeleteBodyTemplateById;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplateById;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.ExtensionMethods;

namespace Witcher.MVC.Controllers
{
	[Authorize]
	public class BodyTemplateController : BaseController
	{
		public BodyTemplateController(IMediator mediator) : base(mediator) { }

		[Route("[controller]/{gameId}")]
		public async Task<IActionResult> Index(GetBodyTemplateQuery request, CancellationToken cancellationToken)
		{
			ViewData["GameId"] = request.GameId;

			var response = await _mediator.SendValidated(request, cancellationToken);

			return View(response.BodyTemplatesList);
		}

		[Route("[controller]/{gameId}/{id}")]
		public async Task<IActionResult> Details(GetBodyTemplateByIdQuery query, CancellationToken cancellationToken)
		{
			var response = await _mediator.SendValidated(query, cancellationToken);

			return View(response);
		}

		[Route("[controller]/[action]/{gameId}")]
		public ActionResult Create(CreateBodyTemplateCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{gameId}")]
		public async Task<IActionResult> Create(CreateBodyTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var draft = await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetBodyTemplateByIdQuery() { GameId = draft.GameId, Id = draft.Id });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{gameId}/{id}")]
		public ActionResult Edit(ChangeBodyTemplateCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{gameId}/{id}")]
		public async Task<IActionResult> Edit(ChangeBodyTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetBodyTemplateByIdQuery() { GameId = command.GameId, Id = command.Id });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{gameId}/{id}")]
		public ActionResult EditParts(ChangeBodyTemplatePartCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{gameId}/{id}")]
		public async Task<IActionResult> EditParts(ChangeBodyTemplatePartCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetBodyTemplateByIdQuery() { GameId = command.GameId, Id = command.Id });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{gameId}/{id}")]
		public ActionResult Delete(DeleteBodyTemplateByIdCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{gameId}/{id}")]
		public async Task<IActionResult> Delete(DeleteBodyTemplateByIdCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Index), new GetBodyTemplateQuery { GameId = command.GameId });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}
	}
}
