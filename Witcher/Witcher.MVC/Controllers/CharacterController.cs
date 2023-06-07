using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CharacterRequests;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Exceptions;
using Witcher.Core.ExtensionMethods;
using Witcher.MVC.ViewModels.CreatureTemplate;

namespace Witcher.MVC.Controllers
{
	public class CharacterController : BaseController
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
				response = await _mediator.SendValidated(query, cancellationToken);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;

				response = await _mediator.SendValidated(new GetCharactersCommand(), cancellationToken);
			}
			catch (Exception ex) { return RedirectToErrorPage<CharacterController>(ex); }

			return View(response);
		}

		[Route("[controller]/{id}")]
		public async Task<IActionResult> Details(GetCharacterByIdCommand request, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.SendValidated(request, cancellationToken);

				return View(response);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;

				var response = await _mediator.SendValidated(new GetCharactersCommand(), cancellationToken);

				return View(response);
			}
			catch (Exception ex) { return RedirectToErrorPage<CharacterController>(ex); }
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
				var result = await _mediator.SendValidated(command, cancellationToken);

				_memoryCache.Remove("characters");

				return RedirectToAction(nameof(Details), new GetCharacterByIdCommand() { Id = result.Id });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
			catch (Exception ex) { return RedirectToErrorPage<CharacterController>(ex); }
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
				await _mediator.SendValidated(command, cancellationToken);

				_memoryCache.Remove("characters");

				return RedirectToAction(nameof(Index), new GetCharactersCommand());
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
			catch (Exception ex) { return RedirectToErrorPage<CharacterController>(ex); }
		}
	}
}
