using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplate
{
	/// <summary>
	/// Ответ на запрос на получение списка шаблонов существа
	/// </summary>
	public sealed class GetCreatureTemplateResponse
	{
		/// <summary>
		/// Список шаблонов существа
		/// </summary>
		public List<GetCreatureTemplateResponseItem> CreatureTemplatesList { get; set; }

		/// <summary>
		/// Общее количество шаблонов существа в списке
		/// </summary>
		public int TotalCount { get; set; }
	}
}
