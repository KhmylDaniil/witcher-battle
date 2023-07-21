using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Contracts.BodyTemplateRequests;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Exceptions;
using Witcher.Core.ExtensionMethods;
using Witcher.MVC.ViewModels.CreatureTemplate;

namespace Witcher.MVC.Controllers
{
	[Authorize]
	public class CreatureTemplateController : BaseController
	{
		private readonly IMemoryCache _memoryCache;
		private readonly IMapper _mapper;

		public CreatureTemplateController(IMediator mediator, IGameIdService gameIdService, IMemoryCache memoryCache, IMapper mapper)
			: base(mediator, gameIdService)
		{
			_memoryCache = memoryCache;
			_mapper = mapper;
		}

		[Route("[controller]")]
		public async Task<IActionResult> Index(GetCreatureTemplateQuery query, CancellationToken cancellationToken)
		{
			ViewData["GameId"] = _gameIdService.GameId;
			IEnumerable<GetCreatureTemplateResponseItem> response;
			try
			{
				response = await _mediator.SendValidated(query, cancellationToken);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;

				response = await _mediator.SendValidated(new GetCreatureTemplateQuery() , cancellationToken);
			}
			catch (Exception ex) { return RedirectToErrorPage<CreatureTemplateController>(ex); }

			return View(response);
		}

		[Route("[controller]/{id}")]
		public async Task<IActionResult> Details(GetCreatureTemplateByIdQuery query, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.SendValidated(query, cancellationToken);

				return View(response);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;

				var response = await _mediator.SendValidated(new GetCreatureTemplateQuery(), cancellationToken);

				return View(response);
			}
			catch (Exception ex) { return RedirectToErrorPage<CreatureTemplateController>(ex); }
		}

		[HttpGet]
		[Route("[controller]/[action]")]
		public async Task<IActionResult> Create(CreateCreatureTemplateCommandViewModel viewModel, CancellationToken cancellationToken)
		{
			return View(await CreateVM(viewModel, cancellationToken));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]")]
		public async Task<IActionResult> Create(CreateCreatureTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _mediator.SendValidated(command, cancellationToken);

				_memoryCache.Remove("creatureTemplates");

				return RedirectToAction(nameof(Details), new GetCreatureTemplateByIdQuery() { Id = result.Id });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
			}
			catch (Exception ex) { return RedirectToErrorPage<CreatureTemplateController>(ex); }

			return View(await CreateVM(command, cancellationToken));
		}

		[HttpGet]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Edit(ChangeCreatureTemplateCommandViewModel viewModel, CancellationToken cancellationToken)
			=> View(await CreateVM(viewModel, cancellationToken));

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Edit(ChangeCreatureTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				_memoryCache.Remove("creatureTemplates");

				return RedirectToAction(nameof(Details), new GetCreatureTemplateByIdQuery() { Id = command.Id });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
			}
			catch (Exception ex) { return RedirectToErrorPage<CreatureTemplateController>(ex); }

			return View(await CreateVM(command, cancellationToken));
		}

		[Route("[controller]/[action]/{creatureTemplateId}/{id?}")]
		public ActionResult EditParts(ChangeCreatureTemplatePartCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{creatureTemplateId}/{id?}")]
		public async Task<IActionResult> EditParts(ChangeCreatureTemplatePartCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetCreatureTemplateByIdQuery() { Id = command.CreatureTemplateId });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
            catch (Exception ex) { return RedirectToErrorPage<CreatureTemplateController>(ex); }
		}

		[Route("[controller]/[action]/{id}")]
		public ActionResult Delete(DeleteCreatureTemplateByIdCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Delete(DeleteCreatureTemplateByIdCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				_memoryCache.Remove("creatureTemplates");

				return RedirectToAction(nameof(Index), new GetCreatureTemplateQuery());
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
            catch (Exception ex) { return RedirectToErrorPage<CreatureTemplateController>(ex); }
		}

		[Route("[controller]/[action]/{creatureTemplateId}")]
		public ActionResult EditSkill(UpdateCreatureTemplateSkillCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{creatureTemplateId}")]
		public async Task<IActionResult> EditSkill(UpdateCreatureTemplateSkillCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetCreatureTemplateByIdQuery() { Id = command.CreatureTemplateId });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
            catch (Exception ex) { return RedirectToErrorPage<CreatureTemplateController>(ex); }
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{creatureTemplateId}")]
		public async Task<IActionResult> DeleteSkill(DeleteCreatureTemplateSkillCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetAbilityByIdQuery() { Id = command.CreatureTemplateId });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
            catch (Exception ex) { return RedirectToErrorPage<CreatureTemplateController>(ex); }
		}

		[Route("[controller]/[action]/{creatureTemplateId}")]
		public ActionResult EditDamageTypeModifier(ChangeDamageTypeModifierForCreatureTemplateCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{creatureTemplateId}")]
		public async Task<IActionResult> EditDamageTypeModifier(ChangeDamageTypeModifierForCreatureTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetCreatureTemplateByIdQuery() { Id = command.CreatureTemplateId });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
            catch (Exception ex) { return RedirectToErrorPage<CreatureTemplateController>(ex); }
		}

		/// <summary>
		/// Создание модели представления для создания шаблона существа
		/// </summary>
		async Task<CreateCreatureTemplateCommandViewModel> CreateVM(CreateCreatureTemplateCommand command, CancellationToken cancellationToken)
		{
			var viewModel = command is CreateCreatureTemplateCommandViewModel vm
				? vm
				: _mapper.Map<CreateCreatureTemplateCommandViewModel>(command);

			viewModel.BodyTemplatesDictionary = await GetBodyTemplateListForViewModel(cancellationToken);
			viewModel.AbilitiesDictionary = await GetAbilityListToViewModel(cancellationToken);

			return viewModel;
		}

		/// <summary>
		/// Создание модели представления для изменения шаблона существа
		/// </summary>
		async Task<ChangeCreatureTemplateCommandViewModel> CreateVM(ChangeCreatureTemplateCommand command, CancellationToken cancellationToken)
		{
			var viewModel = command is ChangeCreatureTemplateCommandViewModel vm
				? vm
				: _mapper.Map<ChangeCreatureTemplateCommandViewModel>(command);

			viewModel.BodyTemplatesDictionary = await GetBodyTemplateListForViewModel(cancellationToken);
			viewModel.AbilitiesDictionary = await GetAbilityListToViewModel(cancellationToken);

			return viewModel;
		}

		/// <summary>
		/// Создание/извлечение из кэша данных о способностях
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private async Task<Dictionary<Guid, string>> GetAbilityListToViewModel(CancellationToken cancellationToken)
		{
			if (_memoryCache.TryGetValue("abilities", out Dictionary<Guid, string> abilitiesFromCache))
				return abilitiesFromCache;
			else
			{
				var abilities = await _mediator.Send(new GetAbilityQuery() { PageSize = int.MaxValue }, cancellationToken);

				var result =  abilities.ToDictionary(x => x.Id, x => x.Name);

				_memoryCache.Set("abilities", result);
				return result;
			}	
		}

		/// <summary>
		/// Создание/извлечение из кэша данных о шаблонах тела
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private async Task<Dictionary<Guid, string>> GetBodyTemplateListForViewModel(CancellationToken cancellationToken)
		{
			if (_memoryCache.TryGetValue("bodyTemplates", out Dictionary<Guid, string> bodyTemplatesFromCache))
				return bodyTemplatesFromCache;
			else
			{
				var bodyTemplates = await _mediator.SendValidated(new GetBodyTemplateQuery() { PageSize = int.MaxValue }, cancellationToken);

				var result = bodyTemplates.ToDictionary(x => x.Id, x => x.Name);

				_memoryCache.Set("bodyTemplates", result);
				return result;
			}
		}
	}
}
