using MediatR;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.BagRequests.TakeItems
{
	/// <summary>
	/// Запрос на помещение вещей в сумку
	/// </summary>
	public class TakeItemsRequest: IRequest
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
		/// Айди сумки-получателя
		/// </summary>
		public Guid ReceiveBagId { get; set; }

		/// <summary>
		/// Элементы запроса на помещение вещей в сумку
		/// </summary>
		public List<TakeItemsRequestItem> BagItems { get; set; }
	}
}
