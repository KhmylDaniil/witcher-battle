using Sindie.ApiService.Core.Contracts.BagRequests.TakeItems;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Requests.BagRequests.TakeItems
{
	/// <summary>
	/// Команда на помещение вещей в сумку
	/// </summary>
	public class TakeItemsCommand: TakeItemsRequest
	{
		/// <summary>
		/// Конструктор команды на помещение вещей в сумку
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		/// <param name="instanceId">Айди экземпляра</param>
		/// <param name="sourceBagId">Айди сумки-источника</param>
		/// <param name="receiveBagId">Айди сумки-получателя</param>
		/// <param name="bagItems">Список предметов в сумке</param>
		public TakeItemsCommand(
			Guid gameId,
			Guid instanceId,
			Guid sourceBagId,
			Guid receiveBagId,
			List<TakeItemsRequestItem> bagItems)
		{
			GameId = gameId;
			InstanceId = instanceId;
			SourceBagId = sourceBagId;
			ReceiveBagId = receiveBagId;
			BagItems = bagItems;
		}
	}
}
