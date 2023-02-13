using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using System;
using System.ComponentModel.DataAnnotations;

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
		[Range(1, 100)]
		public int ApplyChance { get; set; }
	}
}
