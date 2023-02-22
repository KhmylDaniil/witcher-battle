using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Witcher.MVC.Controllers
{
	/// <summary>
	/// Базовый класс для контроллера
	/// </summary>
	public abstract class BaseController : Controller
 	{
		/// <summary>
		/// Медиатор
		/// </summary>
		protected readonly IMediator _mediator;

		/// <summary>
		/// Базовый конструктор
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		protected BaseController(IMediator mediator)
		{
			_mediator = mediator;
		}
	}
}
