using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using System.Diagnostics;
using Witcher.MVC.Models;

namespace Witcher.MVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private readonly IAppDbContext _appDbContext;
		private readonly IUserContext _userContext;

		public HomeController(ILogger<HomeController> logger, IAppDbContext appDbContext, IUserContext userContext)
		{
			_logger = logger;
			_appDbContext = appDbContext;
			_userContext = userContext;
		}

		public IActionResult IndexAsync()
		{
			var aa = _userContext.CurrentUserId;
			
			var aaa = _appDbContext.Users.FirstOrDefault();
			
			
			return View();
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