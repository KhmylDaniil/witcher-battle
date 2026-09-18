using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class StartAttackRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public long AbilityId { get; set; }

		public ParticipantKind DefenderKind { get; set; }

		public long DefenderId { get; set; }
	}
}
