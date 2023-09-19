using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Contracts.CharacterRequests;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.ExtensionMethods;
using Witcher.MVC.ViewModels.Battle;

namespace Witcher.MVC.Controllers
{
	[Authorize]
	public class BattleController : BaseController
	{
		private readonly IMemoryCache _memoryCache;
		private readonly IMapper _mapper;

		public BattleController(IMediator mediator, IGameIdService gameIdService, IMemoryCache memoryCache, IMapper mapper)
			: base(mediator, gameIdService)
		{
			_memoryCache = memoryCache;
			_mapper = mapper;
		}

		[Route("[controller]")]
		public async Task<IActionResult> Index(GetBattleQuery query, CancellationToken cancellationToken)
		{
			ViewData["GameId"] = _gameIdService.GameId;
			IEnumerable<GetBattleResponseItem> response;
			try
			{
				response = await _mediator.Send(query, cancellationToken);
				return View(response);
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync<BattleController>(ex, async () =>
				{
					return View(await _mediator.Send(new GetBattleQuery(), cancellationToken));
				});
			}
		}

		[Route("[controller]/{id}")]
		public async Task<IActionResult> Details(GetBattleByIdQuery query, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.Send(query, cancellationToken);

				return View(response);
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync<BattleController>(ex, async () =>
				{
					return View(await _mediator.Send(new GetBattleQuery(), cancellationToken));
				});
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
				var result = await _mediator.Send(command, cancellationToken);

				return RedirectToAction(nameof(Details), new GetBattleByIdQuery() { Id = result.Id });
			}
			catch (Exception ex)
			{
				return HandleException<BattleController>(ex, () => View(command));
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
				await _mediator.Send(command, cancellationToken);

				return RedirectToAction(nameof(Details), new GetBattleByIdQuery() { Id = command.Id });
			}
			catch (Exception ex)
			{
				return HandleException<BattleController>(ex, () => View(command));
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
				await _mediator.Send(command, cancellationToken);

				return RedirectToAction(nameof(Index), new GetBattleQuery());
			}
			catch (Exception ex)
			{
				return HandleException<BattleController>(ex, () => View(command));
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
				await _mediator.Send(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetBattleByIdQuery() { Id = command.BattleId });
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync<BattleController>(ex, async () =>
				{
					return View(await CreateVM(command, cancellationToken));
				});
			}

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
				await _mediator.Send(command, cancellationToken);

				return RedirectToAction(nameof(Details), new GetBattleByIdQuery() { Id = command.BattleId });
			}
			catch (Exception ex)
			{
				return HandleException<BattleController>(ex, () => View(command));
			}
		}

		[Route("[controller]/[action]/{battleId}/{id}")]
		public async Task<IActionResult> GetCreatureById(GetCreatureByIdQuery command, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _mediator.Send(command, cancellationToken);
				return View(result);
			}
			catch (Exception ex)
			{
				return HandleException<BattleController>(ex, () => RedirectToAction(nameof(Details), new GetBattleByIdQuery() { Id = command.BattleId }));
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
				await _mediator.Send(command, cancellationToken);

				return RedirectToAction(nameof(Details), new GetBattleByIdQuery() { Id = command.BattleId });
			}
			catch (Exception ex)
			{
				return HandleException<BattleController>(ex, () => View(command));
			}
		}

		[HttpGet]
		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> AddCharacter(AddCharacterToBattleViewModel viewModel, CancellationToken cancellationToken)
		{
			return View(await CreateVM(viewModel, cancellationToken));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{battleId}")]
		public async Task<IActionResult> AddCharacter(AddCharacterToBattleCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetBattleByIdQuery() { Id = command.BattleId });
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync<BattleController>(ex, async () => View(await CreateVM(command, cancellationToken)));
			}
		}

		/// <summary>
		/// Создание модели представления для добавления существа в битву
		/// </summary>
		async Task<CreateCreatureCommandViewModel> CreateVM(CreateCreatureCommand command, CancellationToken cancellationToken)
		{
			var viewModel = command is CreateCreatureCommandViewModel vm
				? vm
				: _mapper.Map<CreateCreatureCommandViewModel>(command);

			viewModel.CreatureTemplatesDictionary = await GetCreatureTemplatesListToViewModel(cancellationToken);

			return viewModel;
		}

		/// <summary>
		/// Создание модели представления для добваления персонажа в битву
		/// </summary>
		async Task<AddCharacterToBattleViewModel> CreateVM(AddCharacterToBattleCommand command, CancellationToken cancellationToken)
		{
			var viewModel = command is AddCharacterToBattleViewModel vm
				? vm
				: _mapper.Map<AddCharacterToBattleViewModel>(command);

			viewModel.CharactersDictionary = await GetCharactersListToViewModel(cancellationToken);

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
				var creatureTemplates = await _mediator.Send(new GetCreatureTemplateQuery() { PageSize = int.MaxValue }, cancellationToken);

				var result = creatureTemplates.ToDictionary(x => x.Id, x => x.Name);

				_memoryCache.Set("creatureTemplates", result);
				return result;
			}
		}

		/// <summary>
		/// Создание/извлечение из кэша данных о персонажах
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private async Task<Dictionary<Guid, (string, string)>> GetCharactersListToViewModel(CancellationToken cancellationToken)
		{
			if (_memoryCache.TryGetValue("characters", out Dictionary<Guid, (string, string)> charactersFromCache))
				return charactersFromCache;
			else
			{
				var characters = await _mediator.Send(new GetCharactersCommand() { PageSize = int.MaxValue }, cancellationToken);

				var result = characters.ToDictionary(x => x.Id, x => (x.Name, x.OwnerName));

				_memoryCache.Set("characters", result);
				return result;
			}
		}
	}
}
