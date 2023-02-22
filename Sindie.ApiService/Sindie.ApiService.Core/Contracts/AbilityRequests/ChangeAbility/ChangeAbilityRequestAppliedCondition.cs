using Sindie.ApiService.Core.BaseData;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.ChangeAbility
{
	/// <summary>
	/// Элемент запроса изменения способности - применяемое состояние
	/// </summary>
	public sealed class ChangeAbilityRequestAppliedCondition
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
