using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Dto
{
	public class ItemDto : BaseDto
	{
		public long ItemTemplateId { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }

		public ItemType ItemType { get; set; }

		public double Weight { get; set; }

		public int Cost { get; set; }

		public Skill? AttackSkill { get; set; }

		public int? AttacksPerTurn { get; set; }

		public int? DamageDiceCount { get; set; }

		public int? DamageModifier { get; set; }

		public DamageType? DamageType { get; set; }

		public WeaponKind? WeaponKind { get; set; }

		public int? AttackRange { get; set; }

		public int? HandsRequired { get; set; }

		public int? Durability { get; set; }

		public List<ItemAppliedConditionDto> AppliedConditions { get; set; } = [];

		public bool IsEquipped { get; set; }
	}
}
