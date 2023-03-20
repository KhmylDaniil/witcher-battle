using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.GameRequests;
using Witcher.Core.Contracts.UserGameRequests;
using Witcher.Core.Contracts.UserRequests.GetUsers;
using Witcher.Core.Exceptions;
using Witcher.Core.ExtensionMethods;
using Witcher.MVC.ViewModels.Game;

namespace Witcher.MVC.Controllers
{
	[Authorize]
	public class GameController : BaseController
	{
		private readonly IMemoryCache _memoryCache;
		private readonly IUserContext _userContext;

		public GameController(IMediator mediator, IGameIdService gameIdService, IMemoryCache memoryCache, IUserContext userContext)
			: base(mediator, gameIdService)
		{
			_memoryCache = memoryCache;
			_userContext = userContext;
		}

		/// <summary>
		/// Получение списка игр с возможностью поиска
		/// </summary>
		/// <param name="name">Поле поиска по имени</param>
		/// <param name="description">Поле поиска по описанию</param>
		/// <param name="authorName">Поле поиска по имени автора</param>
		/// <param name="cancellationToken"></param>
		/// <returns>Список игр</returns>
		public async Task<IActionResult> Index(GetGameQuery query, CancellationToken cancellationToken)
		{
			var response = await _mediator.SendValidated(query, cancellationToken);

			_gameIdService.Reset();

			return View(response);
		}

		/// <summary>
		/// Вход в игру
		/// </summary>
		/// <param name="command"></param>
		/// <param name="cancellationToken"></param>
		/// <returns>Содержимое игры</returns>
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Enter(GetGameByIdCommand command, CancellationToken cancellationToken)
		{
			try
			{
				_gameIdService.Set(command.Id);

				var response = await _mediator.SendValidated(command, cancellationToken);

				ViewData["currentUser"] = _userContext.CurrentUserId;

				return View(response);
			}
			catch (RequestValidationException ex)
			{
				_gameIdService.Reset();
				TempData["ErrorMessage"] = ex.UserMessage;
				return RedirectToAction(nameof(Index));
			}
		}

		/// <summary>
		/// Создание игры
		/// </summary>
		/// <returns></returns>
		[Route("[controller]/[action]")]
		public ActionResult Create()
		{
			return View();
		}

		/// <summary>
		/// Создание игры
		/// </summary>
		/// <param name="command"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]")]
		public async Task<IActionResult> Create(CreateGameCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
				return RedirectToAction(nameof(Index));
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		/// <summary>
		/// Изменение игры
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		[Route("[controller]/[action]/{id}")]
		public ActionResult Edit(ChangeGameCommand command)
		{
			return View(command);
		}

		/// <summary>
		/// Изменение игры
		/// </summary>
		/// <param name="command"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Edit(ChangeGameCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Enter), new GetGameByIdCommand { Id = command.Id } );
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		/// <summary>
		/// Удаление игры
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		[Route("[controller]/[action]/{id}")]
		public ActionResult Delete(DeleteGameCommand command) => View(command);

		/// <summary>
		/// Удаление игры
		/// </summary>
		/// <param name="command"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Delete(DeleteGameCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Index));
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
		}

		/// <summary>
		/// Удаление пользователя из игры
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		[Route("[controller]/[action]/{userId}")]
		public ActionResult DeleteUserGame(DeleteUserGameCommand command)
		{
			ViewData["currentUser"] = _userContext.CurrentUserId;
			ViewData["GameId"] = _gameIdService.GameId;
			return View(command);
		}

		/// <summary>
		/// Удаление пользователя из игры
		/// </summary>
		/// <param name="command"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{userId}")]
		public async Task<IActionResult> DeleteUserGame(DeleteUserGameCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
			}
			if (command.UserId == _userContext.CurrentUserId)
				return RedirectToAction(nameof(Index));
			else
				return RedirectToAction(nameof(Enter), new GetGameByIdCommand() { Id = _gameIdService.GameId });
		}

		/// <summary>
		/// Добавление пользователя в игру
		/// </summary>
		/// <param name="command"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[Route("[controller]/[action]")]
		public async Task<IActionResult> CreateUserGame(CreateUserGameCommandViewModel command, CancellationToken cancellationToken)
		{
			ViewData["GameId"] = _gameIdService.GameId;
			return View(await CreateVM(command, cancellationToken));
		}

		/// <summary>
		/// Добавление пользователя в игру
		/// </summary>
		/// <param name="command"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]")]
		public async Task<IActionResult> CreateUserGame(CreateUserGameCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Enter), new GetGameByIdCommand { Id = _gameIdService.GameId });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				ViewData["GameId"] = _gameIdService.GameId;
			}
			return View(await CreateVM(command, cancellationToken));
		}

		/// <summary>
		/// Изменение роли пользователя в игре
		/// </summary>
		/// <param name="command"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[Route("[controller]/[action]")]
		public async Task<IActionResult> ChangeUserGame(ChangeUserGameCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
			}
			return RedirectToAction(nameof(Enter), new GetGameByIdCommand { Id = _gameIdService.GameId });
		}

		/// <summary>
		/// Создание/извлечение из кэша данных о пользователях
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private async Task<Dictionary<Guid, string>> GetUserListToViewModel(CancellationToken cancellationToken)
		{
			if (_memoryCache.TryGetValue("users", out Dictionary<Guid, string> usersFromCache))
				return usersFromCache;
			else
			{
				var users = await _mediator.SendValidated(new GetUsersQuery(), cancellationToken);

				var result = users.UsersList.Where(x => x.Id != _userContext.CurrentUserId).ToDictionary(x => x.Id, x => x.Name);

				_memoryCache.Set("users", result);
				return result;
			}
		}

		/// <summary>
		/// Создание модели представления для добавления пользователя в игру
		/// </summary>
		async Task<CreateUserGameCommandViewModel> CreateVM(CreateUserGameCommand command, CancellationToken cancellationToken)
		{
			var viewModel = command is CreateUserGameCommandViewModel vm
				? vm
				: new CreateUserGameCommandViewModel()
				{
					UserId = command.UserId,
					RoleId = command.RoleId
				};

			viewModel.UserDictionary = await GetUserListToViewModel(cancellationToken);

			return viewModel;
		}
	}
}
