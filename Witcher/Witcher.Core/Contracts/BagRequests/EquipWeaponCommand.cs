using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Contracts.BagRequests
{
	public class EquipWeaponCommand : IValidatableCommand
	{
		public Guid CharacterId { get; set; }

		public Guid WeaponId { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
