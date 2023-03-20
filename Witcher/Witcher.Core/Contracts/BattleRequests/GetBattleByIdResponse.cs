using System;
using System.Collections.Generic;

namespace Witcher.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Ответ на запрос получения битвы по айди
	/// </summary>
	public class GetBattleByIdResponse
	{
		/// <summary>
		/// Айди битвы
		/// </summary>
		public Guid BattleId { get; set; }

		/// <summary>
		/// Название битвы
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание битвы
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Список существ в битве
		/// </summary>
		public List<GetBattleByIdResponseItem> Creatures { get; set; }
	}
}