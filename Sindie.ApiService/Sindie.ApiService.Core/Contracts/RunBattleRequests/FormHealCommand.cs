using Sindie.ApiService.Core.Abstractions;
using System;

namespace Sindie.ApiService.Core.Contracts.RunBattleRequests
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
