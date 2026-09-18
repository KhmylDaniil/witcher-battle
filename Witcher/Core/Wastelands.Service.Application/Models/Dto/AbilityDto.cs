using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Dto
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

		/// <summary>Сгенерирована экипировкой оружия — редактируется/удаляется только через снятие предмета.</summary>
		public bool IsFromEquippedWeapon { get; set; }
	}
}
