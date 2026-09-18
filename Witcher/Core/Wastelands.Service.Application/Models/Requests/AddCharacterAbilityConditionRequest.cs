using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class AddCharacterAbilityConditionRequest : BaseRequest
	{
		public long CharacterId { get; set; }

		public long AbilityId { get; set; }

		public Condition Condition { get; set; }

		public int ApplyChance { get; set; } = 25;
	}
}
