using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.GameRequests.GetGame
{
	public class GetGameResponse
	{
		/// <summary>
		/// Список игр
		/// </summary>
		public List<GetGameResponseItem> GamesList { get; set; }

		/// <summary>
		/// Общее количество игр в списке
		/// </summary>
		public int TotalCount { get; set; }
	}
}