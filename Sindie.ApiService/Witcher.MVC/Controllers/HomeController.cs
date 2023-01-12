using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindie.ApiService.Core.Abstractions;
using System.Diagnostics;
using Witcher.MVC.Models;

namespace Witcher.MVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private readonly IUserContext _userContext;
		private readonly IAppDbContext _appDbContext;
		private readonly IMediator _mediator;

		public HomeController(ILogger<HomeController> logger, IMediator mediator, IUserContext userContext, IAppDbContext appDbContext)
		{
			_logger = logger;
			_mediator = mediator;
			_userContext = userContext;
			_appDbContext = appDbContext;
		}

		public IActionResult Index()
		{
			var userName = _appDbContext.Users.FirstOrDefault(x => x.Id == _userContext.CurrentUserId).Name;
			
			ViewData["Name"] = userName;
			return View();
		}

		[Authorize]
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