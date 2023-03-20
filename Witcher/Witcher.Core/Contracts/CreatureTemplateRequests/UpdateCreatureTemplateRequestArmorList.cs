using System;

namespace Witcher.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Элемент изменения или создания шаблона существа - броня
	/// </summary>
	public sealed class UpdateCreatureTemplateRequestArmorList
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
