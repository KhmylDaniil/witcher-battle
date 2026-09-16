namespace Wastelands.Service.Application.Models.Requests
{
	public class AddCharacterToBattleRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public long CharacterId { get; set; }
	}
}
