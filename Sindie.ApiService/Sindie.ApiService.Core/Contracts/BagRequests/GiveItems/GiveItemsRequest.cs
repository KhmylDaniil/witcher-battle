using MediatR;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.BagRequests.GiveItems
{
	/// <summary>
	/// Запрос на передачу вещей из сумки
	/// </summary>
	public class GiveItemsRequest: IRequest
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Айди экземпляра игры
		/// </summary>
		public Guid InstanceId { get; set; }

		/// <summary>
		/// Айди сумки-источника
		/// </summary>
		public Guid SourceBagId { get; set; }

		/// <summary>
		/// Айди перонажа-получателя
		/// </summary>
		public Guid ReceiveCharacterId { get; set; }

		/// <summary>
		/// Список элеметов запроса на передачу вещей из сумки
		/// </summary>
		public List<GiveItemsRequestItem> BagItems { get; set; }
	}
}

