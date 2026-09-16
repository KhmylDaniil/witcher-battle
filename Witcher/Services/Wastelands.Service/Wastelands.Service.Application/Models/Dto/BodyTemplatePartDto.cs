using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Dto
{
	public class BodyTemplatePartDto : BaseDto
	{
		public string Name { get; set; }

		public BodyPartType BodyPartType { get; set; }

		public double DamageModifier { get; set; }

		public int HitPenalty { get; set; }

		public int MinToHit { get; set; }

		public int MaxToHit { get; set; }
	}
}
