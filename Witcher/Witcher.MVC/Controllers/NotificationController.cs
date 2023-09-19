using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.NotificationRequests;
using Witcher.Core.Exceptions;
using Witcher.Core.ExtensionMethods;
using Witcher.Core.Notifications;

namespace Witcher.MVC.Controllers
{
	[Authorize]
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
				response = await _mediator.Send(query, cancellationToken);
				return View(response);

			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync<NotificationController>(ex, async ()
					=> View(await _mediator.Send(new GetNotificationsQuery(), cancellationToken)));
			}
		}

		[Route("[controller]/{id}")]
		public async Task<IActionResult> Details(GetNotificationByIdQuery query, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _mediator.Send(query, cancellationToken);

				return View(response);
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync<NotificationController>(ex, async ()
					=> View(await _mediator.Send(new GetNotificationsQuery(), cancellationToken)));
			}
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
				await _mediator.Send(command, cancellationToken);

				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex) { return HandleException<NotificationController>(ex, () => View(command)); }
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("[controller]/[action]/{id}")]
		public async Task<IActionResult> Decide(Guid id, bool accept, CancellationToken cancellationToken)
		{
			var command = await FormDecisionFromNotification(id, accept);

			try
			{
				await _mediator.Send(command, cancellationToken);

				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex) { return HandleException<NotificationController>(ex, () => RedirectToAction(nameof(Details), new { Id = id })); }
		}

		private async Task<IRequest> FormDecisionFromNotification(Guid id, bool accept)
		{
			var notification = await _mediator.Send(new GetNotificationByIdQuery { Id = id }) as YesOrNoDecisionNotification
				?? throw new EntityBaseException("Уведомление не найдено.");

			return accept ? notification.Accept() : notification.Decline();
		}
	}
}
