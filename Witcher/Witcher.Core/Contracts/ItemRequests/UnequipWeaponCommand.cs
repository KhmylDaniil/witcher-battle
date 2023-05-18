using System;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Contracts.ItemRequests
{
	public class UnequipWeaponCommand : IValidatableCommand
	{
		public Guid CharacterId { get; set; }

		public Guid WeaponId { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
