using MediatR;
using Microsoft.AspNetCore.Mvc;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CharacterRequests;
using Witcher.Core.Contracts.ItemRequests;
using Witcher.Core.Contracts.ItemTemplateRequests;
using Witcher.Core.Exceptions;
using Witcher.Core.ExtensionMethods;
using Witcher.MVC.ViewModels.Item;

namespace Witcher.MVC.Controllers
{
	public class ItemController : BaseController
	{
		public ItemController(IMediator mediator, IGameIdService gameIdService) : base(mediator, gameIdService)
		{
		}

		// GET: BagController
		public ActionResult Index()
		{
			return View();
		}

		// GET: BagController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		[HttpGet]
		[Route("[controller]/[action]/{characterId}")]
		public async Task<IActionResult> Create(CreateItemViewModel vm, CancellationToken cancellationToken)
		{
			vm.ItemTemplatesDictionary = await GetItemTemplateListForViewModel(cancellationToken);
			return View(vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{characterId}")]
		public async Task<IActionResult> Create(CreateItemCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Details), "Character", new GetCharacterByIdCommand() { Id = command.CharacterId });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
			catch (Exception ex) { return RedirectToErrorPage<ItemController>(ex); }
		}

		[HttpGet]
		[Route("[controller]/[action]/{characterId}")]
		public ActionResult Delete(DeleteItemCommand command)
		{
			return View(command);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{characterId}")]
		public async Task<IActionResult> Delete(DeleteItemCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Details), "Character", new GetCharacterByIdCommand() { Id = command.CharacterId });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
			catch (Exception ex) { return RedirectToErrorPage<ItemController>(ex); }
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{characterId}")]
		public async Task<IActionResult> ChangeItemIsEquipped(ChangeItemIsEquippedCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
			}
			catch (Exception ex) { return RedirectToErrorPage<ItemController>(ex); }

			return RedirectToAction(nameof(Details), "Character", new GetCharacterByIdCommand() { Id = command.CharacterId });
		}

		private async Task<Dictionary<Guid, string>> GetItemTemplateListForViewModel(CancellationToken cancellationToken)
		{
			var itemTemplates = await _mediator.SendValidated(new GetItemTemplateQuery() { PageSize = int.MaxValue }, cancellationToken);

			var result = itemTemplates.ToDictionary(x => x.Id, x => x.Name);

			return result;
		}
	}
}
