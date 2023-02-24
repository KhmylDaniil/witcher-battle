using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.UserRequests.LoginUser;
using Sindie.ApiService.Core.Contracts.UserRequests.RegisterUser;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.ExtensionMethods;
using System.Diagnostics;
using Witcher.MVC.Models;
using Witcher.MVC.ViewModels.Login;

namespace Witcher.MVC.Controllers
{
	public class LoginController : BaseController
	{
		public LoginController(IMediator mediator, IGameIdService gameIdService) : base(mediator, gameIdService) { }

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

				await _mediator.SendValidated(new LoginUserCommand() { Login = request.Login, Password = request.Password }, cancellationToken);

				return RedirectToAction(nameof(SuccessfulRegistration), new SuccessfulRegistration() { Name = request.Name });
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
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

				return RedirectToAction("Index", "Home");
			}
			catch (RequestValidationException ex)
			{
				ViewData["ErrorMessage"] = ex.UserMessage;
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