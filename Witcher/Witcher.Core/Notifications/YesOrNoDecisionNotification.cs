using Witcher.Core.Abstractions;

namespace Witcher.Core.Notifications
{
	public abstract class YesOrNoDecisionNotification : Notification
	{
		public abstract IValidatableCommand Accept();

		public abstract IValidatableCommand Decline();
	}
}
