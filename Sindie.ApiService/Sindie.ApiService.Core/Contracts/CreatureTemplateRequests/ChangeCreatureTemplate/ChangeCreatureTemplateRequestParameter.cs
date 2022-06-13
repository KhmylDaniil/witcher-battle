using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.ChangeCreatureTemplate
{
	public class ChangeCreatureTemplateRequestParameter
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid? Id { get; set; }
		
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
