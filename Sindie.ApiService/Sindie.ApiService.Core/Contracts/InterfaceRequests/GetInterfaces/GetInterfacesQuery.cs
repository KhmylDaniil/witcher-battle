using MediatR;
using Sindie.ApiService.Core.Abstractions;
using System.Collections;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.InterfaceRequests.GetInterfaces
{
	/// <summary>
	/// Запрос получение списка интерфейсов
	/// </summary>
	public sealed class GetInterfacesQuery : IValidatableCommand<IEnumerable<GetInterfacesQueryResponseItem>>
	{
		/// <summary>
		/// Тип интерфейса
		/// </summary>
		public string SearchText { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
