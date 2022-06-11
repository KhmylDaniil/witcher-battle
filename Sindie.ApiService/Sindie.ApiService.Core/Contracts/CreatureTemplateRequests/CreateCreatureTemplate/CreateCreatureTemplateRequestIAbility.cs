using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.CreateCreatureTemplate
{
	/// <summary>
	/// Элемент запроса создания шаблона существа - способность
	/// </summary>
	public class CreateCreatureTemplateRequestIAbility
	{
		/// <summary>
		/// Наазвание способности
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание способности
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Айди параметра атаки
		/// </summary>
		public Guid AttackParameterId { get; set; }

		/// <summary>
		/// Количество кубов атаки
		/// </summary>
		public int AttackDiceQuantity { get; set; }

		/// <summary>
		/// Модификатор атаки
		/// </summary>
		public int DamageModifier { get; set; }

		/// <summary>
		/// Скорость атаки
		/// </summary>
		public int AttackSpeed { get; set; }

		/// <summary>
		/// Точность атаки
		/// </summary>
		public int Accuracy { get; set; }

		/// <summary>
		/// Накладываемые состояния
		/// </summary>
		public List<CreateCreatureTemplateAppliedCondition> AppliedConditions { get; set; }
	}
}
