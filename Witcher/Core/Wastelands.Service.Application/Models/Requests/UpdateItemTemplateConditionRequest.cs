using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class UpdateItemTemplateConditionRequest : BaseRequest
	{
		public long ItemTemplateId { get; set; }

		public long ConditionId { get; set; }

		public Condition Condition { get; set; }

		public int ApplyChance { get; set; } = 25;
	}
}
