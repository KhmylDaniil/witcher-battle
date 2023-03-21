﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Witcher.Core.Abstractions;
using Witcher.MVC.ViewModels;

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
		/// Айди игры для роутов
		/// </summary>
		protected readonly IGameIdService _gameIdService;

		/// <summary>
		/// Базовый конструктор
		/// </summary>
		/// <param name="mediator">Медиатор</param>
		protected BaseController(IMediator mediator, IGameIdService gameIdService)
		{
			_mediator = mediator;
			_gameIdService = gameIdService;
		}

		protected ActionResult RedirectToErrorPage<TController>(Exception ex) where TController : BaseController
		{
			var myLog = Log.ForContext<TController>();
			myLog.Error(ex.Message);
			return View("Error", new ErrorViewModel(ex));
		}
			
	}
}
