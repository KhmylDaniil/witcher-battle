using static Sindie.ApiService.Core.BaseData.Enums;
using System;
using System.Collections.Generic;
using Sindie.ApiService.Core.BaseData;

namespace Sindie.ApiService.Core.Contracts.BattleRequests
{
	public sealed class GetCreatureByIdResponseAbility
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
		/// Накладывваемые состояния
		/// </summary>
		public List<(Condition condition, int applyChance)> AppliedConditions { get; set; }
	}
}