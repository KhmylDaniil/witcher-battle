using System;
using System.Collections.Generic;
using System.Text;

namespace Sindie.ApiService.Core.Contracts.SlotRequests
{
	/// <summary>
	/// Предмет
	/// </summary>
	public class CreateSlotCommandItem
	{
		/// <summary>
		/// Название предмета
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание предмета
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Максимальное количество предметов в ячейке
		/// </summary>
		public int? MaxQuantityItem { get; set; }

		/// <summary>
		/// Может ли предмет быть снят с персонажа
		/// </summary>
		public bool? IsRemovable { get; set; }

		/// <summary>
		/// Автоматически одеваться на персонажа при попадании в сумку
		/// </summary>
		public bool? AutoWear { get; set; }

		/// <summary>
		/// Вес предмета
		/// </summary>
		public double? Weight { get; set; }
	}
}
