using MediatR;

namespace Sindie.ApiService.Core.Contracts.InterfaceRequests.GetInterfaces
{
	/// <summary>
	/// Запрос получение списка интерфейсов
	/// </summary>
	public sealed class GetInterfacesQuery : IRequest<GetInterfacesQueryResponse>
	{
		/// <summary>
		/// Тип интерфейса
		/// </summary>
		public string SearchText { get; set; }
	}
}
