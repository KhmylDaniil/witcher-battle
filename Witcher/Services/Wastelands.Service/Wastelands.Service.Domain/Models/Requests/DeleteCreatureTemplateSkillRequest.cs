using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Models.Requests
{
	public class DeleteCreatureTemplateSkillRequest : BaseDeleteRequest
	{
		public Skill Skill { get; set; }
	}
}
