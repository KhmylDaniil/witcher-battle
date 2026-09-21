using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class SetAttackerChoicesRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public long? TargetedCreaturePartId { get; set; }

		public HumanBodyPart? TargetedHumanBodyPart { get; set; }

		public int? AttackRoll { get; set; }
	}
}
