namespace Wastelands.Service.Domain.Models.Requests
{
	public class AddCreatureToBattleRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public long CreatureTemplateId { get; set; }

		public string? Name { get; set; }
	}
}
