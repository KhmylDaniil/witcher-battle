using System;

namespace Sindie.ApiService.Core.BaseData
{
	/// <summary>
	/// Состояния (справочник)
	/// </summary>
	public static class Conditions
	{
		/// <summary>
		/// Айди состояния кровотечение
		/// </summary>
		public static readonly Guid BleedId = new Guid("9994e0d0-3147-4791-9053-9667cbe127d7");

		/// <summary>
		/// Названия состояния кровотечение
		/// </summary>
		public static readonly string BleedName = "Bleed";

		/// <summary>
		/// Айди состояния кровавая рана
		/// </summary>
		public static readonly Guid BleedingWoundId = new Guid("7794e0d0-3147-4791-9053-9667cbe127d7");

		/// <summary>
		/// Названия состояния кровавая рана
		/// </summary>
		public static readonly string BleedingWoundName = "BleedingWound";

		/// <summary>
		/// Айди состояния отравление
		/// </summary>
		public static readonly Guid PoisonId = new Guid("8894e0d0-3147-4791-9053-9667cbe127d7");

		/// <summary>
		/// Названия состояния отравление
		/// </summary>
		public static readonly string PoisonName = "Poison";

		/// <summary>
		/// Айди состояния горение
		/// </summary>
		public static readonly Guid FireId = new Guid("8895e0d0-3147-4791-9053-9667cbe127d7");

		/// <summary>
		/// Названия состояния горение
		/// </summary>
		public static readonly string FireName = "Fire";

		/// <summary>
		/// Айди состояния горение
		/// </summary>
		public static readonly Guid FreezeId = new Guid("8895e0d1-3147-4791-9053-9667cbe127d7");

		/// <summary>
		/// Названия состояния горение
		/// </summary>
		public static readonly string FreezeName = "Freeze";
	}
}
