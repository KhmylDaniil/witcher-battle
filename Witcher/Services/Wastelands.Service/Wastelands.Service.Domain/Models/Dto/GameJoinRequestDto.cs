using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Models.Dto
{
	public class GameJoinRequestDto : BaseDto
	{
		public long UserId { get; set; }

		public long GameId { get; set; }

		public GameJoinRequestStatus Status { get; set; }
	}
}
