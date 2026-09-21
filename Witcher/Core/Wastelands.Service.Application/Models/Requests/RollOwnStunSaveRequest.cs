namespace Wastelands.Service.Application.Models.Requests
{
	public class RollOwnStunSaveRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public int? Roll { get; set; }
	}
}
