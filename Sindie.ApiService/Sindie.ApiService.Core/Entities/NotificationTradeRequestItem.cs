using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Элемент уведомления о предложении передать предметы
	/// </summary>
	public class NotificationTradeRequestItem
    {
		/// <summary>
		/// Айди предмета
		/// </summary>
		public Guid ItemId { get; set; }

		/// <summary>
		/// Название предмета
		/// </summary>
		public string ItemName { get; set; }

		/// <summary>
		/// Количество в стаке
		/// </summary>
		public int Quantity { get; set; }

		/// <summary>
		/// Максимальное количество в стеке
		/// </summary>
		public int MaxQuantity { get; set; }

		/// <summary>
		/// Общий вес стака
		/// </summary>
		public double TotalWeight { get; set; }

		/// <summary>
		/// Стак
		/// </summary>
		public int Stack { get; set; }

		/// <summary>
		/// Конструктор для EF
		/// </summary>
        protected NotificationTradeRequestItem()
        {
        }
		/// <summary>
		/// Конструктор для Элемент уведомления о предложении передать предметы 
		/// </summary>
		/// <param name="item">Предмет</param>
		/// <param name="quantity">Количество</param>
		/// <param name="stack">Стак</param>
		public NotificationTradeRequestItem(Item item, int quantity, int stack)
        {
			ItemId = item.Id;
			ItemName = item.Name;
			Quantity = quantity;
			MaxQuantity = item.MaxQuantity;
			Stack = stack;
			TotalWeight = quantity * item.Weight;
		}
	}
}
