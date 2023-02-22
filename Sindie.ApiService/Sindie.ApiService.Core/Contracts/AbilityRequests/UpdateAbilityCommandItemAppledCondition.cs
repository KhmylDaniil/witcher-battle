using Sindie.ApiService.Core.BaseData;
using System;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Элемент запроса созадния или изменения способности(накладываемое состояние) 
	/// </summary>
	public class UpdateAbilityCommandItemAppledCondition
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
