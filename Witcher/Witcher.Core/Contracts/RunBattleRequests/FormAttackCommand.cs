using Witcher.Core.Abstractions;
using System;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	public class FormAttackCommand : IValidatableCommand<FormAttackResponse>
	{
		public Guid AttackerId { get; set; }

		public Guid AttackFormulaId { get; set; }

		public Guid TargetId { get; set; }

		public AttackType AttackType { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
