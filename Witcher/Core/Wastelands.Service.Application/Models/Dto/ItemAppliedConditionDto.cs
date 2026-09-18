using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Dto
{
	public class ItemAppliedConditionDto : BaseDto
	{
		public Condition Condition { get; set; }

		public int ApplyChance { get; set; }
	}
}
