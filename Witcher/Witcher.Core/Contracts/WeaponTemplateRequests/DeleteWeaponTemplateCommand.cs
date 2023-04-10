using System;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Contracts.WeaponTemplateRequests
{
	public class DeleteWeaponTemplateCommand : IValidatableCommand
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
