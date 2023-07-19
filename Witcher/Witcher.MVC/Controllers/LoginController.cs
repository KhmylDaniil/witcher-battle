using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.UserRequests;

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
				await _mediator.Send(request, cancellationToken);

				_memoryCache.Remove("users");

				await _mediator.Send(new LoginUserCommand() { Login = request.Login, Password = request.Password }, cancellationToken);

				return RedirectToAction("Index", "Game");
			}
			catch (Exception ex)
			{
				return HandleException<LoginController>(ex, () => View());
			}
		}

		public IActionResult Login() => View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginUserCommand request, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.Send(request, cancellationToken);

				return RedirectToAction("Index", "Game");
			}
			catch (Exception ex)
			{
				return HandleException<LoginController>(ex, () => View());
			}
		}
	}
}