using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Models.Requests
{
	public class UpdateAbilityConditionRequest : BaseRequest
	{
		public long CreatureTemplateId { get; set; }

		public long AbilityId { get; set; }

		public long ConditionId { get; set; }

		public Condition Condition { get; set; }

		public int ApplyChance { get; set; } = 25;
	}
}
