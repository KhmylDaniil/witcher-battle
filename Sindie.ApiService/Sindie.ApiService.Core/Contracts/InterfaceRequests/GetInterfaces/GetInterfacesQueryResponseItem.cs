using System;

namespace Sindie.ApiService.Core.Contracts.InterfaceRequests.GetInterfaces
{
	/// <summary>
	/// Шаблон интерфейса для отправки списка интерфейсов
	/// </summary>
	public sealed class GetInterfacesQueryResponseItem
	{

		/// <summary>
		/// Айди интерфейса
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Название интерфейса
		/// </summary>
		public string Name { get; set; }
	}
}
