using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.ChangeCreatureTemplate
{
	/// <summary>
	/// Элемент запроса изменения шаблона существа - применяемое состояние
	/// </summary>
	public class ChangeCreatureTemplateRequestAppliedCondition
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
