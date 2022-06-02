
namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Часть тела
	/// </summary>
	public class BodyPart
	{
		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Модификатор урона
		/// </summary>
		public double DamageModifer { get; set; }

		/// <summary>
		/// Минимум на попадание
		/// </summary>
		public int MinToHit { get; set; }

		/// <summary>
		/// Максимум на попадание
		/// </summary>
		public int MaxToHit { get; set; }

		/// <summary>
		/// Начальная броня
		/// </summary>
		public int StartingArmor { get; set; }

		/// <summary>
		/// Текущая броня
		/// </summary>
		public int CurrentArmor { get; set; }
	}
}
