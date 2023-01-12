using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Contracts.UserRequests.LoginUser;
using Sindie.ApiService.Core.Contracts.UserRequests.RegisterUser;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.Requests.UserRequests.RegisterUser;
using System.Diagnostics;
using Witcher.MVC.Areas.Login.ViewModels;
using Witcher.MVC.Models;

namespace Witcher.MVC.Controllers
{
	public class LoginController : Controller
	{
		private readonly ILogger<LoginController> _logger;

		private readonly IMediator _mediator;

		public LoginController(ILogger<LoginController> logger, IMediator mediator)
		{
			_logger = logger;
			_mediator = mediator;
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
		public async Task<IActionResult> RegisterUser(RegisterUserRequest request, CancellationToken cancellationToken)
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
				await _mediator.Send(request);

				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View();
			}
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}