using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.BagRequests.GetBag
{
	/// <summary>
	/// Ответ на запрос на получение сумки со списком предметов в сумке
	/// </summary>
	public class GetBagResponse
	{
		/// <summary>
		/// Список предметов в сумке
		/// </summary>
		public List<GetBagResponseItem> BagItemsList { get; set; }

		/// <summary>
		/// Общее количество предметов в сумке
		/// </summary>
		public int TotalCount { get; set; }
	}
}
