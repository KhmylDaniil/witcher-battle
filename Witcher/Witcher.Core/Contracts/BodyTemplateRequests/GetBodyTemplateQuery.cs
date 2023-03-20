using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;

namespace Witcher.Core.Contracts.BodyTemplateRequests
{
	/// <summary>
	/// Запрос на получение списка шаблонов тела
	/// </summary>
	public class GetBodyTemplateQuery : GetBaseQuery, IValidatableCommand<IEnumerable<GetBodyTemplateResponseItem>>
	{
		/// <summary>
		/// Фильтр по названию
		/// </summary>
		public string Name { get; set; }

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
		/// Название части тела
		/// </summary>
		public string BodyPartName { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (CreationMaxTime != default && CreationMinTime >= CreationMaxTime)
				throw new RequestFieldIncorrectDataException<GetBodyTemplateQuery>(nameof(CreationMaxTime));

			if (ModificationMaxTime != default && ModificationMinTime >= ModificationMaxTime)
				throw new RequestFieldIncorrectDataException<GetBodyTemplateQuery>(nameof(ModificationMaxTime));
		}
	}
}
