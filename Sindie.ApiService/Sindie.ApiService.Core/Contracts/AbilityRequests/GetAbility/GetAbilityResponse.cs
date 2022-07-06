using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbility
{
	/// <summary>
	/// Ответ на запрос на получение списка способностей
	public class GetAbilityResponse
	{
		/// <summary>
		/// Список способностей
		/// </summary>
		public List<GetAbilityResponseItem> AbilitiesList { get; set; }

		/// <summary>
		/// Общее количество способностей в списке
		/// </summary>
		public int TotalCount { get; set; }
	}
}
