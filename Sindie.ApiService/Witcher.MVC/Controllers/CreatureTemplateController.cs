using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplateById;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplate;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplateById;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.ExtensionMethods;

namespace Witcher.MVC.Controllers
{
	[Authorize]

	public class CreatureTemplateController : BaseController
	{
		public CreatureTemplateController(IMediator mediator) : base(mediator) { }

		[Route("[controller]/{gameId}")]
		public async Task<IActionResult> Index(GetCreatureTemplateQuery query, CancellationToken cancellationToken)
		{
			ViewData["GameId"] = query.GameId;

			try
			{
				var response = await _mediator.SendValidated(query, cancellationToken);

				return View(response.CreatureTemplatesList);
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;

				var response = await _mediator.SendValidated(new GetCreatureTemplateQuery() { GameId = query.GameId }, cancellationToken);

				return View(response.CreatureTemplatesList);
			}
		}

		[Route("[controller]/{gameId}/{id}")]
		public async Task<IActionResult> Details(GetCreatureTemplateByIdQuery query, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.SendValidated(query, cancellationToken);

				return View(response);
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;

				var response = await _mediator.SendValidated(new GetCreatureTemplateQuery() { GameId = query.GameId }, cancellationToken);

				return View(response.CreatureTemplatesList);
			}
		}

		// GET: CreatureTemplateController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: CreatureTemplateController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: CreatureTemplateController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: CreatureTemplateController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: CreatureTemplateController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: CreatureTemplateController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
