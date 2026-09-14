using MediatR;

namespace Witcher.Core.Notifications
{
	public abstract class YesOrNoDecisionNotification : Notification
	{
		public abstract IRequest<Unit> Accept();

		public abstract IRequest<Unit> Decline();
	}
}
