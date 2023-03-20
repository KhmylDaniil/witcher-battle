using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.UserRequests.LoginUser;
using Witcher.Core.Contracts.UserRequests.RegisterUser;
using Witcher.Core.Exceptions;
using Witcher.Core.ExtensionMethods;
using Witcher.MVC.ViewModels.Login;

namespace Witcher.MVC.Controllers
{
	public class LoginController : BaseController
	{
		private readonly IMemoryCache _memoryCache;

		public LoginController(IMediator mediator, IGameIdService gameIdService, IMemoryCache memoryCache)
			: base(mediator, gameIdService)
		{
			_memoryCache = memoryCache;
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
		public async Task<IActionResult> RegisterUser(RegisterUserCommand request, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(request ?? throw new ArgumentNullException(nameof(request)), cancellationToken);

				_memoryCache.Remove("users");

				await _mediator.SendValidated(new LoginUserCommand() { Login = request.Login, Password = request.Password }, cancellationToken);

				return RedirectToAction(nameof(SuccessfulRegistration), new SuccessfulRegistration() { Name = request.Name });
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
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
				await _mediator.SendValidated(request ?? throw new ArgumentNullException(nameof(request)), cancellationToken);

				return RedirectToAction("Index", "Game");
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View();
			}
		}
	}
}