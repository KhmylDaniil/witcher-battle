using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Models.Requests
{
	public class AddOrUpdateCreatureTemplateSkillRequest : BaseRequest
	{
		public long CreatureTemplateId { get; set; }

		public Skill Skill { get; set; }

		public int Value { get; set; }
	}
}
