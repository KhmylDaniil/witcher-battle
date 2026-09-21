using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class UpdateItemTemplateRequest : BaseRequest
	{
		public long Id { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }

		public ItemType ItemType { get; set; }

		public int Weight { get; set; }

		public int Cost { get; set; }

		// Заполняются только когда ItemType == Weapon — см. UpdateItemTemplateRequestValidator.
		public Skill? AttackSkill { get; set; }

		public bool? IsMultiAttack { get; set; }

		public int? DamageDiceCount { get; set; }

		public int? DamageModifier { get; set; }

		public DamageType? DamageType { get; set; }

		public WeaponKind? WeaponKind { get; set; }

		public int? AttackRange { get; set; }

		public int? HandsRequired { get; set; }

		public int? Durability { get; set; }
	}
}
