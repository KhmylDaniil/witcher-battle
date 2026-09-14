using MediatR;
using System;
using Witcher.Core.Contracts.BaseRequests;

namespace Witcher.Core.Contracts.WeaponTemplateRequests
{
	public sealed class UpdateAppliedConditionForWeaponTemplateCommand : UpdateAttackFormulaCommandItemAppledCondition, IRequest<Unit>
	{
		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid WeaponTemplateId { get; set; }
	}
}
