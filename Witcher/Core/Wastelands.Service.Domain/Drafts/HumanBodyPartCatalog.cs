using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Drafts
{
	public readonly record struct HumanBodyPartInfo(string Name, int HitPenalty, double DamageModifier, int MinToHit, int MaxToHit);

	/// <summary>
	/// Геометрия удара по частям тела персонажа-человека — те же значения, что
	/// <see cref="DefaultHumanBodyTemplatePartsDraft"/> использует для стартового BodyTemplate,
	/// но зафиксированные раз и навсегда (персонажи не редактируют свою анатомию). Используется
	/// боевым расчётом (BattleCombatCalculator) для попадания/брони персонажа-защитника.
	/// </summary>
	public static class HumanBodyPartCatalog
	{
		public static readonly IReadOnlyDictionary<HumanBodyPart, HumanBodyPartInfo> Parts = new Dictionary<HumanBodyPart, HumanBodyPartInfo>
		{
			[HumanBodyPart.Head] = new("Голова", HitPenalty: 4, DamageModifier: 3, MinToHit: 1, MaxToHit: 1),
			[HumanBodyPart.Torso] = new("Торс", HitPenalty: 1, DamageModifier: 1, MinToHit: 2, MaxToHit: 4),
			[HumanBodyPart.RightArm] = new("Правая рука", HitPenalty: 3, DamageModifier: 0.5, MinToHit: 5, MaxToHit: 5),
			[HumanBodyPart.LeftArm] = new("Левая рука", HitPenalty: 3, DamageModifier: 0.5, MinToHit: 6, MaxToHit: 6),
			[HumanBodyPart.RightLeg] = new("Правая нога", HitPenalty: 2, DamageModifier: 0.5, MinToHit: 7, MaxToHit: 8),
			[HumanBodyPart.LeftLeg] = new("Левая нога", HitPenalty: 2, DamageModifier: 0.5, MinToHit: 9, MaxToHit: 10),
		};

		public static HumanBodyPartInfo Get(HumanBodyPart part) => Parts[part];

		/// <summary>Часть тела по броску d10, аналогично распределению MinToHit..MaxToHit у BodyTemplatePart.</summary>
		public static HumanBodyPart ResolveByRoll(int roll) => Parts.First(kv => roll >= kv.Value.MinToHit && roll <= kv.Value.MaxToHit).Key;
	}
}
