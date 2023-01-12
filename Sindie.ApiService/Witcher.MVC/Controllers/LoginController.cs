using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.UserRequests.LoginUser;
using Sindie.ApiService.Core.Contracts.UserRequests.RegisterUser;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.Requests.UserRequests.RegisterUser;
using System;
using System.Diagnostics;
using System.Security.Authentication;
using System.Security.Claims;
using Witcher.MVC.Areas.Login.ViewModels;
using Witcher.MVC.Models;


namespace Witcher.MVC.Controllers
{
	public class LoginController : Controller
	{
		private readonly ILogger<LoginController> _logger;

		private readonly IMediator _mediator;

		private readonly IAppDbContext _appDbContext;

		private readonly IPasswordHasher _passwordHasher;

		private readonly IHttpContextAccessor _httpContextAccessor;

		public LoginController(ILogger<LoginController> logger, IMediator mediator, IAppDbContext appDbContext, IPasswordHasher passwordHasher, IHttpContextAccessor httpContextAccessor)
		{
			_logger = logger;
			_mediator = mediator;
			_appDbContext = appDbContext;
			_passwordHasher = passwordHasher;
			_httpContextAccessor = httpContextAccessor;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult RegisterUser()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(
				request == null
				? new RegisterUserCommand()
				: new RegisterUserCommand(request)
				{
					Name = request.Name,
					Email = request.Email,
					Phone = request.Phone,
					Login = request.Login,
					Password = request.Password
				},
				cancellationToken);

				return RedirectToAction(nameof(SuccessfulRegistration), new SuccessfulRegistration() { Name = request!.Name });
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View();
			}
		}

		public IActionResult SuccessfulRegistration(SuccessfulRegistration registration)
		{
			return View(registration);
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginUserCommand request, CancellationToken cancellationToken)
		{
			try
			{
				//await _mediator.Send(request);

				if (request == null)
					throw new ArgumentNullException($"Пришел пустой запрос {typeof(LoginUserCommand)}");
				if (string.IsNullOrWhiteSpace(request.Password))
					throw new ExceptionRequestFieldIncorrectData<LoginUserCommand>(nameof(request.Password));
				if (string.IsNullOrWhiteSpace(request.Login))
					throw new ExceptionRequestFieldIncorrectData<LoginUserCommand>(nameof(request.Login));

				var existingUserAccount = await _appDbContext.UserAccounts
					.Include(x => x.User)
						.ThenInclude(x => x.UserRoles)
						.ThenInclude(x => x.SystemRole)
					.FirstOrDefaultAsync(x => x.Login == request.Login, cancellationToken);

				if (existingUserAccount == null)
					throw new AuthenticationException("Не верный логин");

				bool isPasswordCorrect = _passwordHasher.VerifyHash
					(request.Password, existingUserAccount.PasswordHash);

				if (!isPasswordCorrect)
					throw new AuthenticationException("Не верный пароль");


				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, existingUserAccount.UserId.ToString()),
					new Claim(ClaimTypes.Role, existingUserAccount.User.UserRoles.FirstOrDefault().ToString())
				};
				// создаем объект ClaimsIdentity
				ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
				// установка аутентификационных куки
				await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View();
			}
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}