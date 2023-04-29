﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.NotificationRequests;
using Witcher.Core.Exceptions;
using Witcher.Core.ExtensionMethods;
using Witcher.Core.Notifications;

namespace Witcher.MVC.Controllers
{
	public class NotificationController : BaseController
	{
		public NotificationController(IMediator mediator, IGameIdService gameIdService) : base(mediator, gameIdService)
		{
		}

		[Route("[controller]")]
		public async Task<IActionResult> Index(GetNotificationsQuery query, CancellationToken cancellationToken)
		{
			IEnumerable<Notification> response;
			try
			{
				response = await _mediator.SendValidated(query, cancellationToken);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;

				response = await _mediator.SendValidated(new GetNotificationsQuery(), cancellationToken);
			}
			catch (Exception ex) { return RedirectToErrorPage<NotificationController>(ex); }

			return View(response);
		}

		[Route("[controller]/{id}")]
		public async Task<IActionResult> Details(GetNotificationByIdQuery query, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.SendValidated(query, cancellationToken);

				return View(response);
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;

				var response = await _mediator.SendValidated(new GetNotificationsQuery(), cancellationToken);

				return View(response);
			}
			catch (Exception ex) { return RedirectToErrorPage<NotificationController>(ex); }
		}

		[Route("[controller]/[action]/{id}")]
		public ActionResult Delete(DeleteNotificationCommand command) => View(command);

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Delete(DeleteNotificationCommand command, CancellationToken cancellationToken)
		{
			try
			{
				await _mediator.SendValidated(command, cancellationToken);

				return RedirectToAction(nameof(Index));
			}
			catch (RequestValidationException ex)
			{
				TempData["ErrorMessage"] = ex.UserMessage;
				return View(command);
			}
			catch (Exception ex) { return RedirectToErrorPage<NotificationController>(ex); }
		}
	}
}
