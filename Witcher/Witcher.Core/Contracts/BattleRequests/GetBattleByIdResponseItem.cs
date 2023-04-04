using System;

namespace Witcher.Core.Contracts.BattleRequests
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

		/// <summary>
		/// Хиты
		/// </summary>
		public (int current, int max) HP { get; set; }

		/// <summary>
		/// Эффекты
		/// </summary>
		public string Effects { get; set; }

		/// <summary>
		/// Инициатива в битве
		/// </summary>
		public int Initiative { get; set; }

		/// <summary>
		/// Является ли существо персонажем
		/// </summary>
		public bool IsCharacter { get; set; }
	}
}