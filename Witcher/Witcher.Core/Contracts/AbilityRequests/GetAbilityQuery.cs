using System;
using System.Collections.Generic;
using Witcher.Core.Contracts.BaseRequests;
using MediatR;

namespace Witcher.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Запрос на получение списка способностей
	/// </summary>
	public sealed class GetAbilityQuery : GetBaseQuery, IRequest<IEnumerable<GetAbilityResponseItem>>
	{
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
	}
}
