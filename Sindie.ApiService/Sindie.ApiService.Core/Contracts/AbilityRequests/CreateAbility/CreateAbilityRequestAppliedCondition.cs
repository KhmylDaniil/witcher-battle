using System;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.CreateAbility
{
	/// <summary>
	/// Элемент запроса создания способности - применяемое состояние
	/// </summary>
	public sealed class CreateAbilityRequestAppliedCondition
	{
		/// <summary>
		/// Айди состояния
		/// </summary>
		public Guid ConditionId { get; set; }

		/// <summary>
		/// Шанс применения
		/// </summary>
		public int ApplyChance { get; set; }
	}
}
