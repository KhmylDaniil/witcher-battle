using System;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Contracts.ArmorTemplateRequests
{
	public class DeleteArmorTemplateCommand : IValidatableCommand
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
