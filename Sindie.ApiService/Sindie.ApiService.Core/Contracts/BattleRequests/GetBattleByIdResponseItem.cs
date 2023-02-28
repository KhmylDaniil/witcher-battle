using System;

namespace Sindie.ApiService.Core.Contracts.BattleRequests
{
	/// <summary>
	/// Элемент ответа на запрос получения битвы по айди - существо
	/// </summary>
	public class GetBattleByIdResponseItem
	{
		/// <summary>
		/// Айди существа
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Название существа
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Базовое название существа по шаблону
		/// </summary>
		public string CreatureTemplateName { get; set; }

		/// <summary>
		/// Описание существа
		/// </summary>
		public string Description { get; set; }
	}
}