using System;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Contracts.BodyTemplateRequests
{
	/// <summary>
	/// Элемент ответа на запрос шаблона тела по айди - часть шаблона тела
	/// </summary>
	public sealed class GetBodyTemplateByIdResponseItem
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
		/// Тип части тела
		/// </summary>
		public BodyPartType BodyPartType { get; set; }

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
