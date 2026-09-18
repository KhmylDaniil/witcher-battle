using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class AddCharacterAbilityDefensiveSkillRequest : BaseRequest
	{
		public long CharacterId { get; set; }

		public long AbilityId { get; set; }

		public Skill Skill { get; set; } = Skill.Dodge;
	}
}
