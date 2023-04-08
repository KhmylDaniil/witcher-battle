using System.Collections.Generic;
using Witcher.Core.Entities;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Abstractions
{
	public interface IAbility
	{
		/// <summary>
		/// Наазвание способности
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Описание способности
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Навык атаки
		/// </summary>
		Skill AttackSkill { get; }

		/// <summary>
		/// Тип урона
		/// </summary>
		DamageType DamageType { get; }

		/// <summary>
		/// Количество кубов атаки
		/// </summary>
		int AttackDiceQuantity { get; }

		/// <summary>
		/// Модификатор атаки
		/// </summary>
		int DamageModifier { get; }
		
		/// <summary>
		/// Точность атаки
		/// </summary>
		int Accuracy { get; }

		/// <summary>
		/// Накладываемые состояния
		/// </summary>
		List<AppliedCondition> AppliedConditions { get;  }

		/// <summary>
		/// Защитные навыки
		/// </summary>
		List<DefensiveSkill> DefensiveSkills { get; }
	}
}
