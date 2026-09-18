namespace Wastelands.Service.Application.Models.Requests
{
	public class SetDamageRollRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public int? DamageRoll { get; set; }
	}
}
