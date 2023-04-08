using Witcher.Core.Abstractions;
using System;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	public class FormAttackCommand : IValidatableCommand<FormAttackResponse>
	{
		public Guid AttackerId { get; set; }

		public Guid? AbilityId { get; set; }

		public Guid TargetId { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
