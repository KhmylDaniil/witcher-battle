using Sindie.ApiService.Core.BaseData;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.CreateAbility
{
	/// <summary>
	/// Элемент запроса создания способности - применяемое состояние
	/// </summary>
	public sealed class CreateAbilityRequestAppliedCondition
	{
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
