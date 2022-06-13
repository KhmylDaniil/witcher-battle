
using System;

namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.CreateCreatureTemplate
{
	/// <summary>
	/// Элемент запроса создания шаблона существа - броня
	/// </summary>
	public class CreateCreatureTemplateRequestArmorList
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
