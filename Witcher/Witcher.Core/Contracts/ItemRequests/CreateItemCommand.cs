using MediatR;
using System;

namespace Witcher.Core.Contracts.ItemRequests
{
	public class CreateItemCommand : IRequest
	{
		public Guid CharacterId { get; set; }

		public Guid ItemTemplateId { get; set; }

		public int Quantity { get; set; } = 1;
	}
}
