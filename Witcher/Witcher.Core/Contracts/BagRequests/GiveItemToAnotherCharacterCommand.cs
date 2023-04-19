using System;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.Notifications;

namespace Witcher.Core.Contracts.BagRequests
{
	public class GiveItemToAnotherCharacterCommand : IValidatableCommand<GiveItemNotification>
	{
		public Guid CharacterId { get; set; }

		public Guid TargetCharacterId { get; set; }

		public Guid ItemId { get; set; }

		public int Quantity { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
