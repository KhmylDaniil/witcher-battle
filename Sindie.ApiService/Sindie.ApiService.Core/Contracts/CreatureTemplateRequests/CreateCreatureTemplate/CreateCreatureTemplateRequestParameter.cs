using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.CreateCreatureTemplate
{
	/// <summary>
	/// Элемент запроса создания шаблона существа - параметр шаблона существа
	/// </summary>
	public class CreateCreatureTemplateRequestParameter
	{
		/// <summary>
		/// Айди параметра
		/// </summary>
		public Guid ParameterId { get; set; }

		/// <summary>
		/// Значение параметра
		/// </summary>
		public int Value { get; set; } 
	}
}
