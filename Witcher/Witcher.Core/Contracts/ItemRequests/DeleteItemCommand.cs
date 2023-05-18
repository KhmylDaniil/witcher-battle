using System;
using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Contracts.ItemRequests
{
	public class DeleteItemCommand : IValidatableCommand
	{
		public Guid CharacterId { get; set; }

		public Guid ItemId { get; set; }

		public int Quantity { get; set; } = 1;

		public string Name { get; set; }

		public void Validate()
		{
			if (Quantity < 1)
				throw new RequestFieldIncorrectDataException<CreateItemCommand>(nameof(Quantity), "Значение должно быть больше нуля");
		}
	}
}
