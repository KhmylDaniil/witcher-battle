﻿using System;
using System.Collections.Generic;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Элемент ответа на запрос шаблона существа по айди - способность
	/// </summary>
	public sealed class GetCreatureTemplateByIdResponseAbility
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Наазвание способности
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание способности
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Навык атаки
		/// </summary>
		public Skill AttackSkill { get; set; }

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
		public List<GetCreatureTemplateByIdResponseAppliedCondition> AppliedConditions { get; set; }
	}
}
