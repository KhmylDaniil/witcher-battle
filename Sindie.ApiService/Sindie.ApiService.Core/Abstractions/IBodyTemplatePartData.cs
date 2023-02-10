using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Abstractions
{
	/// <summary>
	/// Данные о части шаблона тела
	/// </summary>
	public interface IBodyTemplatePartData
	{
		/// <summary>
		/// Название части тела
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Тип части тела
		/// </summary>

		public BodyPartType BodyPartType { get; }

		/// <summary>
		/// Модификатор урона
		/// </summary>
		public double DamageModifier { get; }

		/// <summary>
		/// Пенальти за прицеливание
		/// </summary>
		public int HitPenalty { get; }

		/// <summary>
		/// Минимум на попадание
		/// </summary>
		public int MinToHit { get; }

		/// <summary>
		/// Максимум на попадание
		/// </summary>
		public int MaxToHit { get; }
	}
}
