
namespace Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplateById
{
	/// <summary>
	/// Элемент ответа на запрос шаблона существа по айди - часть шаблона тела
	/// </summary>
	public class GetCreatureTemplateByIdResponseBodyPart
	{
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
		/// Минимум на попаданиу
		/// </summary>
		public int MinToHit { get; set; }

		/// <summary>
		/// Максимум на попаданиу
		/// </summary>
		public int MaxToHit { get; set; }

		/// <summary>
		/// Броня
		/// </summary>
		public int Armor { get; set; }
	}
}
