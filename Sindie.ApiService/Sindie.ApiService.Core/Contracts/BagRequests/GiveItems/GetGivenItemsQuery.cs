using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.BagRequests.GiveItems
{
	/// <summary>
	/// Запрос на получение списка предлагаемых к передаче предметов
	/// </summary>
	public class GetGivenItemsQuery: IRequest<GetGivenItemsResponse>
	{
		/// <summary>
		/// Айди уведомления о предложении передать предметы
		/// </summary>
		public Guid NotificationId { get; set; }
	}
}
