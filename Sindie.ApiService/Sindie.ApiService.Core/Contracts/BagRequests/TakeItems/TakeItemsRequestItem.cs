using System;

namespace Sindie.ApiService.Core.Contracts.BagRequests.TakeItems
{
	/// <summary>
	/// Элемент запроса на перемещение вещей в сумку
	/// </summary>
	public class TakeItemsRequestItem
	{
		/// <summary>
		/// Айди предмета
		/// </summary>
		public Guid ItemId { get; set; }

		/// <summary>
		/// Количество предметов в стеке
		/// </summary>
		public int QuantityItem { get; set; }
	}
}
