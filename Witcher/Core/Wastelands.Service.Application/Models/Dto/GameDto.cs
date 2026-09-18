using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Dto
{
	public class GameDto : BaseDto
	{
		public string Name { get; set; }

		public long CreatedByUserId { get; set; }

		public GameMembershipStatus MembershipStatus { get; set; }
	}
}
