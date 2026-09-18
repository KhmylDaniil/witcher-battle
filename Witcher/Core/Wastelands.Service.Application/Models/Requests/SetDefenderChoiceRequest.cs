using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class SetDefenderChoiceRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public Skill DefensiveSkill { get; set; }

		public int? DefenseRoll { get; set; }
	}
}
