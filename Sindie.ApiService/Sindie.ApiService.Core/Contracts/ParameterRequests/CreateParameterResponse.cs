using System;

namespace Sindie.ApiService.Core.Contracts.ParameterRequests
{
	/// <summary>
	/// Ответ на команду создания параметра
	/// </summary>
	public class CreateParameterResponse
	{
		/// <summary>
		/// Айди параметра
		/// </summary>
		public Guid ParameterId { get; set; }
	}
}