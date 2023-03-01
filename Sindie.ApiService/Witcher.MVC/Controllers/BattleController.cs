using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.ExtensionMethods;
using Witcher.MVC.ViewModels.Battle;

namespace Witcher.MVC.Controllers
{
	[Authorize]
	public class BattleController : BaseController
	{
		private readonly IMemoryCache _memoryCache;

		public BattleController(IMediator mediator, IGameIdService gameIdService, IMemoryCache memoryCache)
			: base(mediator, gameIdService)
		{
			_memoryCache = memoryCache;
		}

		[Route("[controller]")]
		public async Task<IActionResult> Index(GetBattleQuery query, CancellationToken cancellationToken)
		{
			ViewData["GameId"] = _gameIdService.GameId;
			IEnumerable<GetBattleResponseItem> response;
			try
			{
				response = await _mediator.SendValidated(query, cancellationToken);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;

				response = await _mediator.SendValidated(new GetBattleQuery(), cancellationToken);
			}
			return View(response);
		}

		[Route("[controller]/{id}")]
		public async Task<IActionResult> Details(GetBattleByIdQuery query, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.SendValidated(query, cancellationToken);

				return View(response);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;

				var response = await _mediator.SendValidated(new GetBattleQuery(), cancellationToken);

				return View(response);
			}
		}

		[Route("[controller]/[action]")]
		public ActionResult Create(CreateBattleCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]")]
		public async Task<IActionResult> Create(CreateBattleCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Details), new GetBattleByIdQuery() { Id = result.Id });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{id}")]
		public ActionResult Edit(ChangeBattleCommand command) => View(command);


		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Edit(ChangeBattleCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Details), new GetBattleByIdQuery() { Id = command.Id });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{id}")]
		public ActionResult Delete(DeleteBattleCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Delete(DeleteBattleCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Index), new GetBattleQuery());
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[HttpGet]
		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> CreateCreature(CreateCreatureCommandViewModel viewModel, CancellationToken cancellationToken)
		{
			return View(await CreateVM(viewModel, cancellationToken));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> CreateCreature(CreateCreatureCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetBattleByIdQuery() { Id = command.BattleId });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
			}
			return View(await CreateVM(command, cancellationToken));
		}

		[Route("[controller]/[action]/{battleId}/{id}")]
		public ActionResult ChangeCreature(ChangeCreatureCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{battleId}/{id}")]
		public async Task<IActionResult> ChangeCreature(ChangeCreatureCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Details), new GetBattleByIdQuery() { Id = command.BattleId });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{battleId}/{id}")]
		public async Task<IActionResult> GetCreatureById(GetCreatureByIdQuery command, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _mediator.SendValidated(command, cancellationToken);
				return View(result);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return RedirectToAction(nameof(Details), new GetBattleByIdQuery() { Id = command.BattleId });
			}
		}

		[Route("[controller]/[action]/{battleId}/{id}")]
		public ActionResult DeleteCreature(DeleteCreatureCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{battleId}/{id}")]
		public async Task<IActionResult> DeleteCreature(DeleteCreatureCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Details), new GetBattleByIdQuery() { Id = command.BattleId });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		/// <summary>
		/// Создание модели представления для добвления существа в битву
		/// </summary>
		async Task<CreateCreatureCommandViewModel> CreateVM(CreateCreatureCommand command, CancellationToken cancellationToken)
		{
			var viewModel = command is CreateCreatureCommandViewModel vm
				? vm
				: new CreateCreatureCommandViewModel()
				{
					BattleId = command.BattleId,
					CreatureTemplateId = command.CreatureTemplateId,
					Name = command.Name,
					Description = command.Description
				};

			viewModel.CreatureTemplatesDictionary = await GetCreatureTemplatesListToViewModel(cancellationToken);

			return viewModel;
		}

		/// <summary>
		/// Создание/извлечение из кэша данных о шаблонах существ
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private async Task<Dictionary<Guid, string>> GetCreatureTemplatesListToViewModel(CancellationToken cancellationToken)
		{
			if (_memoryCache.TryGetValue("creatureTemplates", out Dictionary<Guid, string> creatureTemplatesFromCache))
				return creatureTemplatesFromCache;
			else
			{
				var creatureTemplates = await _mediator.SendValidated(new GetCreatureTemplateQuery(), cancellationToken);

				var result = creatureTemplates.ToDictionary(x => x.Id, x => x.Name);

				_memoryCache.Set("creatureTemplates", result);
				return result;
			}
		}
	}
}
