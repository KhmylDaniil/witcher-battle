using System;

namespace Sindie.ApiService.Core.Contracts.BagRequests.GiveItems
{
	/// <summary>
	/// Элемент ответа на запрос на получение списка предлагаемых к передаче предметов
	/// </summary>
	public class GetGivenItemsResponseItem
    {
		// <summary>
		/// Айди предмета
		/// </summary>
		public Guid ItemId { get; set; }

		/// <summary>
		/// Название предмета
		/// </summary>
		public string ItemName { get; set; }

		/// <summary>
		/// Максимальное количество предметов в стеке
		/// </summary>
		public int MaxQuantity { get; set; }

		/// <summary>
		/// Количество предметов в стеке
		/// </summary>
		public int Quantity { get; set; }

		/// <summary>
		/// Вес стака
		/// </summary>
		public double TotalWeight { get; set; }
	}
}
