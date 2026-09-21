namespace Wastelands.Service.Domain.Enums
{
	public enum Condition
	{
		Bleed,
		BleedingWound,
		Poison,
		Fire,
		Freeze,
		Stun,
		Staggered,
		Intoxication,
		Hallutination,
		Nausea,
		Sufflocation,
		Blinded,
		Dying,

		SimpleLeg,
		SimpleArm,
		SimpleWing,
		SimpleTail,
		SimpleHead1,
		SimpleHead2,
		SimpleTorso1,
		SimpleTorso2,

		ComplexLeg,
		ComplexArm,
		ComplexWing,
		ComplexTail,
		ComplexHead1,
		ComplexHead2,
		ComplexTorso1,
		ComplexTorso2,

		DifficultLeg,
		DifficultArm,
		DifficultWing,
		DifficultTail,
		DifficultHead1,
		DifficultHead2,
		DifficultTorso1,
		DifficultTorso2,

		DeadlyLeg,
		DeadlyArm,
		DeadlyWing,
		DeadlyTail,
		DeadlyHead1,
		DeadlyHead2,
		DeadlyTorso1,
		DeadlyTorso2,

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
	}
}
