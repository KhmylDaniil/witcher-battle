using Witcher.Core.BaseData;
using System;

namespace Witcher.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Элемент запроса созадния или изменения способности(накладываемое состояние) 
	/// </summary>
	public class UpdateAttackFormulaCommandItemAppledCondition
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid? Id { get; set; }

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
