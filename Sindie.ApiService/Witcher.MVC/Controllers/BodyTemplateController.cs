using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.ChangeBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.CreateBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.DeleteBodyTemplateById;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplate;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplateById;
using Sindie.ApiService.Core.Drafts.BodyTemplateDrafts;

namespace Witcher.MVC.Controllers
{
	[Authorize]
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
		public ActionResult Create(Guid gameId) => View(new CreateBodyTemplateRequest() { GameId = gameId} );

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{gameId}")]
		public async Task<IActionResult> Create(CreateBodyTemplateRequest request, CancellationToken cancellationToken)
		{
			try
			{
				request.BodyTemplateParts = CreateBodyTemplatePartsDraft.CreateBodyPartsDraft();
				var draft = await _mediator.Send(request, cancellationToken);
				return RedirectToAction(nameof(Details), new GetBodyTemplateByIdQuery() { GameId = draft.GameId, Id = draft.Id });
			}
			catch
			{
				return View();
			}
		}

		[Route("[controller]/[action]{gameId}/{id}")]
		public ActionResult Edit(Guid gameId, Guid id)
		{
			return View(new ChangeBodyTemplateRequest() { GameId = gameId, Id = id });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]{gameId}/{id}")]
		public async Task<IActionResult> Edit(ChangeBodyTemplateRequest request, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(request, cancellationToken);
				return RedirectToAction(nameof(Details), new GetBodyTemplateByIdQuery() { GameId = request.GameId, Id = request.Id });
			}
			catch
			{
				return View();
			}
		}

		[Route("[controller]/[action]{gameId}/{id}")]
		public ActionResult Delete(Guid gameId, Guid id) => View(new DeleteBodyTemplateByIdCommand() { GameId = gameId, Id = id });

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]{gameId}/{id}")]
		public async Task<IActionResult> Delete(DeleteBodyTemplateByIdCommand request, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(request, cancellationToken);
				return RedirectToAction(nameof(Index), new GetBodyTemplateQuery { GameId = request.GameId });
			}
			catch
			{
				return View();
			}
		}
	}
}
