using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.ArmorTemplateRequests;
using Witcher.Core.Contracts.BodyTemplateRequests;
using Witcher.Core.Exceptions;
using Witcher.Core.ExtensionMethods;
using Witcher.MVC.ViewModels.ArmorTemplate;

namespace Witcher.MVC.Controllers
{
	[Authorize]
	public class ArmorTemplateController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IMemoryCache _memoryCache;

		public ArmorTemplateController(IMediator mediator, IGameIdService gameIdService, IMapper mapper, IMemoryCache memoryCache)
			: base(mediator, gameIdService)
		{
			_mapper = mapper;
			_memoryCache = memoryCache;
		}

		[Route("[controller]/[action]")]
		public async Task<IActionResult> Index(GetArmorTemplateQuery request, CancellationToken cancellationToken)
		{
			ViewData["GameId"] = _gameIdService.GameId;

			try
			{
				var response = await _mediator.SendValidated(request, cancellationToken);

				return View(response);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;

				return View(await _mediator.SendValidated(new GetArmorTemplateQuery(), cancellationToken));
			}
			catch (Exception ex) { return RedirectToErrorPage<ArmorTemplateController>(ex); }
		}

		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Details(GetArmorTemplateByIdQuery query, CancellationToken cancellationToken)
		{
			try
			{
				return View(await _mediator.SendValidated(query, cancellationToken));
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;

				return View(await _mediator.SendValidated(new GetArmorTemplateQuery(), cancellationToken));
			}
			catch (Exception ex) { return RedirectToErrorPage<ArmorTemplateController>(ex); }
		}

		[HttpGet]
		[Route("[controller]/[action]")]
		public async Task<IActionResult> Create(CreateArmorTemplateViewModel viewModel, CancellationToken cancellationToken)
		{
			return View(await CreateVM(viewModel, cancellationToken));
		}

		[Route("[controller]/[action]")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateArmorTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				var id = await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Details), new GetArmorTemplateByIdQuery() { Id = id });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
			catch (Exception ex) { return RedirectToErrorPage<ArmorTemplateController>(ex); }
		}

		[Route("[controller]/[action]/{id}")]
		public ActionResult Edit(ChangeArmorTemplateCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Edit(ChangeArmorTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Details), new GetArmorTemplateByIdQuery() { Id = command.Id });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
			catch (Exception ex) { return RedirectToErrorPage<ArmorTemplateController>(ex); }
		}

		[Route("[controller]/[action]/{armorTemplateId}")]
		public ActionResult EditDamageTypeModifier(ChangeDamageTypeModifierForArmorTemplateCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{armorTemplateId}")]
		public async Task<IActionResult> EditDamageTypeModifier(ChangeDamageTypeModifierForArmorTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetArmorTemplateByIdQuery() { Id = command.ArmorTemplateId });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
			catch (Exception ex) { return RedirectToErrorPage<ArmorTemplateController>(ex); }
		}

		[Route("[controller]/[action]/{id}")]
		public ActionResult Delete(DeleteArmorTemplateCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Delete(DeleteArmorTemplateCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Index), new GetArmorTemplateQuery());
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
			catch (Exception ex) { return RedirectToErrorPage<ArmorTemplateController>(ex); }
		}

		/// <summary>
		/// Создание модели представления для создания шаблона брони
		/// </summary>
		async Task<CreateArmorTemplateViewModel> CreateVM(CreateArmorTemplateCommand command, CancellationToken cancellationToken)
		{
			var viewModel = command is CreateArmorTemplateViewModel vm
				? vm
				: _mapper.Map<CreateArmorTemplateViewModel>(command);

			viewModel.BodyTemplatesDictionary = await GetBodyTemplateListForViewModel(cancellationToken);

			return viewModel;
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
