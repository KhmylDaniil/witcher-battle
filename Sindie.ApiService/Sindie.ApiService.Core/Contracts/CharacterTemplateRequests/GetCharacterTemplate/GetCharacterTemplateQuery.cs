using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.CharacterTemplateRequests.GetCharacterTemplate
{
	/// <summary>
	/// Запрос на получение списка шаблонов персонажа
	/// </summary>
	public class GetCharacterTemplateQuery: IRequest<GetCharacterTemplateQueryResponse>
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Фильтр по названию шаблона персонажа
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Фильтр по имени персонажа
		/// </summary>
		public string CharacterName { get; set; }

		/// <summary>
		/// Фильтр по автору
		/// </summary>
		public string AuthorName { get; set; }

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
