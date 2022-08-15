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

		/// <summary>
		/// Айди состояния дизориентация
		/// </summary>
		public static readonly Guid StunId = new Guid("afb1c2ac-f6ab-435e-aedd-011da6f5ea9a");

		/// <summary>
		/// Названия состояния дизориентация
		/// </summary>
		public static readonly string StunName = "Stun";

		/// <summary>
		/// Айди состояния ошеломление
		/// </summary>
		public static readonly Guid StaggeredId = new Guid("afb1c2ac-f6ab-535e-aedd-011da6f5ea9a");

		/// <summary>
		/// Названия состояния ошеломление
		/// </summary>
		public static readonly string StaggeredName = "Staggered";

		/// <summary>
		/// Айди состояния опьянение
		/// </summary>
		public static readonly Guid IntoxicationId = new Guid("afb1c2ac-f6ab-635e-aedd-011da6f5ea9a");

		/// <summary>
		/// Названия состояния опьянение
		/// </summary>
		public static readonly string IntoxicationName = "Intoxication";

		/// <summary>
		/// Айди состояния галлюцинация
		/// </summary>
		public static readonly Guid HallutinationId = new Guid("afb1c2ac-f6ab-735e-aedd-011da6f5ea9a");

		/// <summary>
		/// Названия состояния галлюцинация
		/// </summary>
		public static readonly string HallutinationName = "Hallutination";

		/// <summary>
		/// Айди состояния тошнота
		/// </summary>
		public static readonly Guid NauseaId = new Guid("afb1c2ac-f6ab-835e-aedd-011da6f5ea9a");

		/// <summary>
		/// Названия состояния тошнота
		/// </summary>
		public static readonly string NauseaName = "Nausea";

		/// <summary>
		/// Айди состояния удушье
		/// </summary>
		public static readonly Guid SufflocationId = new Guid("afb1c2ac-f6ab-935e-aedd-011da6f5ea9a");

		/// <summary>
		/// Названия состояния удушье
		/// </summary>
		public static readonly string SufflocationName = "Sufflocation";

		/// <summary>
		/// Айди состояния слепота
		/// </summary>
		public static readonly Guid BlindedId = new Guid("afb1c2ac-f6ab-035e-aedd-011da6f5ea9a");

		/// <summary>
		/// Названия состояния слепота
		/// </summary>
		public static readonly string BlindedName = "Blinded";

		/// <summary>
		/// Айди состояния при смерти
		/// </summary>
		public static readonly Guid DyingId = new Guid("afb1c2ac-f6ab-035e-aedd-011da6f5ea9b");

		/// <summary>
		/// Названия состояния при смерти
		/// </summary>
		public static readonly string DyingName = "Blinded";
	}
}
