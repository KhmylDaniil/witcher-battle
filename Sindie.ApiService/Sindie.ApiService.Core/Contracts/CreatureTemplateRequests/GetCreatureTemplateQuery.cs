using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Запрос на получение списка шаблонов существа
	/// </summary>
	public class GetCreatureTemplateQuery : GetBaseQuery, IValidatableCommand<IEnumerable<GetCreatureTemplateResponseItem>>
	{
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

		public void Validate()
		{
			if (CreationMaxTime != default && CreationMinTime >= CreationMaxTime)
				throw new RequestFieldIncorrectDataException<GetCreatureTemplateQuery>(nameof(CreationMaxTime));

			if (ModificationMaxTime != default && ModificationMinTime >= ModificationMaxTime)
				throw new RequestFieldIncorrectDataException<GetCreatureTemplateQuery>(nameof(ModificationMaxTime));
		}
	}
}
