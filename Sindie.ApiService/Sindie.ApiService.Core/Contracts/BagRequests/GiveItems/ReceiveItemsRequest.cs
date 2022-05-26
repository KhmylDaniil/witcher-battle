using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.BagRequests.GiveItems
{
    /// <summary>
	/// Запрос на получение предметов
	/// </summary>
	public class ReceiveItemsRequest: IRequest
    {
		/// <summary>
		/// Айди уведомления
		/// </summary>
		public Guid NotificationId { get; set; }

		/// <summary>
		/// Согласие на получение предметов
		/// </summary>
		public bool Consent { get; set; }
	}
}
