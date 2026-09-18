using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class DeleteCreatureTemplateSkillRequest : BaseDeleteRequest
	{
		public Skill Skill { get; set; }
	}
}
