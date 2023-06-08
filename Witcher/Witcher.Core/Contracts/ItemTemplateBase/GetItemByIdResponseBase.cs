using System;

namespace Witcher.Core.Contracts.ItemTemplateBase
{
	public class GetItemByIdResponseBase
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Наазвание
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Цена
		/// </summary>
		public int Price { get; set; }

		/// <summary>
		/// Вес
		/// </summary>
		public int Weight { get; set; }

		/// <summary>
		/// Стакается
		/// </summary>
		public bool IsStackable { get; set; }

		/// <summary>
		/// Дата создания
		/// </summary>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Дата изменения
		/// </summary>
		public DateTime ModifiedOn { get; set; }
	}
}
