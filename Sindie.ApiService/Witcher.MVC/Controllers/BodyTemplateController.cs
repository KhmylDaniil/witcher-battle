using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.ChangeBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.CreateBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplateById;
using Witcher.MVC.ViewModels.Drafts;

namespace Witcher.MVC.Controllers
{
	public class BodyTemplateController : Controller
	{
		private readonly IMediator _mediator;

		public BodyTemplateController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[Route("[controller]/{gameId}")]
		public async Task<IActionResult> Index(Guid gameId, string name, string authorName, string bodyPartName, CancellationToken cancellationToken)
		{
			ViewData["GameId"] = gameId;

			var request = new GetBodyTemplateQuery() { GameId = gameId, Name = name, UserName = authorName, BodyPartName = bodyPartName};

			var response = await _mediator.Send(request, cancellationToken);

			return View(response.BodyTemplatesList);
		}

		[Route("[controller]/{gameId}/{id}")]
		public async Task<IActionResult> Details(GetBodyTemplateByIdQuery query, CancellationToken cancellationToken)
		{
			var response = await _mediator.Send(query, cancellationToken);

			return View(response);
		}

		[Route("[controller]/[action]/{gameId}")]
		public async Task<IActionResult> Create(Guid gameId)
		{
			var draft = await _mediator.Send(CreateBodyTemplateDraft.CreateDraft(gameId));
			
			return View(draft);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{gameId}")]
		public async Task<IActionResult> Create(ChangeBodyTemplateRequest request)
		{
			try
			{
				await _mediator.Send(request);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: BodyTemplateController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: BodyTemplateController/Edit/5
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

		// GET: BodyTemplateController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: BodyTemplateController/Delete/5
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
