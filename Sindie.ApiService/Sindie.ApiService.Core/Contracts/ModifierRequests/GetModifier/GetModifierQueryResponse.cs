using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.ModifierRequests.GetModifier
{
	/// <summary>
	/// Ответ на запрос получения списка модификаторов
	/// </summary>
	public class GetModifierQueryResponse
	{
		/// <summary>
		/// Список модификаторов
		/// </summary>
		public List<GetModifierQueryResponseItem> ModifiersList { get; set; }

		/// <summary>
		/// Общее количество модификаторов в списке
		/// </summary>
		public int TotalCount { get; set; }
	}
}
