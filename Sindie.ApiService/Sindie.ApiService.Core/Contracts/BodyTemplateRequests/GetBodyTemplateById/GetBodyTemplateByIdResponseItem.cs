using System;

namespace Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplateById
{
	/// <summary>
	/// Элемент ответа на запрос шаблона тела по айди - часть шаблона тела
	/// </summary>
	public class GetBodyTemplateByIdResponseItem
	{
		/// <summary>
		/// Айди части шаблона тела
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Пенальти за прицеливание
		/// </summary>
		public int HitPenalty { get; set; }

		/// <summary>
		/// Модификатор урона
		/// </summary>
		public double DamageModifier { get; set; }

		/// <summary>
		/// Минимум на попадание
		/// </summary>
		public int MinToHit { get; set; }

		/// <summary>
		/// Максимум на попадание
		/// </summary>
		public int MaxToHit { get; set; }
	}
}
