using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CharacterRequests;
using Witcher.Core.Exceptions;
using Witcher.Core.ExtensionMethods;

namespace Witcher.MVC.Controllers
{
	public sealed class CharacterController : BaseController
	{
		private readonly IMemoryCache _memoryCache;

		public CharacterController(IMediator mediator, IGameIdService gameIdService, IMemoryCache memoryCache) : base(mediator, gameIdService)
		{
			_memoryCache = memoryCache;
		}

		[Route("[controller]")]
		public async Task<IActionResult> Index(GetCharactersCommand query, CancellationToken cancellationToken)
		{
			ViewData["GameId"] = _gameIdService.GameId;
			IEnumerable<GetCharactersResponseItem> response;
			try
			{
				response = await _mediator.Send(query, cancellationToken);
				return View(response);
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync<CharacterController>(ex, async () =>
				{
					return View(await _mediator.Send(new GetCharactersCommand(), cancellationToken));
				});
			}
		}

		[Route("[controller]/{id}")]
		public async Task<IActionResult> Details(GetCharacterByIdCommand request, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.Send(request, cancellationToken);

				return View(response);
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync<CharacterController>(ex, async () =>
				{
					return View(await _mediator.Send(new GetCharactersCommand(), cancellationToken));
				});
			}
		}

		[HttpGet]
		[Route("[controller]/[action]")]
		public ActionResult Create(CreateCharacterCommand command)
		{
			return View(command);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]")]
		public async Task<IActionResult> Create(CreateCharacterCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _mediator.Send(command, cancellationToken);

				_memoryCache.Remove("characters");

				return RedirectToAction(nameof(Details), new GetCharacterByIdCommand() { Id = result.Id });
			}
			catch (Exception ex)
			{
				return HandleException<CharacterController>(ex, () => View(command));
			}
		}

		[Route("[controller]/[action]/{id}")]
		public ActionResult Delete(DeleteCharacterCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Delete(DeleteCharacterCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(command, cancellationToken);

				_memoryCache.Remove("characters");

				return RedirectToAction(nameof(Index), new GetCharactersCommand());
			}
			catch (Exception ex)
			{
				return HandleException<CharacterController>(ex, () => View(command));
			}
		}
	}
}
