using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplateById;

namespace Witcher.MVC.Controllers
{
	public class BodyTemplateController : Controller
	{
		private readonly IMediator _mediator;

		public BodyTemplateController(IMediator mediator)
		{
			_mediator = mediator;
		}

		// GET: BodyTemplateController
		[Route("[controller]/{gameId}")]
		public async Task<IActionResult> Index(Guid gameId, string name, string authorName, string bodyPartName, CancellationToken cancellationToken)
		{
			var request = new GetBodyTemplateQuery() { GameId = gameId, Name = name, UserName = authorName, BodyPartName = bodyPartName};

			var response = await _mediator.Send(request, cancellationToken);

			return View(response.BodyTemplatesList);
		}

		// GET: BodyTemplateController/Details/5
		[Route("[controller]/{gameId}/{id}")]
		public async Task<IActionResult> Details(GetBodyTemplateByIdQuery query, CancellationToken cancellationToken)
		{
			var response = await _mediator.Send(query, cancellationToken);

			return View(response);
		}

		// GET: BodyTemplateController/Create
		[Route("[controller]/[action]/{gameId}")]
		public ActionResult Create()
		{
			return View();
		}

		// POST: BodyTemplateController/Create
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
