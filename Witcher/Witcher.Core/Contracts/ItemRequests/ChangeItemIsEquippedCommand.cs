using MediatR;
using System;

namespace Witcher.Core.Contracts.ItemRequests
{
	public sealed class ChangeItemIsEquippedCommand : IRequest<Unit>
	{
		public Guid CharacterId { get; set; }

		public Guid ItemId { get; set; }
	}
}
