using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Models.Requests
{
	public class AddAbilityConditionRequest : BaseRequest
	{
		public long CreatureTemplateId { get; set; }

		public long AbilityId { get; set; }

		public Condition Condition { get; set; }

		public int ApplyChance { get; set; } = 25;
	}
}
