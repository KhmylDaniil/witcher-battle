using MediatR;
using System;
using Witcher.Core.Contracts.Notifications;

namespace Witcher.Core.Contracts.ItemRequests
{
	public sealed class GiveItemToAnotherCharacterCommand : IRequest<GiveItemNotification>
	{
		public Guid CharacterId { get; set; }

		public Guid TargetCharacterId { get; set; }

		public Guid ItemId { get; set; }

		public int Quantity { get; set; }
	}
}
