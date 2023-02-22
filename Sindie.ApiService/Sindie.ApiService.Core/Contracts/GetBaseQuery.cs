
using System.ComponentModel.DataAnnotations;

namespace Sindie.ApiService.Core.Contracts
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
		public int PageSize { get; set; }

		/// <summary>
		/// Номер страницы, с которой вывести записи
		/// </summary>
		[Range(1, int.MaxValue)]
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
