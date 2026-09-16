using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Models.Requests
{
	public class UpdateAbilityRequest : BaseRequest
	{
		public long CreatureTemplateId { get; set; }

		public long AbilityId { get; set; }

		public string Name { get; set; }

		public Skill AttackSkill { get; set; }

		public int AttacksPerTurn { get; set; }

		public int DamageDiceCount { get; set; }

		public int DamageModifier { get; set; }

		public DamageType DamageType { get; set; }
	}
}
