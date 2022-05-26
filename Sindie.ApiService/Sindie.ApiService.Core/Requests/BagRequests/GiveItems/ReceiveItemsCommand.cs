using Sindie.ApiService.Core.Contracts.BagRequests.GiveItems;
using System;

namespace Sindie.ApiService.Core.Requests.BagRequests.GiveItems
{
	/// <summary>
	/// Команда на получение предметов
	/// </summary>
	public class ReceiveItemsCommand: ReceiveItemsRequest
	{
		/// <summary>
		/// Конструктор команды на получение предметов
		/// </summary>
		/// <param name="notificationId">Айди уведомления</param>
		/// <param name="consent">Согласие на передачу</param>
		public ReceiveItemsCommand(
			Guid notificationId,
			bool consent)
		{
			NotificationId = notificationId;
			Consent = consent;
		}
	}
}
