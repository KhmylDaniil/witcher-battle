using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class CreateAbilityRequest : BaseRequest
	{
		public long CreatureTemplateId { get; set; }

		public string Name { get; set; }

		public Skill AttackSkill { get; set; }

		public int AttacksPerTurn { get; set; }

		public int DamageDiceCount { get; set; }

		public int AttackModifier { get; set; }

		public int DamageModifier { get; set; }

		public DamageType DamageType { get; set; }
	}
}
