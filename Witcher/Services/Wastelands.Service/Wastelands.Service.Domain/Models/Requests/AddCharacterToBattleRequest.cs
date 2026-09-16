namespace Wastelands.Service.Domain.Models.Requests
{
	public class AddCharacterToBattleRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public long CharacterId { get; set; }
	}
}
