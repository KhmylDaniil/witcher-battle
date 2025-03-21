using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Models.Requests
{
	public class AddOrUpdateCharacterSkillRequest : BaseRequest
	{
		public long CharacterId { get; set; }

		public Skill Skill { get; set; }

		public int Value { get; set; }
	}
}
