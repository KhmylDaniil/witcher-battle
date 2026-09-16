namespace Wastelands.Service.Application.Models.Requests
{
	public class CreateBattleRequest : BaseRequest
	{
		public long GameId { get; set; }

		public string Name { get; set; }
	}
}
