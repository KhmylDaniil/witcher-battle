using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.UserRequests;
using Witcher.Core.Exceptions;

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

		public IActionResult Index() => View();

		public IActionResult RegisterUser() => View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RegisterUser(RegisterUserCommand request, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(request ?? throw new ArgumentNullException(nameof(request)), cancellationToken);

				_memoryCache.Remove("users");

				await _mediator.Send(new LoginUserCommand() { Login = request.Login, Password = request.Password }, cancellationToken);

				return RedirectToAction("Index", "Game");
			}
			catch (ValidationException) { return View(); }
			catch (Exception ex) { return RedirectToErrorPage<LoginController>(ex); }
		}

		public IActionResult Login() => View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginUserCommand request, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(request ?? throw new ArgumentNullException(nameof(request)), cancellationToken);

				return RedirectToAction("Index", "Game");
			}
			catch (RequestValidationException) { return View(); }
			catch (Exception ex) { return RedirectToErrorPage<LoginController>(ex); }
		}
	}
}