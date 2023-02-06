using Sindie.ApiService.Core.BaseData;
using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplateById
{
	/// <summary>
	/// Элемент ответа на запрос шаблона существа по айди - применяемое состояние
	/// </summary>
	public sealed class GetCreatureTemplateByIdResponseAppliedCondition
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Тип состояния
		/// </summary>
		public Condition Condition { get; set; }

		/// <summary>
		/// Шанс применения
		/// </summary>
		public int ApplyChance { get; set; }
	}
}
