using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.UserRequests.GetUsers
{
	/// <summary>
	/// Ответ на запрос получения списка пользователей
	/// </summary>
	public class GetUsersQueryResponse
	{
		/// <summary>
		///  Список найденных по запросу пользователей
		/// </summary>
		public List<GetUsersQueryResponseItem> UsersList { get; set; }

		/// <summary>
		/// Общее количество пользователей в списке
		/// </summary>
		public int TotalCount { get; set; }
	}
}
