using System;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BaseRequests;

namespace Witcher.Core.Contracts.WeaponTemplateRequests
{
	public class UpdateAppliedConditionForWeaponTemplateCommand : UpdateAttackFormulaCommandItemAppledCondition, IValidatableCommand
	{
		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid WeaponTemplateId { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
