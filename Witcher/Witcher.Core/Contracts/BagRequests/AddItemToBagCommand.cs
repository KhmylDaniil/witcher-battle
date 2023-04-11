using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Contracts.BagRequests
{
	public class AddItemToBagCommand : IValidatableCommand
	{
		public Guid CharacterId { get; set; }

		public Guid ItemTemplateId { get; set; }

		public ushort Quantity { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
