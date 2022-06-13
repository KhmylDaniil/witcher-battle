
using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.ChangeCreatureTemplate
{
	/// <summary>
	/// Элемент запроса изменения шаблона существа - броня
	/// </summary>
	public class ChangeCreatureTemplateRequestArmorList
	{
		/// <summary>
		/// Айди части шаблона тела
		/// </summary>
		public Guid BodyTemplatePartId { get; set; }

		/// <summary>
		/// Броня
		/// </summary>
		public int Armor { get; set; }
	}
}
