using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	/// <summary>
	/// Экземпляр предмета в инвентаре персонажа — снапшот-копия ItemTemplate в момент добавления
	/// (тот же приём, что Creature делает с CreatureTemplate): дальнейшее редактирование шаблона на
	/// уже созданные экземпляры не влияет.
	/// </summary>
	public class Item : Entity
	{
		public long CharacterId { get; private set; }

		public long ItemTemplateId { get; private set; }

		public string Name { get; private set; }

		public string? Description { get; private set; }

		public ItemType ItemType { get; private set; }

		public double Weight { get; private set; }

		public int Cost { get; private set; }

		public Skill? AttackSkill { get; private set; }

		public bool? IsMultiAttack { get; private set; }

		public int? DamageDiceCount { get; private set; }

		public int? DamageModifier { get; private set; }

		public DamageType? DamageType { get; private set; }

		public WeaponKind? WeaponKind { get; private set; }

		public int? AttackRange { get; private set; }

		public int? HandsRequired { get; private set; }

		public int? Durability { get; private set; }

		public List<ItemAppliedCondition> AppliedConditions { get; private set; } = [];

		public bool IsEquipped { get; private set; }

		private Item()
		{
		}

		public Item(long characterId, ItemTemplate template)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(characterId, nameof(characterId));
			InvalidArgumentException.ThrowIfNull(template, nameof(template));

			CharacterId = characterId;
			ItemTemplateId = template.Id;
			Name = template.Name;
			Description = template.Description;
			ItemType = template.ItemType;
			Weight = template.Weight;
			Cost = template.Cost;
			AttackSkill = template.AttackSkill;
			IsMultiAttack = template.IsMultiAttack;
			DamageDiceCount = template.DamageDiceCount;
			DamageModifier = template.DamageModifier;
			DamageType = template.DamageType;
			WeaponKind = template.WeaponKind;
			AttackRange = template.AttackRange;
			HandsRequired = template.HandsRequired;
			Durability = template.Durability;
			AppliedConditions = template.AppliedConditions.Select(c => new ItemAppliedCondition(c.Condition, c.ApplyChance)).ToList();
		}

		public void Equip()
		{
			if (IsEquipped)
			{
				throw new InvalidArgumentException(ErrorCode.ItemAlreadyEquipped, "Предмет уже экипирован.");
			}

			IsEquipped = true;
		}

		public void Unequip()
		{
			if (!IsEquipped)
			{
				throw new InvalidArgumentException(ErrorCode.ItemNotEquipped, "Предмет не экипирован.");
			}

			IsEquipped = false;
		}
	}
}
