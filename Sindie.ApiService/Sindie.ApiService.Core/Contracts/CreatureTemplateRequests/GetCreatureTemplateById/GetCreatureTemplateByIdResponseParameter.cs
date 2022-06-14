using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplateById
{
	/// <summary>
	/// Элемент ответа на запрос шаблона существа по айди - параметр шаблона существа
	/// </summary>
	public class GetCreatureTemplateByIdResponseParameter
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Айди параметра
		/// </summary>
		public Guid ParameterId { get; set; }

		/// <summary>
		/// Название параметра
		/// </summary>
		public string ParameterName { get; set; }

		/// <summary>
		/// Значение параметра
		/// </summary>
		public double ParameterValue { get; set; }
	}
}
