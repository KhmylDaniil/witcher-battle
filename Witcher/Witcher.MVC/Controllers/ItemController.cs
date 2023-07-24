using MediatR;
using Microsoft.AspNetCore.Mvc;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CharacterRequests;
using Witcher.Core.Contracts.ItemRequests;
using Witcher.Core.Contracts.ItemTemplateRequests;
using Witcher.MVC.ViewModels.Item;

namespace Witcher.MVC.Controllers
{
	public class ItemController : BaseController
	{
		public ItemController(IMediator mediator, IGameIdService gameIdService) : base(mediator, gameIdService)
		{
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
				await _mediator.Send(command, cancellationToken);

				return InnerRedirectToCharacterDetails(command.CharacterId);
			}
			catch (Exception ex) { return HandleException<ItemController>(ex, () => View(command)); }
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
				await _mediator.Send(command, cancellationToken);

				return InnerRedirectToCharacterDetails(command.CharacterId);
			}
			catch (Exception ex) { return HandleException<ItemController>(ex, () => View(command)); }
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{characterId}")]
		public async Task<IActionResult> ChangeItemIsEquipped(ChangeItemIsEquippedCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(command, cancellationToken);
				return InnerRedirectToCharacterDetails(command.CharacterId);
			}
			catch (Exception ex) { return HandleException<ItemController>(ex, () => InnerRedirectToCharacterDetails(command.CharacterId)); }
		}

		private async Task<Dictionary<Guid, string>> GetItemTemplateListForViewModel(CancellationToken cancellationToken)
		{
			var itemTemplates = await _mediator.Send(new GetItemTemplateQuery() { PageSize = int.MaxValue }, cancellationToken);

			var result = itemTemplates.ToDictionary(x => x.Id, x => x.Name);
			return result;
		}

		private ActionResult InnerRedirectToCharacterDetails(Guid characterId)
				=> RedirectToAction(nameof(CharacterController.Details), "Character", new GetCharacterByIdCommand() { Id = characterId });
	}
}
