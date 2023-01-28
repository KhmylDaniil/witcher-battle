using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Contracts.BodyTemplateRequests.ChangeBodyTemplate
{
	/// <summary>
	/// Элемент запроса на изменение шаблона тела
	/// </summary>
	public sealed class ChangeBodyTemplateRequestItem
	{
		/// <summary>
		/// Название части тела
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Тип части тела
		/// </summary>
		public BodyPartType BodyPartType { get; set; }

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
