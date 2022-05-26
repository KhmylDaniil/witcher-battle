using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.BagRequests.GiveItems
{
	/// <summary>
	/// Ответ на запрос на получение списка предлагаемых к передаче предметов
	/// </summary>
	public class GetGivenItemsResponse
    {
		/// <summary>
		/// Список предметов к передаче
		/// </summary>
		public List<GetGivenItemsResponseItem> BagItemsList { get; set; }

		/// <summary>
		/// Общее количество предметов к передаче
		/// </summary>
		public int TotalCount { get; set; }
	}
}
