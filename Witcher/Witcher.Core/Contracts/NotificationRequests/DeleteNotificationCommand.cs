using System;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Contracts.NotificationRequests
{
	public class DeleteNotificationCommand : IValidatableCommand
	{
		public Guid Id { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
