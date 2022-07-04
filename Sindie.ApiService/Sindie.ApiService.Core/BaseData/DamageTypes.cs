using System;

namespace Sindie.ApiService.Core.BaseData
{
	/// <summary>
	/// Типы урона справочник
	/// </summary>
	public static class DamageTypes
	{
		/// <summary>
		/// Айди типа урона режущий
		/// </summary>
		public static readonly Guid SlashingId = new("42e5a598-f6e6-4ccd-8de3-d0e0963d1a33");

		/// <summary>
		/// Название типа урона режущий
		/// </summary>
		public static readonly string SlashingName = "Slashing";

		/// <summary>
		/// Айди типа урона колющий
		/// </summary>
		public static readonly Guid PiercingId = new("43e5a598-f6e6-4ccd-8de3-d0e0963d1a33");

		/// <summary>
		/// Название типа урона колющий
		/// </summary>
		public static readonly string PiercingName = "Piercing";

		/// <summary>
		/// Айди типа урона дробящий
		/// </summary>
		public static readonly Guid BludgeoningId = new("44e5a598-f6e6-4ccd-8de3-d0e0963d1a33");

		/// <summary>
		/// Название типа урона дробящий
		/// </summary>
		public static readonly string BludgeoningName = "Bludgeoning";

		/// <summary>
		/// Айди типа урона стихийный
		/// </summary>
		public static readonly Guid ElementalId = new("45e5a598-f6e6-4ccd-8de3-d0e0963d1a33");

		/// <summary>
		/// Название типа урона стихийный
		/// </summary>
		public static readonly string ElementalName = "Elemental";

		/// <summary>
		/// Айди типа урона огненный
		/// </summary>
		public static readonly Guid FireId = new("46e5a598-f6e6-4ccd-8de3-d0e0963d1a33");

		/// <summary>
		/// Название типа урона огненный
		/// </summary>
		public static readonly string FireName = "Fire";

		/// <summary>
		/// Айди типа урона серебряный
		/// </summary>
		public static readonly Guid SilverId = new("47e5a598-f6e6-4ccd-8de3-d0e0963d1a33");

		/// <summary>
		/// Название типа урона серебряный
		/// </summary>
		public static readonly string SilverName = "Silver";
	}
}
