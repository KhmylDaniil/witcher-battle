using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Models.Dto
{
	public class AbilityDto : BaseDto
	{
		public string Name { get; set; }

		public Skill AttackSkill { get; set; }

		public int AttacksPerTurn { get; set; }

		public int DamageDiceCount { get; set; }

		public int DamageModifier { get; set; }

		public DamageType DamageType { get; set; }

		public List<AbilityAppliedConditionDto> AppliedConditions { get; set; } = [];

		public List<AbilityDefensiveSkillDto> DefensiveSkills { get; set; } = [];
	}
}
