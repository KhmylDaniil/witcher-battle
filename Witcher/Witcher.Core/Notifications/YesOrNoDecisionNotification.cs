using MediatR;

namespace Witcher.Core.Notifications
{
	public abstract class YesOrNoDecisionNotification : Notification
	{
		public abstract IRequest Accept();

		public abstract IRequest Decline();
	}
}
