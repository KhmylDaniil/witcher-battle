using Sindie.ApiService.Core.Contracts.BagRequests.GiveItems;
using System;

namespace Sindie.ApiService.Core.Requests.BagRequests.GiveItems
{
	/// <summary>
	/// Команда на получение списка предлагаемых к передаче предметов
	/// </summary>
	public class GetGivenItemsCommand: GetGivenItemsQuery
	{
		/// <summary>
		/// Конструктор команды на получение списка предлагаемых к передаче предметов
		/// </summary>
		/// <param name="notificationId">Айди уведомления</param>
		public GetGivenItemsCommand(Guid notificationId)
        {
			NotificationId = notificationId;
        }
    }
}
