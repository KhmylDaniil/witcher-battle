
namespace Sindie.ApiService.Core.Contracts
{
	/// <summary>
	/// Базоввая часть гет запроса 
	/// </summary>
	public  class GetBaseQuery
	{
		/// <summary>
		/// Колоичество записей на одной странице 
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
