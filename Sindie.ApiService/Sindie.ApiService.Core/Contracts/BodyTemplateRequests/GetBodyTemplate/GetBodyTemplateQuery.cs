using MediatR;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.Services.DateTimeProvider;
using System;

namespace Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplate
{
	/// <summary>
	/// Запрос на получение списка шаблонов тела
	/// </summary>
	public class GetBodyTemplateQuery: GetBaseQuery, IValidatableCommand<GetBodyTemplateResponse>
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Фильтр по названию
		/// </summary>
		public string Name {get; set; }

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
