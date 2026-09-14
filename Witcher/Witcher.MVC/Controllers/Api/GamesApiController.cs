using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.GameRequests;
using Witcher.Core.Contracts.UserGameRequests;
using Witcher.MVC.Controllers.Api.Dto;

namespace Witcher.MVC.Controllers.Api
{
	/// <summary>
	/// Зеркалит Witcher.MVC/Controllers/GameController.cs. Для любого действия над конкретной игрой
	/// сначала выставляется _gameIdService.GameId из маршрута — вся авторизация (AuthorizationService.*Filter)
	/// в Core завязана на это ambient-значение.
	/// </summary>
	[Route("api/games")]
	public class GamesApiController : ApiControllerBase
	{
		private readonly IUserContext _userContext;

		public GamesApiController(IMediator mediator, IGameIdService gameIdService, IUserContext userContext)
			: base(mediator, gameIdService)
		{
			_userContext = userContext;
		}

		/// <summary>Список игр текущего пользователя (с фильтрами)</summary>
		[HttpGet]
		public async Task<IEnumerable<GetGameResponseItem>> Index([FromQuery] GetGameQuery query, CancellationToken cancellationToken)
			=> await _mediator.Send(query, cancellationToken);

		/// <summary>Детали игры — аналог GameController.Enter</summary>
		[HttpGet("{id:guid}")]
		public async Task<GameDetailsDto> Get(Guid id, CancellationToken cancellationToken)
		{
			_gameIdService.Set(id);
			var response = await _mediator.Send(new GetGameByIdCommand { Id = id }, cancellationToken);
			return GameDetailsDto.From(response);
		}

		/// <summary>Создание новой игры (создатель становится MainMaster)</summary>
		[HttpPost]
		public async Task<IActionResult> Create(CreateGameCommand command, CancellationToken cancellationToken)
		{
			await _mediator.Send(command, cancellationToken);
			return Ok();
		}

		/// <summary>Изменение игры</summary>
		[HttpPut("{id:guid}")]
		public async Task<IActionResult> Edit(Guid id, ChangeGameCommand command, CancellationToken cancellationToken)
		{
			_gameIdService.Set(id);
			command.Id = id;
			await _mediator.Send(command, cancellationToken);
			return NoContent();
		}

		/// <summary>Удаление игры</summary>
		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> Delete(Guid id, [FromQuery] string name, CancellationToken cancellationToken)
		{
			_gameIdService.Set(id);
			await _mediator.Send(new DeleteGameCommand { Id = id, Name = name }, cancellationToken);
			return NoContent();
		}

		/// <summary>Запрос ГМ-у на вступление в игру</summary>
		[HttpPost("join")]
		public async Task<IActionResult> AskForJoin(JoinGameRequest request, CancellationToken cancellationToken)
		{
			request.UserId = _userContext.CurrentUserId;
			await _mediator.Send(request, cancellationToken);
			return Ok();
		}

		/// <summary>Добавление пользователя в игру (ГМ добавляет напрямую)</summary>
		[HttpPost("{id:guid}/members")]
		public async Task<IActionResult> AddMember(Guid id, CreateUserGameCommand command, CancellationToken cancellationToken)
		{
			_gameIdService.Set(id);
			await _mediator.Send(command, cancellationToken);
			return Ok();
		}

		/// <summary>Переключение роли участника игры Master &lt;-&gt; Player</summary>
		[HttpPut("{id:guid}/members/{userId:guid}")]
		public async Task<IActionResult> ChangeMemberRole(Guid id, Guid userId, CancellationToken cancellationToken)
		{
			_gameIdService.Set(id);
			await _mediator.Send(new ChangeUserGameCommand { UserId = userId }, cancellationToken);
			return NoContent();
		}

		/// <summary>Удаление участника из игры (или собственный выход)</summary>
		[HttpDelete("{id:guid}/members/{userId:guid}")]
		public async Task<IActionResult> RemoveMember(Guid id, Guid userId, [FromQuery] string name, CancellationToken cancellationToken)
		{
			_gameIdService.Set(id);
			await _mediator.Send(new DeleteUserGameCommand { UserId = userId, Name = name }, cancellationToken);
			return NoContent();
		}
	}
}
