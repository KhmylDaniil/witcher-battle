using Witcher.Core.Abstractions;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.BodyTemplateRequests
{
	/// <summary>
	/// Элемент запроса создания или изменения шаблона тела
	/// </summary>
	public class UpdateBodyTemplateRequestItem : IBodyTemplatePartData
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
