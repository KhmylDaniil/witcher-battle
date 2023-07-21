using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BodyTemplateRequests;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Witcher.MVC.Controllers
{
	[Authorize]
	public class BodyTemplateController : BaseController
	{
		private readonly IMemoryCache _memoryCache;

		public BodyTemplateController(IMediator mediator, IGameIdService gameIdService, IMemoryCache memoryCache)
			: base(mediator, gameIdService)
		{
			_memoryCache = memoryCache;
		}

		[Route("[controller]/[action]")]
		public async Task<IActionResult> Index(GetBodyTemplateQuery query, CancellationToken cancellationToken)
		{
			ViewData["GameId"] = _gameIdService.GameId;

			try
			{
				var response = await _mediator.Send(query, cancellationToken);

				return View(response);
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync<BodyTemplateController>(ex, async () =>
				{
					return View(await _mediator.Send(new GetBodyTemplateQuery(), cancellationToken));
				});
			}
		}

		[Route("[controller]/{id}")]
		public async Task<IActionResult> Details(GetBodyTemplateByIdQuery query, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.Send(query, cancellationToken);

				return View(response);
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync<BodyTemplateController>(ex, async () =>
				{
					return View(await _mediator.Send(new GetBodyTemplateQuery(), cancellationToken));
				});
			}
		}

		[Route("[controller]/[action]")]
		public ActionResult Create() => View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]")]
		public async Task<IActionResult> Create(CreateBodyTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var draft = await _mediator.Send(command, cancellationToken);

				_memoryCache.Remove("bodyTemplates");

				return RedirectToAction(nameof(Details), new GetBodyTemplateByIdQuery() { Id = draft.Id });
			}
			catch (Exception ex)
			{
				return HandleException<BodyTemplateController>(ex,() => View(command));
			}
		}

		[Route("[controller]/[action]/{id}")]
		public ActionResult Edit(ChangeBodyTemplateCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Edit(ChangeBodyTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(command, cancellationToken);

				_memoryCache.Remove("bodyTemplates");

				return RedirectToAction(nameof(Details), new GetBodyTemplateByIdQuery() { Id = command.Id });
			}
			catch (Exception ex)
			{
				return HandleException<BodyTemplateController>(ex, () => View(command));
			}
		}

		//TODO made full CRUD
		[Route("[controller]/[action]/{id}")]
		public ActionResult EditParts(ChangeBodyTemplatePartCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> EditParts(ChangeBodyTemplatePartCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetBodyTemplateByIdQuery() { Id = command.Id });
			}
			catch (Exception ex)
			{
				return HandleException<BodyTemplateController>(ex, () => View(command));
			}
		}

		[Route("[controller]/[action]/{id}")]
		public ActionResult Delete(DeleteBodyTemplateByIdCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Delete(DeleteBodyTemplateByIdCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(command, cancellationToken);

				_memoryCache.Remove("bodyTemplates");

				return RedirectToAction(nameof(Index), new GetBodyTemplateQuery());
			}
			catch (Exception ex)
			{
				return HandleException<BodyTemplateController>(ex, () => View(command));
			}
		}
	}
}
