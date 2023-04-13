using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Contracts.BagRequests
{
	public class RemoveItemFromBagCommand : IValidatableCommand
	{
		public Guid CharacterId { get; set; }

		public Guid ItemId { get; set; }

		public int Quantity { get; set; } = 1;

		public void Validate()
		{
			if (Quantity < 1)
				throw new RequestFieldIncorrectDataException<AddItemToBagCommand>(nameof(Quantity), "Значение должно быть больше нуля");
		}
	}
}
