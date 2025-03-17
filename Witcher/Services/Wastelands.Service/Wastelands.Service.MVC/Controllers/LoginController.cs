using Microsoft.AspNetCore.Mvc;
using Wastelands.Service.Domain.Contracts;
using Wastelands.Service.Domain.Models.Requests;
using Wastelands.Service.MVC.Attributes;

namespace Wastelands.Service.MVC.Controllers
{
	public class LoginController : Controller
	{
		private readonly IUserService _userService;

		public LoginController(IUserService userService)
		{
			_userService = userService;
		}

		public IActionResult Index() => View();

		public IActionResult RegisterUser() => View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Exception("register")]
		public async Task<IActionResult> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken)
		{
			await _userService.RegisterUserAsync(request);
			await _userService.LoginUserAsync(new LoginUserRequest { Login = request.Login, Password = request.Password });
			return RedirectToAction("Index", "Home");

		}

		public IActionResult Login() => View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Exception("login")]
		public async Task<IActionResult> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken)
		{
			await _userService.LoginUserAsync(request);
			return RedirectToAction("Index", "Home");
		}
	}
}
