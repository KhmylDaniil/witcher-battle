using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Witcher.Core.Abstractions;

namespace Witcher.MVC.Controllers.Api
{
	/// <summary>
	/// Базовый класс для JSON API контроллеров. Тонкая обёртка вокруг тех же MediatR-команд/запросов,
	/// что использует Razor MVC — вся бизнес-логика и авторизация переиспользуются как есть.
	/// Исключения маппятся в JSON-ответы через <see cref="ApiExceptionFilter"/> вместо View/Redirect.
	/// </summary>
	// Намеренно без класс-уровневого [Route(...)] здесь — у каждого конкретного контроллера свой маршрут
	// (см. AuthApiController/GamesApiController/...); дублирование Route на базовом+наследнике даёт два
	// одновременно рабочих маршрута на один контроллер.
	[ApiController]
	[Authorize]
	[TypeFilter(typeof(ApiExceptionFilter))]
	public abstract class ApiControllerBase : ControllerBase
	{
		/// <summary>
		/// Медиатор
		/// </summary>
		protected readonly IMediator _mediator;

		/// <summary>
		/// Айди игры для авторизации (per-request scoped — см. Witcher.Core/Entry.cs)
		/// </summary>
		protected readonly IGameIdService _gameIdService;

		protected ApiControllerBase(IMediator mediator, IGameIdService gameIdService)
		{
			_mediator = mediator;
			_gameIdService = gameIdService;
		}
	}
}
