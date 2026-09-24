namespace Wastelands.Service.Application.Models.Requests
{
	public class RollDyingSaveRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public int? Roll { get; set; }
	}
}
