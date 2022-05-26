using System;

namespace Sindie.ApiService.Core.Contracts.BagRequests.GetBag
{
	public class GetBagResponseItem
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
		public int MaxQuantityItem { get; set; }

		/// <summary>
		/// Количество предметов в стеке
		/// </summary>
		public int QuantityItem { get; set; }

		/// <summary>
		/// Указанный предмету стек
		/// </summary>
		public int Stack { get; set; }

		/// <summary>
		/// Количество заблокированных предметов
		/// </summary>
		public int Blocked { get; set; }
	}
}

