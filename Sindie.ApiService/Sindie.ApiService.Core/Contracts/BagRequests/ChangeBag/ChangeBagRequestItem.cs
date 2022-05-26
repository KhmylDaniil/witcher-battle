using System;

namespace Sindie.ApiService.Core.Contracts.BagRequests.ChangeBag
{
	/// <summary>
	/// Подкласс запроса изменения сумки
	/// </summary>
	public class ChangeBagRequestItem
	{
		/// <summary>
		/// Айди предмета
		/// </summary>
		public Guid ItemId { get; set; }

		/// <summary>
		/// Количество предметов в стеке
		/// </summary>
		public int QuantityItem { get; set; }

		/// <summary>
		/// Указанный предмету стек
		/// </summary>
		public int? Stack { get; set; }
		
		/// <summary>
		/// Количество заблокированных предметов
		/// </summary>
		public int Blocked { get; set; }
	}
}