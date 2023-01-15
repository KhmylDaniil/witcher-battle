using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.GameRequests.GetGame;

namespace Witcher.MVC.Controllers
{
	public class GameController : Controller
	{
		private readonly IMediator _mediator;

		public GameController(IMediator mediator)
		{
			_mediator = mediator;
		}

		// GET: GameController
		public async Task<IActionResult> Index(string? name, string? userName, CancellationToken cancellationToken)
		{
			var query = new GetGameQuery() { Name = name, AuthorName = userName };
			
			var response = await _mediator.Send(query, cancellationToken);
			
			return View(response.GamesList);
		}

		// GET: GameController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: GameController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: GameController/Create
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

		// GET: GameController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: GameController/Edit/5
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

		// GET: GameController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: GameController/Delete/5
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
