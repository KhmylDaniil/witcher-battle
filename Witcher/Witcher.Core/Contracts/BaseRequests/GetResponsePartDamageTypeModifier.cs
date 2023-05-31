using System;

namespace Witcher.Core.Contracts.BaseRequests
{
	/// <summary>
	/// Элемент ответа на запрос шаблона существа по айди - модификаторы урона
	/// </summary>
	public class GetResponsePartDamageTypeModifier
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Тип урона
		/// </summary>
		public BaseData.Enums.DamageType DamageType { get; set; }

		/// <summary>
		/// Модификатор типа урона
		/// </summary>
		public BaseData.Enums.DamageTypeModifier DamageTypeModifier { get; set; }
	}
}
