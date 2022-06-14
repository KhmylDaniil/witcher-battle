using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplateById
{
	/// <summary>
	/// Элемент ответа на запрос шаблона существа по айди - применяемое состояние
	/// </summary>
	public class GetCreatureTemplateByIdResponseAppliedCondition
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Айди состояния
		/// </summary>
		public Guid ConditionId { get; set; }

		/// <summary>
		/// Название состояния
		/// </summary>
		public string ConditionName { get; set; }

		/// <summary>
		/// Шанс применения
		/// </summary>
		public double ApplyChance { get; set; }
	}
}
