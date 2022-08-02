using System;

namespace Sindie.ApiService.Core.Contracts.BattleRequests.CreateBattle
{
	/// <summary>
	/// Элемент запроса на создание боя
	/// </summary>
	public sealed class CreateBattleRequestItem
	{
		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; set; }

		/// <summary>
		/// Название существа
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание существа
		/// </summary>
		public string Description { get; set; }
	}
}
