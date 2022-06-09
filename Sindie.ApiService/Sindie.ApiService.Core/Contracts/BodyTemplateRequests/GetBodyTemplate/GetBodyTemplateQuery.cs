using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplate
{
	/// <summary>
	/// Запрос на получение списка шаблонов тела
	/// </summary>
	public class GetBodyTemplateQuery: IRequest<GetBodyTemplateResponse>
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
