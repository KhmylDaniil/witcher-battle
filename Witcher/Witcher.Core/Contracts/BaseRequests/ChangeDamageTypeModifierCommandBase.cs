using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.BaseRequests
{
	public class ChangeDamageTypeModifierCommandBase
	{
		/// <summary>
		/// Тип урона
		/// </summary>
		public DamageType DamageType { get; set; }

		/// <summary>
		/// Модификатор урона
		/// </summary>
		public DamageTypeModifier DamageTypeModifier { get; set; }
	}
}
