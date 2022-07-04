using System;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.ChangeAbility
{
	/// <summary>
	/// Элемент запроса изменения способности - применяемое состояние
	/// </summary>
	public class ChangeAbilityRequestAppliedCondition
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid? Id { get; set; }
		
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
