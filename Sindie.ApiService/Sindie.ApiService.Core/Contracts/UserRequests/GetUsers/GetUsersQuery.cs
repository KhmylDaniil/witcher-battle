using MediatR;

namespace Sindie.ApiService.Core.Contracts.UserRequests.GetUsers
{
	/// <summary>
	/// Запрос получение списка пользователей
	/// </summary>
	public class GetUsersQuery : IRequest<GetUsersQueryResponse>
	{
		/// <summary>
		/// Подстрока для поиска пользователей
		/// </summary>
		public string SearchText { get; set; }

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
