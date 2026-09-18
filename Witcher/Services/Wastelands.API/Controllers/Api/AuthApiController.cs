using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.API.Controllers.Api
{
	/// <summary>
	/// Аутентификация/регистрация для SPA. Переиспользует существующую cookie-схему — UserService сам вызывает
	/// httpContext.SignInAsync внутри LoginUserAsync, контроллеру достаточно отправить запрос.
	/// Зеркалит Wastelands.API/Controllers/LoginController.cs, но возвращает JSON вместо Razor View.
	/// </summary>
	[Route("api/auth")]
	public class AuthApiController : ApiControllerBase
	{
		private readonly IUserService _userService;
		private readonly IUserContext _userContext;

		public AuthApiController(IUserService userService, IUserContext userContext)
		{
			_userService = userService;
			_userContext = userContext;
		}

		/// <summary>Регистрация нового пользователя + автоматический вход (как в LoginController.RegisterUserAsync)</summary>
		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken cancellationToken)
		{
			await _userService.RegisterUserAsync(request);
			await _userService.LoginUserAsync(new LoginUserRequest { Login = request.Login, Password = request.Password });
			return Ok();
		}

		/// <summary>Вход по логину/паролю</summary>
		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginUserRequest request, CancellationToken cancellationToken)
		{
			await _userService.LoginUserAsync(request);
			return Ok();
		}

		/// <summary>Выход — сброс аутентификационной cookie</summary>
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return NoContent();
		}

		/// <summary>Текущий пользователь — используется SPA при загрузке для восстановления сессии</summary>
		[HttpGet("me")]
		public IActionResult Me() => Ok(new { userId = _userContext.CurrentUserId });
	}
}
