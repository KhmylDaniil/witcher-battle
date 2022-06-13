using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.CreateCreatureTemplate
{
	/// <summary>
	/// Элемент запроса создания шаблона существа - применяемое состояние
	/// </summary>
	public class CreateCreatureTemplateRequestAppliedCondition
	{
		/// <summary>
		/// Айди состояния
		/// </summary>
		public Guid ConditionId { get; set; }

		/// <summary>
		/// Шанс применения
		/// </summary>
		public double ApplyChance { get; set; }
	}
}
