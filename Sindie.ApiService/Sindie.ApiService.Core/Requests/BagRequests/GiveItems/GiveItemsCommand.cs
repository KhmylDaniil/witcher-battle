using Sindie.ApiService.Core.Contracts.BagRequests.GiveItems;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Requests.BagRequests.GiveItems
{
	public class GiveItemsCommand: GiveItemsRequest
	{
		/// <summary>
		/// Конструктор команды на передачу вещей из сумки
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		/// <param name="instanceId">Айди экземпляра</param>
		/// <param name="sourceBagId">Айди сумки-источника</param>
		/// <param name="receiveCharacterId">Айди персонажа-получателя</param>
		/// <param name="bagItems">Список предметов в сумке</param>
		public GiveItemsCommand(
			Guid gameId,
			Guid instanceId,
			Guid sourceBagId,
			Guid receiveCharacterId,
			List<GiveItemsRequestItem> bagItems)
		{
			GameId = gameId;
			InstanceId = instanceId;
			SourceBagId = sourceBagId;
			ReceiveCharacterId = receiveCharacterId;
			BagItems = bagItems;
		}
	}
}
