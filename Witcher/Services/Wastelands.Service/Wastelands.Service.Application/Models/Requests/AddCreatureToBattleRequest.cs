namespace Wastelands.Service.Application.Models.Requests
{
	public class AddCreatureToBattleRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public long CreatureTemplateId { get; set; }

		public string? Name { get; set; }
	}
}
