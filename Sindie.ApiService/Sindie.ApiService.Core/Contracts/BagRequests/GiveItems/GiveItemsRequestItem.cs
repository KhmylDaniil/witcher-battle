using System;

namespace Sindie.ApiService.Core.Contracts.BagRequests.GiveItems
{
	/// <summary>
	/// Элемент запроса на передачу вещей из сумки
	/// </summary>
	public class GiveItemsRequestItem
	{
		/// <summary>
		/// Айди предмета
		/// </summary>
		public Guid ItemId { get; set; }

		/// <summary>
		/// Количество предметов для блокирования
		/// </summary>
		public int ToBlockQuantity { get; set; }

		/// <summary>
		/// Стак
		/// </summary>
		public int Stack { get; set; }
	}
}
