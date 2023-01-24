using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.GameRequests.ChangeGame;
using Sindie.ApiService.Core.Contracts.GameRequests.CreateGame;
using Sindie.ApiService.Core.Contracts.GameRequests.GetGame;
using Sindie.ApiService.Core.Contracts.GameRequests.GetGameById;

namespace Witcher.MVC.Controllers
{
	[Authorize]
	public class GameController : Controller
	{
		private readonly IMediator _mediator;

		public GameController(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<IActionResult> Index(string name, string description, string authorName, CancellationToken cancellationToken)
		{
			var query = new GetGameQuery() { Name = name, Description = description, AuthorName = authorName};
			
			var response = await _mediator.Send(query, cancellationToken);

			return View(response.GamesList);
		}

		[Route("[controller]/{id}")]
		public async Task<IActionResult> EnterAsync(Guid id, CancellationToken cancellationToken)
		{
			if (id == Guid.Empty && TempData["GameId"] is not null)
				id = (Guid)TempData["GameId"];

			var command = new GetGameByIdCommand() { Id = id };

			try
			{
				var response = await _mediator.Send(command, cancellationToken);

				return View(response);
			}
			catch
			{
				ViewData["ErrorMessage"] = "You`re not authorized to requested game.";
				return View("Error");
			}
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateGameCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(command ?? throw new ArgumentNullException(nameof(command)), cancellationToken);
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: GameController/Edit/5
		[Route("[controller]/[action]/{id}")]
		public ActionResult Edit()
		{
			return View();
		}

		// POST: GameController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Edit(ChangeGameCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(command ?? throw new ArgumentNullException(nameof(command)), cancellationToken);

				TempData["GameId"] = command.Id;

				return RedirectToAction(nameof(EnterAsync));
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = ex.Message ?? "You`re don`t have permission to make changes in requested game.";
				return View("Error");
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
