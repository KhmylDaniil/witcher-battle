﻿using System;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Модификатор по типу урона для шаблона существа
	/// </summary>
	public class CreatureTemplateDamageTypeModifier : EntityBase 
	{
		/// <summary>
		/// Пустой конструктор для EF
		/// </summary>
		protected CreatureTemplateDamageTypeModifier()
		{
		}

		/// <summary>
		/// Конструктор для для модификаторов по типу урона
		/// </summary>
		/// <param name="primaryEntityid"></param>
		/// <param name="damageType"></param>
		/// <param name="modifier"></param>
		public CreatureTemplateDamageTypeModifier(Guid primaryEntityid, DamageType damageType, DamageTypeModifier modifier)
		{
			PrimaryEntityid = primaryEntityid;
			DamageType = damageType;
			DamageTypeModifier = modifier;
		}

		/// <summary>
		/// Тип урона
		/// </summary>
		public DamageType DamageType { get; set; }

		/// <summary>
		/// Модификатор урона
		/// </summary>
		public DamageTypeModifier DamageTypeModifier { get; set; }

		/// <summary>
		/// Айди первичной сущности
		/// </summary>
		public Guid PrimaryEntityid { get; set; }
	}

	/// <summary>
	/// Модификатор по типу урона для существа
	/// </summary>
	public class CreatureDamageTypeModifier : CreatureTemplateDamageTypeModifier
	{
	}
}
