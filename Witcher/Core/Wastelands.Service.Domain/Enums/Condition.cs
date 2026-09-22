namespace Wastelands.Service.Domain.Enums
{
	public enum Condition
	{
		Bleed,
		Poison,
		Fire,
		Stun,
		Staggered,
		Sufflocation,
		Blinded,
		Dying,

		// Критические ранения (новая система) — Simple/Medium/Difficult (excess атаки над защитой
		// >=7/>=10/>=13) x часть тела x тип урона. Добавлены в конец, чтобы не сдвинуть числовые
		// значения уже сохранённых условий выше (список хранится в БД как jsonb-массив чисел) — см.
		// CriticalWoundCatalog.
		SimpleHeadPiercing, SimpleHeadSlashing, SimpleHeadBludgeoning, SimpleHeadFire,
		SimpleTorsoPiercing, SimpleTorsoSlashing, SimpleTorsoBludgeoning, SimpleTorsoFire,
		SimpleArmPiercing, SimpleArmSlashing, SimpleArmBludgeoning, SimpleArmFire,
		SimpleLegPiercing, SimpleLegSlashing, SimpleLegBludgeoning, SimpleLegFire,
		SimpleWingPiercing, SimpleWingSlashing, SimpleWingBludgeoning, SimpleWingFire,
		SimpleTailPiercing, SimpleTailSlashing, SimpleTailBludgeoning, SimpleTailFire,

		MediumHeadPiercing, MediumHeadSlashing, MediumHeadBludgeoning, MediumHeadFire,
		MediumTorsoPiercing, MediumTorsoSlashing, MediumTorsoBludgeoning, MediumTorsoFire,
		MediumArmPiercing, MediumArmSlashing, MediumArmBludgeoning, MediumArmFire,
		MediumLegPiercing, MediumLegSlashing, MediumLegBludgeoning, MediumLegFire,
		MediumWingPiercing, MediumWingSlashing, MediumWingBludgeoning, MediumWingFire,
		MediumTailPiercing, MediumTailSlashing, MediumTailBludgeoning, MediumTailFire,

		DifficultHeadPiercing, DifficultHeadSlashing, DifficultHeadBludgeoning, DifficultHeadFire,
		DifficultTorsoPiercing, DifficultTorsoSlashing, DifficultTorsoBludgeoning, DifficultTorsoFire,
		DifficultArmPiercing, DifficultArmSlashing, DifficultArmBludgeoning, DifficultArmFire,
		DifficultLegPiercing, DifficultLegSlashing, DifficultLegBludgeoning, DifficultLegFire,
		DifficultWingPiercing, DifficultWingSlashing, DifficultWingBludgeoning, DifficultWingFire,
		DifficultTailPiercing, DifficultTailSlashing, DifficultTailBludgeoning, DifficultTailFire,

		// Добавлено после первоначального набора — в конец, чтобы не сдвинуть числовые значения уже
		// сохранённых условий выше (см. комментарий про jsonb-массив чисел).
		Prone,
	}
}
