using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbility;
using Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbilityById;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.ExtensionMethods;
using Witcher.MVC.ViewModels.CreatureTemplate;

namespace Witcher.MVC.Controllers
{
	[Authorize]
	public class CreatureTemplateController : BaseController
	{

		public CreatureTemplateController(IMediator mediator, IGameIdService gameIdService) : base(mediator, gameIdService) { }

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
				ViewData["ErrorMessage"] = ex.UserMessage;

				response = await _mediator.SendValidated(new GetCreatureTemplateQuery() , cancellationToken);
			}
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
				ViewData["ErrorMessage"] = ex.UserMessage;

				var response = await _mediator.SendValidated(new GetCreatureTemplateQuery(), cancellationToken);

				return View(response);
			}
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
				return RedirectToAction(nameof(Details), new GetCreatureTemplateByIdQuery() { Id = result.Id });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
			}
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
				return RedirectToAction(nameof(Details), new GetCreatureTemplateByIdQuery() { Id = command.Id });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
			}
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
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
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
				return RedirectToAction(nameof(Index), new GetCreatureTemplateQuery());
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
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
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

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
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		[Route("[controller]/[action]/{creatureTemplateId}")]
		public ActionResult EditDamageTypeModifier(ChangeDamageTypeModifierCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{creatureTemplateId}")]
		public async Task<IActionResult> EditDamageTypeModifier(ChangeDamageTypeModifierCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Details), new GetCreatureTemplateByIdQuery() { Id = command.CreatureTemplateId });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		/// <summary>
		/// Создание модели представления для создания шаблона существа
		/// </summary>
		async Task<CreateCreatureTemplateCommandViewModel> CreateVM(CreateCreatureTemplateCommand command, CancellationToken cancellationToken)
		{
			var viewModel = command is CreateCreatureTemplateCommandViewModel vm
				? vm
				: new CreateCreatureTemplateCommandViewModel()
				{
					ImgFileId = command.ImgFileId,
					BodyTemplateId = command.BodyTemplateId,
					CreatureType = command.CreatureType,
					Name = command.Name,
					Description = command.Description,
					HP = command.HP,
					Sta = command.Sta,
					Int = command.Int,
					Ref = command.Ref,
					Dex = command.Dex,
					Body = command.Body,
					Emp = command.Emp,
					Cra = command.Cra,
					Will = command.Will,
					Speed = command.Speed,
					Luck = command.Luck,
					ArmorList = command.ArmorList,
					Abilities = command.Abilities,
					CreatureTemplateSkills = command.CreatureTemplateSkills,
				};

			var bodyTemplates = await _mediator.SendValidated(new GetBodyTemplateQuery(), cancellationToken);

			viewModel.BodyTemplatesDictionary = bodyTemplates.ToDictionary(x => x.Id, x => x.Name);

			var abilities = await _mediator.SendValidated(new GetAbilityQuery(), cancellationToken);

			viewModel.AbilitiesDictionary = abilities.ToDictionary(x => x.Id, x => x.Name);

			return viewModel;
		}

		/// <summary>
		/// Создание модели представления для изменения шаблона существа
		/// </summary>
		async Task<ChangeCreatureTemplateCommandViewModel> CreateVM(ChangeCreatureTemplateCommand command, CancellationToken cancellationToken)
		{
			var viewModel = command is ChangeCreatureTemplateCommandViewModel vm
				? vm
				: new ChangeCreatureTemplateCommandViewModel()
				{
					ImgFileId = command.ImgFileId,
					BodyTemplateId = command.BodyTemplateId,
					CreatureType = command.CreatureType,
					Name = command.Name,
					Description = command.Description,
					HP = command.HP,
					Sta = command.Sta,
					Int = command.Int,
					Ref = command.Ref,
					Dex = command.Dex,
					Body = command.Body,
					Emp = command.Emp,
					Cra = command.Cra,
					Will = command.Will,
					Speed = command.Speed,
					Luck = command.Luck,
					ArmorList = command.ArmorList,
					Abilities = command.Abilities,
					CreatureTemplateSkills = command.CreatureTemplateSkills,
				};

			var bodyTemplates = await _mediator.SendValidated(new GetBodyTemplateQuery(), cancellationToken);

			viewModel.BodyTemplatesDictionary = bodyTemplates.ToDictionary(x => x.Id, x => x.Name);

			var abilities = await _mediator.SendValidated(new GetAbilityQuery(), cancellationToken);

			viewModel.AbilitiesDictionary = abilities.ToDictionary(x => x.Id, x => x.Name);

			return viewModel;
		}
	}
}
