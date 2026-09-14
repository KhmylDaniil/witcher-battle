using System;
using Witcher.Core.Contracts.BaseRequests;
using MediatR;

namespace Witcher.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Команда создания или изменения накладываемого состояния
	/// </summary>
	public sealed class UpdateAppliedConditionForAbilityCommand : UpdateAttackFormulaCommandItemAppledCondition, IRequest<Unit>
	{
		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid AbilityId { get; set; }
	}
}
