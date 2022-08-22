﻿using MediatR;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbility
{
	/// <summary>
	/// Запрос на получение списка способностей
	/// </summary>
	public class GetAbilityQuery : GetBaseQuery, IRequest<GetAbilityResponse>
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
		/// Фильтр по навыку атаки
		/// </summary>
		public Guid? AttackSkillId { get; set; }

		/// <summary>
		/// Фильтр по типу урона
		/// </summary>
		public Guid? DamageTypeId { get; set; }

		/// <summary>
		/// фильтр по названию накладываемого состояния
		/// </summary>
		public Guid? ConditionId { get; set; }

		/// <summary>
		/// Минимальное количество кубов атаки
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
	}
}