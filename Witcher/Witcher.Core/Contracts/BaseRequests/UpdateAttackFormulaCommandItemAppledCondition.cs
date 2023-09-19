using Witcher.Core.BaseData;
using System;

namespace Witcher.Core.Contracts.BaseRequests
{
	/// <summary>
	/// Элемент запроса создания или изменения способности(накладываемое состояние) 
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
		public int ApplyChance { get; set; } = 25;
	}
}
