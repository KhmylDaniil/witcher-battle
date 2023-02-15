using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbility
{
	/// <summary>
	/// Запрос на получение списка способностей
	/// </summary>
	public class GetAbilityQuery : GetBaseQuery, IValidatableCommand<IEnumerable<Ability>>
	{
		// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Фильтр по названию
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Навык атаки
		/// </summary>
		public string AttackSkillName { get; set; }

		/// <summary>
		/// Фильтр по типу урона
		/// </summary>
		public string DamageType { get; set; }

		/// <summary>
		/// фильтр по названию накладываемого состояния
		/// </summary>
		public string ConditionName { get; set; }

		/// <summary>
		/// Минимальное количество кубов атаки
		/// </summary>
		public int MinAttackDiceQuantity { get; set; }

		/// <summary>
		/// Максимальное количество кубов атаки
		/// </summary>
		public int MaxAttackDiceQuantity { get; set; } = int.MaxValue;

		/// <summary>
		/// Фильтр по автору
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Начальное значение фильтра создания
		/// </summary>
		public DateTime CreationMinTime { get; set; }

		/// <summary>
		/// Конечное значение фильтра создания
		/// </summary>
		public DateTime CreationMaxTime { get; set; }

		/// <summary>
		/// Начальное значение фильтра модификации
		/// </summary>
		public DateTime ModificationMinTime { get; set; }

		/// <summary>
		/// Конечное значение фильтра модификации
		/// </summary>
		public DateTime ModificationMaxTime { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (MinAttackDiceQuantity < 0)
				throw new RequestFieldIncorrectDataException<GetAbilityQuery>(nameof(MinAttackDiceQuantity), 
					"Значение должно быть больше нуля.");

			if (MaxAttackDiceQuantity < MinAttackDiceQuantity)
				throw new RequestFieldIncorrectDataException<GetAbilityQuery>(nameof(MaxAttackDiceQuantity), 
					"Значение должно быть больше минимального значения фильтра по количеству кубов атаки.");
		}
	}
}
