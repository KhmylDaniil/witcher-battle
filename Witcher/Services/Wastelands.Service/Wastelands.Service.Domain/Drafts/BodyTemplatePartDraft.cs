using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Drafts
{
	public class BodyTemplatePartDraft
	{
		public string Name { get; init; }

		public BodyPartType BodyPartType { get; init; }

		public double DamageModifier { get; init; }

		public int HitPenalty { get; init; }

		public int MinToHit { get; init; }

		public int MaxToHit { get; init; }
	}
}
