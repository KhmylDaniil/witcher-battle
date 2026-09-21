using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Drafts
{
	public enum CriticalWoundSeverity
	{
		Simple = 1,
		Medium = 2,
		Difficult = 3,
	}

	/// <summary>
	/// Критические ранения: при превышении броска атаки над броском защиты на 7/10/13 и более (при
	/// условии, что удар нанёс хоть 1 урон) накладывается ранение Simple/Medium/Difficult, зависящее
	/// от части тела и типа урона — см. Condition.cs (72 значения вида SimpleHeadFire, добавленные в
	/// конце enum). Названия ранений (Simple*/Medium*/Difficult*) читаемого текста не несут — их даёт
	/// <see cref="FlavorNames"/>, прямо по таблице дизайнера. Один и тот же excess>=13 не считается
	/// одновременно Difficult и Medium — берётся самый тяжёлый подходящий порог.
	/// </summary>
	public static class CriticalWoundCatalog
	{
		/// <summary>Порог excess (AttackTotal-DefenseTotal), с которого применяется соответствующая тяжесть.</summary>
		public static CriticalWoundSeverity? GetSeverity(int attackExcess) => attackExcess switch
		{
			>= 13 => CriticalWoundSeverity.Difficult,
			>= 10 => CriticalWoundSeverity.Medium,
			>= 7 => CriticalWoundSeverity.Simple,
			_ => null,
		};

		/// <summary>Немодифицируемый доп. урон критического попадания — прибавляется после всех модификаторов.</summary>
		public static int GetBonusDamage(CriticalWoundSeverity severity) => severity switch
		{
			CriticalWoundSeverity.Simple => 3,
			CriticalWoundSeverity.Medium => 5,
			CriticalWoundSeverity.Difficult => 8,
			_ => 0,
		};

		public static Condition GetWound(CriticalWoundSeverity severity, BodyPartType bodyPart, DamageType damageType) =>
			Enum.Parse<Condition>($"{severity}{bodyPart}{damageType}");

		/// <summary>true, если existing не легче candidate — т.е. более лёгкое повторное ранение той же части+типа урона не заменяет уже наложенное.</summary>
		public static bool IsAtLeastAsSevere(Condition existing, Condition candidate) => SeverityRank[existing] >= SeverityRank[candidate];

		/// <summary>
		/// Ключ "слота" критического ранения — часть тела + тип урона. У существа части тела с одним и
		/// тем же BodyPartType (например, несколько Leg) — это РАЗНЫЕ слоты (ключ по CreatureTemplatePart.Id,
		/// не по типу), у персонажа — по конкретной HumanBodyPart (левая и правая рука/нога — тоже разные слоты).
		/// </summary>
		public static string SlotKey(long creaturePartId, DamageType damageType) => $"part:{creaturePartId}:{damageType}";

		public static string SlotKey(HumanBodyPart humanBodyPart, DamageType damageType) => $"human:{humanBodyPart}:{damageType}";

		public static string GetFlavorName(Condition wound) => FlavorNames.GetValueOrDefault(wound, wound.ToString());

		private static readonly Dictionary<Condition, CriticalWoundSeverity> SeverityRank = BuildSeverityRank();

		private static Dictionary<Condition, CriticalWoundSeverity> BuildSeverityRank()
		{
			var result = new Dictionary<Condition, CriticalWoundSeverity>();
			foreach (CriticalWoundSeverity severity in Enum.GetValues<CriticalWoundSeverity>())
			{
				foreach (BodyPartType bodyPart in Enum.GetValues<BodyPartType>())
				{
					if (bodyPart == BodyPartType.Void)
					{
						continue;
					}

					foreach (DamageType damageType in Enum.GetValues<DamageType>())
					{
						var condition = Enum.Parse<Condition>($"{severity}{bodyPart}{damageType}");
						result[condition] = severity;
					}
				}
			}

			return result;
		}

		/// <summary>Русские названия ранений — из таблицы дизайнера, по части тела и типу урона (без учёта конкретной части тела внутри одного BodyPartType — Рука/Нога/Крыло/Хвост используют одни и те же названия).</summary>
		private static readonly Dictionary<Condition, string> FlavorNames = new()
		{
			[Condition.SimpleHeadPiercing] = "Касательный удар по черепу",
			[Condition.SimpleHeadSlashing] = "Скальпирующая рана",
			[Condition.SimpleHeadBludgeoning] = "Перебитый нос",
			[Condition.SimpleHeadFire] = "Обожженное горло",
			[Condition.SimpleTorsoPiercing] = "Колотая рана",
			[Condition.SimpleTorsoSlashing] = "Удар по ребрам",
			[Condition.SimpleTorsoBludgeoning] = "Удар под дых",
			[Condition.SimpleTorsoFire] = "Воспаление",
			[Condition.SimpleArmPiercing] = "Глубокая рана",
			[Condition.SimpleArmSlashing] = "Рубленая рана",
			[Condition.SimpleArmBludgeoning] = "Вывих",
			[Condition.SimpleArmFire] = "Болезненный ожог",
			[Condition.SimpleLegPiercing] = "Глубокая рана",
			[Condition.SimpleLegSlashing] = "Рубленая рана",
			[Condition.SimpleLegBludgeoning] = "Вывих",
			[Condition.SimpleLegFire] = "Болезненный ожог",
			[Condition.SimpleWingPiercing] = "Глубокая рана",
			[Condition.SimpleWingSlashing] = "Рубленая рана",
			[Condition.SimpleWingBludgeoning] = "Вывих",
			[Condition.SimpleWingFire] = "Болезненный ожог",
			[Condition.SimpleTailPiercing] = "Глубокая рана",
			[Condition.SimpleTailSlashing] = "Рубленая рана",
			[Condition.SimpleTailBludgeoning] = "Вывих",
			[Condition.SimpleTailFire] = "Болезненный ожог",
			[Condition.MediumHeadPiercing] = "Удар в лицо",
			[Condition.MediumHeadSlashing] = "Трещина в черепе",
			[Condition.MediumHeadBludgeoning] = "Сотрясение мозга",
			[Condition.MediumHeadFire] = "Обожженное лицо",
			[Condition.MediumTorsoPiercing] = "Внутреннее кровотечение",
			[Condition.MediumTorsoSlashing] = "Рваная рана",
			[Condition.MediumTorsoBludgeoning] = "Сломанные ребра",
			[Condition.MediumTorsoFire] = "Незаживающие раны",
			[Condition.MediumArmPiercing] = "Сквозное ранение",
			[Condition.MediumArmSlashing] = "Рассечение до кости",
			[Condition.MediumArmBludgeoning] = "Треснувшая кость",
			[Condition.MediumArmFire] = "Обширный ожог",
			[Condition.MediumLegPiercing] = "Сквозное ранение",
			[Condition.MediumLegSlashing] = "Рассечение до кости",
			[Condition.MediumLegBludgeoning] = "Треснувшая кость",
			[Condition.MediumLegFire] = "Обширный ожог",
			[Condition.MediumWingPiercing] = "Сквозное ранение",
			[Condition.MediumWingSlashing] = "Рассечение до кости",
			[Condition.MediumWingBludgeoning] = "Треснувшая кость",
			[Condition.MediumWingFire] = "Обширный ожог",
			[Condition.MediumTailPiercing] = "Сквозное ранение",
			[Condition.MediumTailSlashing] = "Рассечение до кости",
			[Condition.MediumTailBludgeoning] = "Треснувшая кость",
			[Condition.MediumTailFire] = "Обширный ожог",
			[Condition.DifficultHeadPiercing] = "Пробитый череп",
			[Condition.DifficultHeadSlashing] = "Рана в горло",
			[Condition.DifficultHeadBludgeoning] = "Контузия",
			[Condition.DifficultHeadFire] = "Ужасный ожог",
			[Condition.DifficultTorsoPiercing] = "Проникающее ранение",
			[Condition.DifficultTorsoSlashing] = "Распоротый живот",
			[Condition.DifficultTorsoBludgeoning] = "Шок",
			[Condition.DifficultTorsoFire] = "Сепсис",
			[Condition.DifficultArmPiercing] = "Повреждение связок",
			[Condition.DifficultArmSlashing] = "Перелом",
			[Condition.DifficultArmBludgeoning] = "Раздробленная конечность",
			[Condition.DifficultArmFire] = "Глубокий ожог",
			[Condition.DifficultLegPiercing] = "Повреждение связок",
			[Condition.DifficultLegSlashing] = "Перелом",
			[Condition.DifficultLegBludgeoning] = "Раздробленная конечность",
			[Condition.DifficultLegFire] = "Глубокий ожог",
			[Condition.DifficultWingPiercing] = "Повреждение связок",
			[Condition.DifficultWingSlashing] = "Перелом",
			[Condition.DifficultWingBludgeoning] = "Раздробленная конечность",
			[Condition.DifficultWingFire] = "Глубокий ожог",
			[Condition.DifficultTailPiercing] = "Повреждение связок",
			[Condition.DifficultTailSlashing] = "Перелом",
			[Condition.DifficultTailBludgeoning] = "Раздробленная конечность",
			[Condition.DifficultTailFire] = "Глубокий ожог",
		};
	}
}
