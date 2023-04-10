using System;
using System.Collections.Generic;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Contracts.ItemTemplateBase;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.WeaponTemplateRequests
{
	public class GetWeaponTemplateByIdResponse : GetItemByIdResponseBase
	{
		/// <summary>
		/// Навык атаки
		/// </summary>
		public Skill AttackSkill { get; set; }

		/// <summary>
		/// Тип урона
		/// </summary>
		public DamageType DamageType { get; set; }

		/// <summary>
		/// Количество кубов атаки
		/// </summary>
		public int AttackDiceQuantity { get; set; }

		/// <summary>
		/// Модификатор атаки
		/// </summary>
		public int DamageModifier { get; set; }

		/// <summary>
		/// Прочность
		/// </summary>
		public int Durability { get; set; }

		/// <summary>
		/// Точность атаки
		/// </summary>
		public int Accuracy { get; set; }

		/// <summary>
		/// Дальность
		/// </summary>
		public int? Range { get; set; }

		/// <summary>
		/// Накладываемые состояния
		/// </summary>
		public List<UpdateAttackFormulaCommandItemAppledCondition> AppliedConditions { get; set; }
	}
}