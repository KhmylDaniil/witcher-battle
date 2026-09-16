namespace Wastelands.Service.Application.Models.Requests
{
	public class UpdateBattleCreatureRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public long CreatureId { get; set; }

		public string Name { get; set; }

		public int CurrentHP { get; set; }

		public int CurrentSta { get; set; }
	}
}
