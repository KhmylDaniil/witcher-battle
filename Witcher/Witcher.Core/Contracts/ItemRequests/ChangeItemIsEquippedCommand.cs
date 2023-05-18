using System;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Contracts.ItemRequests
{
	public class ChangeItemIsEquippedCommand : IValidatableCommand
	{
		public Guid CharacterId { get; set; }

		public Guid ItemId { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
