namespace Wastelands.Service.Application.Models.Requests
{
	public class SetAttackerChoicesRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public long? TargetedCreaturePartId { get; set; }

		public int? AttackRoll { get; set; }
	}
}
