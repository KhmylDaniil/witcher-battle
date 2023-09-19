using MediatR;
using System;

namespace Witcher.Core.Contracts.ItemRequests
{
	public sealed class DeleteItemCommand : IRequest
	{
		public Guid CharacterId { get; set; }

		public Guid ItemId { get; set; }

		public int Quantity { get; set; } = 1;

		public string Name { get; set; }
	}
}
