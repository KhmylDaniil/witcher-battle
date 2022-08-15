using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplate
{
	/// <summary>
	/// Ответ на запрос списка шаблонов тела
	/// </summary>
	public sealed class GetBodyTemplateResponse
	{
		/// <summary>
		/// Список шаблонов тела
		/// </summary>
		public List<GetBodyTemplateResponseItem> BodyTemplatesList { get; set; }

		/// <summary>
		/// Общее количество шаблонов тела в списке
		/// </summary>
		public int TotalCount { get; set; }
	}
}
