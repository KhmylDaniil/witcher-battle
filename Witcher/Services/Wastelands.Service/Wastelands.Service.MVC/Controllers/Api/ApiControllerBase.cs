using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wastelands.Service.MVC.Controllers.Api
{
	/// <summary>
	/// Базовый класс для JSON API контроллеров. Тонкая обёртка вокруг тех же сервисов (ICharacterService,
	/// IUserService), что использует Razor MVC — бизнес-логика переиспользуется как есть.
	/// Исключения маппятся в JSON через <see cref="ApiExceptionFilter"/> вместо Razor View.
	/// </summary>
	// Намеренно без класс-уровневого [Route(...)] — у каждого конкретного контроллера свой маршрут
	// (см. AuthApiController/CharactersApiController); Route на базовом+наследнике даёт два рабочих маршрута.
	[ApiController]
	[Authorize]
	[TypeFilter(typeof(ApiExceptionFilter))]
	public abstract class ApiControllerBase : ControllerBase
	{
	}
}
