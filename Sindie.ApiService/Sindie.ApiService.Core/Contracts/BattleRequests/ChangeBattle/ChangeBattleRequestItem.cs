using System;

namespace Sindie.ApiService.Core.Contracts.BattleRequests.ChangeBattle
{
	/// <summary>
	/// Элемент запроса на изменение боя
	/// </summary>
	public class ChangeBattleRequestItem
	{
		/// <summary>
		/// Айди существа
		/// </summary>
		public Guid? CreatureId { get; set; }
		
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
