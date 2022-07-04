using MediatR;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.CreateAbility
{
	/// <summary>
	/// Запрос создания способности
	/// </summary>
	public class CreateAbilityRequest: IRequest
	{
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
		/// Параметры для защиты
		/// </summary>
		public List<Guid> DefensiveParameters { get; set; }

		/// <summary>
		/// Типы урона
		/// </summary>
		public List<Guid> DamageTypes { get; set; }

		/// <summary>
		/// Накладываемые состояния
		/// </summary>
		public List<CreateAbilityRequestAppliedCondition> AppliedConditions { get; set; }
	}
}
