
using System.ComponentModel.DataAnnotations;

namespace Witcher.Core.Contracts.BaseRequests
{
	/// <summary>
	/// Базоввая часть гет запроса 
	/// </summary>
	public class GetBaseQuery
	{
		/// <summary>
		/// Колоичество записей на одной странице 
		/// </summary>
		[Range(1, 50)]
		public int PageSize { get; set; } = 20;

		/// <summary>
		/// Номер страницы, с которой вывести записи
		/// </summary>
		[Range(1, int.MaxValue)]
		public int PageNumber { get; set; } = 1;

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
