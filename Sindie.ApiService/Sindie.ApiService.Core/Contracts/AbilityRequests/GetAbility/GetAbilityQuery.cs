using MediatR;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbility
{
	/// <summary>
	/// Запрос на получение списка способностей
	/// </summary>
	public class GetAbilityQuery : IRequest<GetAbilityResponse>
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
		/// Фильтр по параметру атаки
		/// </summary>
		public Guid? AttackParameterId { get; set; }

		/// <summary>
		/// Фильтр по типу уронв
		/// </summary>
		public Guid? DamageTypeId { get; set; }

		/// <summary>
		/// фильтр по названию накладываемого состояния
		/// </summary>
		public Guid? ConditionId { get; set; }

		/// <summary>
		/// Минимальное количество ккубов атаки
		/// </summary>
		public int MinAttackDiceQuantity { get; set; }

		/// <summary>
		/// Максимальное количество кубов атаки
		/// </summary>
		public int MaxAttackDiceQuantity { get; set; }

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
		/// Количество записей на одной странице 
		/// </summary>
		public int PageSize { get; set; }

		/// <summary>
		/// Номер страницы, с которой вывести записи
		/// </summary>
		public int PageNumber { get; set; }

		/// <summary>
		/// Сортировка по полю
		/// </summary>
		public string OrderBy { get; set; }

		/// <summary>
		/// Сортировка по возрастанию
		/// </summary>
		public bool IsAscending { get; set; }
	}
}
