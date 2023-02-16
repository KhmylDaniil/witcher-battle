using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.GameRequests.ChangeGame;
using Sindie.ApiService.Core.Contracts.GameRequests.CreateGame;
using Sindie.ApiService.Core.Contracts.GameRequests.DeleteGame;
using Sindie.ApiService.Core.Contracts.GameRequests.GetGame;
using Sindie.ApiService.Core.Contracts.GameRequests.GetGameById;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.ExtensionMethods;

namespace Witcher.MVC.Controllers
{
	[Authorize]
	public class GameController : BaseController
	{
		public GameController(IMediator mediator) : base(mediator) { }

		public async Task<IActionResult> Index(string name, string description, string authorName, CancellationToken cancellationToken)
		{
			var query = new GetGameQuery() { Name = name, Description = description, AuthorName = authorName};
			
			var response = await _mediator.SendValidated(query, cancellationToken);

			return View(response.GamesList);
		}

		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Enter(GetGameByIdCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.SendValidated(command, cancellationToken);

				return View(response);
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]")]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]")]
		public async Task<IActionResult> Create(CreateGameCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command ?? throw new ArgumentNullException(nameof(command)), cancellationToken);
				return RedirectToAction(nameof(Index));
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		// GET: GameController/Edit/5
		[Route("[controller]/[action]/{id}")]
		public ActionResult Edit(ChangeGameCommand command)
		{
			return View(command);
		}

		// POST: GameController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Edit(ChangeGameCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command ?? throw new ArgumentNullException(nameof(command)), cancellationToken);

				return RedirectToAction(nameof(Enter), new GetGameByIdCommand { Id = command.Id } );
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{id}")]
		public ActionResult Delete(DeleteGameCommand command)
		{
			TempData["GameId"] = command.Id;
			return View(command);
		}

		// POST: GameController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Delete(DeleteGameCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command ?? throw new ArgumentNullException(nameof(command)), cancellationToken);

				return RedirectToAction(nameof(Index));
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}
	}
}
