using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.GameRequests;
using Witcher.Core.Contracts.UserGameRequests;
using Witcher.Core.Contracts.UserRequests;
using Witcher.MVC.ViewModels.Game;

namespace Witcher.MVC.Controllers
{
	[Authorize]
	public class GameController : BaseController
	{
		private readonly IMemoryCache _memoryCache;
		private readonly IUserContext _userContext;
		private readonly IMapper _mapper;

		public GameController(IMediator mediator, IGameIdService gameIdService, IMemoryCache memoryCache,
			IUserContext userContext, IMapper mapper)
			: base(mediator, gameIdService)
		{
			_memoryCache = memoryCache;
			_userContext = userContext;
			_mapper = mapper;
		}

		/// <summary>
		/// Get games query with search filters
		/// </summary>
		/// <param name="query"></param>
		/// <param name="cancellationToken"></param>
		/// <returns>games</returns>
		public async Task<IActionResult> Index(GetGameQuery query, CancellationToken cancellationToken)
		{
			var response = await _mediator.Send(query, cancellationToken);

			_gameIdService.Reset();

			ViewData["currentUser"] = _userContext.CurrentUserId;

			return View(response);
		}

		/// <summary>
		/// enter the game menu
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

				var response = await _mediator.Send(command, cancellationToken);

				ViewData["currentUser"] = _userContext.CurrentUserId;

				return View(response);
			}
			catch (Exception ex)
			{
				return HandleException<GameController>(ex, () =>
				{
					_gameIdService.Reset();
					return RedirectToAction(nameof(Index));
				});
			}
		}

		/// <summary>
		/// Request to GM for joining game
		/// </summary>
		/// <param name="request">request</param>
		/// <returns></returns>
		[Route("[controller]/[action]")]
		public ActionResult AskForJoin(JoinGameRequest request) => View(request);

		/// <summary>
		/// Request to GM for joining game
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]")]
		public async Task<IActionResult> AskForJoin(JoinGameRequest request, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(request, cancellationToken);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex) { return HandleException<GameController>(ex, () => View(request)); }
		}

		/// <summary>
		/// Создание игры
		/// </summary>
		/// <returns></returns>
		[Route("[controller]/[action]")]
		public ActionResult Create() => View();

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
				await _mediator.Send(command, cancellationToken);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex) { return HandleException<GameController>(ex, () => View(command)); }
		}

		/// <summary>
		/// Изменение игры
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		[Route("[controller]/[action]/{id}")]
		public ActionResult Edit(ChangeGameCommand command) => View(command);

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
				await _mediator.Send(command, cancellationToken);

				return RedirectToAction(nameof(Enter), new GetGameByIdCommand { Id = command.Id } );
			}
			catch(Exception ex) { return HandleException<GameController>(ex, () => View(command)); }
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
				await _mediator.Send(command, cancellationToken);

				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex) { return HandleException<GameController>(ex, () => View(command)); }
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
				await _mediator.Send(command, cancellationToken);
				return InnerRedirect();
			}
			catch (Exception ex) { return HandleException<GameController>(ex, InnerRedirect); }

			ActionResult InnerRedirect() => command.UserId == _userContext.CurrentUserId
					? RedirectToAction(nameof(Index))
					: RedirectToAction(nameof(Enter), new GetGameByIdCommand() { Id = _gameIdService.GameId });
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
				await _mediator.Send(command, cancellationToken);

				return RedirectToAction(nameof(Enter), new GetGameByIdCommand { Id = _gameIdService.GameId });
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync<GameController>(ex, async () =>
				{
					ViewData["GameId"] = _gameIdService.GameId;
					return View(await CreateVM(command, cancellationToken));
				});
			}
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
				await _mediator.Send(command, cancellationToken);
				return RedirectToAction(nameof(Enter), new GetGameByIdCommand { Id = _gameIdService.GameId });
			}
			catch (Exception ex) { return HandleException<GameController>(ex, () =>
				RedirectToAction(nameof(Enter), new GetGameByIdCommand { Id = _gameIdService.GameId })); }
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
				var users = await _mediator.Send(new GetUsersQuery(), cancellationToken);

				var result = users.Where(x => x.Id != _userContext.CurrentUserId).ToDictionary(x => x.Id, x => x.Name);

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
				: _mapper.Map<CreateUserGameCommandViewModel>(command);

			viewModel.UserDictionary = await GetUserListToViewModel(cancellationToken);

			return viewModel;
		}
	}
}
