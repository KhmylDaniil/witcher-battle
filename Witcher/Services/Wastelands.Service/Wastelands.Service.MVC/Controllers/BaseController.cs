using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wastelands.Service.MVC.Filters;

namespace Wastelands.Service.MVC.Controllers
{
	[Authorize]
	[ServiceFilter<ExceptionFilter>]
	public abstract class BaseController : Controller
	{

	}
}
