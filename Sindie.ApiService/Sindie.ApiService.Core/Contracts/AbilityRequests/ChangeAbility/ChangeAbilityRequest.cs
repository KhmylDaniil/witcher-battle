using MediatR;
using System;
using System.Collections.Generic;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.ChangeAbility
{
	/// <summary>
	/// Запрос изменения способности
	/// </summary>
	public class ChangeAbilityRequest: IRequest
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }
		
		/// <summary>
		/// Ацди игры
		/// </summary>
		public Guid GameId { get; set; }

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
		/// Тип урона
		/// </summary>
		public DamageType DamageType { get; set; }

		/// <summary>
		/// Навыки для защиты
		/// </summary>
		public List<Skill> DefensiveSkills { get; set; }

		/// <summary>
		/// Накладываемые состояния
		/// </summary>
		public List<ChangeAbilityRequestAppliedCondition> AppliedConditions { get; set; }
	}
}
