
using System;

namespace Sindie.ApiService.Core.Contracts.BodyTemplateRequests.CreateBodyTemplate
{
	/// <summary>
	/// Элемент запроса создания шаблона тела
	/// </summary>
	public sealed class CreateBodyTemplateRequestItem
	{
		/// <summary>
		/// Название части тела
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Айди типа части тела
		/// </summary>
		public Guid BodyPartTypeId { get; set; }

		/// <summary>
		/// Модификатор урона
		/// </summary>
		public double DamageModifier { get; set; }

		/// <summary>
		/// Пенальти за прицеливание
		/// </summary>
		public int HitPenalty { get; set; }

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
