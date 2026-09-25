namespace Wastelands.Service.Application.Models.Requests
{
	public class SetBattleMapMarkerRequest : BaseRequest
	{
		public long BattleMapId { get; set; }

		public int Column { get; set; }

		public int Row { get; set; }

		public string Text { get; set; }
	}
}
