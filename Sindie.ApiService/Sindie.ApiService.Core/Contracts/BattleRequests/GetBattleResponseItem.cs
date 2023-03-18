using System;

namespace Witcher.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Элемент ответа на запрос списка битв
	/// </summary>
	public class GetBattleResponseItem
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Наазвание битвы
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание битвы
		/// </summary>
		public string Description { get; set; }
	}
}