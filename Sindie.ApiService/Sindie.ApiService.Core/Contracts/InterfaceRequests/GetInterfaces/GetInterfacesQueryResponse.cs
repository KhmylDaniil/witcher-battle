using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.InterfaceRequests.GetInterfaces
{
	/// <summary>
	/// Ответ на запрос получения списка интерфейсов
	/// </summary>
	public sealed class GetInterfacesQueryResponse
	{
		/// <summary>
		///  Список найденных по запросу интерфейсов
		/// </summary>
		public List<GetInterfacesQueryResponseItem> InterfacesList { get; set; }

		/// <summary>
		/// Общее количество интерфейсов в списке
		/// </summary>
		public int TotalCount { get; set; }
	}
}
