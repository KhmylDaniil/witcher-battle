using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.UserRequests;

namespace Witcher.MVC.Controllers.Api
{
	/// <summary>
	/// Аутентификация/регистрация для SPA. Переиспользует существующую cookie-схему —
	/// LoginUserHandler сам вызывает httpContext.SignInAsync, контроллеру достаточно отправить команду.
	/// Зеркалит Witcher.MVC/Controllers/LoginController.cs, но возвращает JSON вместо Razor View.
	/// </summary>
	[Route("api/auth")]
	public class AuthApiController : ApiControllerBase
	{
		private readonly IUserContext _userContext;
		private readonly IMemoryCache _memoryCache;

		public AuthApiController(IMediator mediator, IGameIdService gameIdService, IUserContext userContext, IMemoryCache memoryCache)
			: base(mediator, gameIdService)
		{
			_userContext = userContext;
			_memoryCache = memoryCache;
		}

		/// <summary>
		/// Регистрация нового пользователя + автоматический вход (как в LoginController.RegisterUser)
		/// </summary>
		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterUserCommand command, CancellationToken cancellationToken)
		{
			await _mediator.Send(command, cancellationToken);

			_memoryCache.Remove("users");

			var userId = await _mediator.Send(
				new LoginUserCommand { Login = command.Login, Password = command.Password }, cancellationToken);

			return Ok(new { userId });
		}

		/// <summary>
		/// Вход по логину/паролю
		/// </summary>
		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginUserCommand command, CancellationToken cancellationToken)
		{
			var userId = await _mediator.Send(command, cancellationToken);
			return Ok(new { userId });
		}

		/// <summary>
		/// Выход — сброс аутентификационной cookie
		/// </summary>
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return NoContent();
		}

		/// <summary>
		/// Текущий пользователь — используется SPA при загрузке для восстановления сессии
		/// </summary>
		[HttpGet("me")]
		public IActionResult Me() => Ok(new { userId = _userContext.CurrentUserId, role = _userContext.Role });
	}
}
