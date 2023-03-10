using Sindie.ApiService.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Contracts.RunBattleRequests
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
