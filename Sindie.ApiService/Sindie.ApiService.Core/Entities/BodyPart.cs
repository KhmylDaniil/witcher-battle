
namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Часть тела
	/// </summary>
	public class BodyPart: BodyTemplatePart
	{
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
