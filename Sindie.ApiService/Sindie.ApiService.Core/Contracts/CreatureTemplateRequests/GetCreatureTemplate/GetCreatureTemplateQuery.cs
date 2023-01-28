﻿using MediatR;
using Sindie.ApiService.Core.BaseData;
using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplate
{
	/// <summary>
	/// Запрос на получение списка шаблонов существа
	/// </summary>
	public class GetCreatureTemplateQuery : GetBaseQuery, IRequest<GetCreatureTemplateResponse>
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Фильтр по названию
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Тип существа
		/// </summary>
		public string CreatureType { get; set; }

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
		/// Фильтр по названию шаблона тела
		/// </summary>
		public string BodyTemplateName { get; set; }

		/// <summary>
		/// Фильтр по типу части тела
		/// </summary>
		public string BodyPartType { get; set; }

		/// <summary>
		/// фильтр по названию накладываемого состояния
		/// </summary>
		public string ConditionName { get; set; }
	}
}
