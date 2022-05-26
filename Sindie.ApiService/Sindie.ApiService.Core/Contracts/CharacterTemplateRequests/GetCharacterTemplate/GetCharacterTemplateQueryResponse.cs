using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.CharacterTemplateRequests.GetCharacterTemplate
{
	/// <summary>
	/// Ответ на запрос списка шаблонов персонажа
	/// </summary>
	public class GetCharacterTemplateQueryResponse
	{
		/// <summary>
		/// Список шаблонов персонажа
		/// </summary>
		public List<GetCharacterTemplateQueryResponseItem> CharacterTemplatesList { get; set; }

		/// <summary>
		/// Общее количество элементов в списке
		/// </summary>
		public int TotalCount { get; set; }
	}
}
