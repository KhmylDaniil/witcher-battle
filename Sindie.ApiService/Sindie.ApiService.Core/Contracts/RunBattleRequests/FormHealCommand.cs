using Witcher.Core.Abstractions;
using System;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	public class FormHealCommand : IValidatableCommand<FormHealResponse>
	{
		public Guid TargetCreatureId { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
