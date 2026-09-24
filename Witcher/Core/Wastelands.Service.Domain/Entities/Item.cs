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

		public int Weight { get; private set; }

		public int Cost { get; private set; }

		public Skill? AttackSkill { get; private set; }

		public bool? IsMultiAttack { get; private set; }

		public int? DamageDiceCount { get; private set; }

		public int? AttackModifier { get; private set; }

		public int? DamageModifier { get; private set; }

		public DamageType? DamageType { get; private set; }

		public WeaponKind? WeaponKind { get; private set; }

		public int? AttackRange { get; private set; }

		public int? HandsRequired { get; private set; }

		public int? Durability { get; private set; }

		public List<ItemAppliedCondition> AppliedConditions { get; private set; } = [];

		/// <summary>Заполнены только когда ItemType == Armor — снапшот ItemTemplate.ArmorParts.</summary>
		public List<ItemArmorPart> ArmorParts { get; private set; } = [];

		public Dictionary<DamageType, DamageTypeModifier> DamageTypeModifiers { get; private set; } = [];

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
			AttackModifier = template.AttackModifier;
			DamageModifier = template.DamageModifier;
			DamageType = template.DamageType;
			WeaponKind = template.WeaponKind;
			AttackRange = template.AttackRange;
			HandsRequired = template.HandsRequired;
			Durability = template.Durability;
			AppliedConditions = template.AppliedConditions.Select(c => new ItemAppliedCondition(c.Condition, c.ApplyChance)).ToList();
			ArmorParts = template.ArmorParts.Select(p => new ItemArmorPart(p.Part, p.Armor)).ToList();
			DamageTypeModifiers = new Dictionary<DamageType, DamageTypeModifier>(template.DamageTypeModifiers);
		}

		public void Equip()
		{
			if (IsEquipped)
			{
				throw new InvalidArgumentException(ErrorCode.ItemAlreadyEquipped, "Предмет уже экипирован.");
			}

			if (ItemType == ItemType.Weapon && Durability <= 0)
			{
				throw new InvalidArgumentException(
					ErrorCode.WeaponDurabilityDepleted, "У оружия нулевая прочность — сначала отремонтируйте его.");
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

		public void RepairWeapon(int durability)
		{
			if (ItemType != ItemType.Weapon)
			{
				throw new InvalidArgumentException(ErrorCode.ItemNotWeapon, "Это не оружие.");
			}

			InvalidArgumentException.ThrowIfLessThanZero(durability, nameof(durability));
			Durability = durability;
		}

		public void RepairArmorPart(HumanBodyPart part, int durability)
		{
			var armorPart = GetArmorPart(part);
			armorPart.Repair(durability);
		}

		/// <summary>Износ от попадания в часть тела, покрытую этой бронёй — см. BattleCombatService.ContinueDamageAsync.</summary>
		public void WearArmor(HumanBodyPart part)
		{
			var armorPart = GetArmorPart(part);
			armorPart.Wear();
		}

		/// <summary>
		/// Износ от блокирования удара этим оружием в бою (amount=1 — см. BattleCombatService.
		/// ConfirmDefenderAsync) или от критического провала (переменный бросок — см.
		/// BattleFumbleResolver). Экипировку при падении прочности до нуля снимает вызывающий код
		/// (нужен доступ к Character.Abilities, которого у Item нет) — см. CharacterItemService.
		/// UnequipIfBroken.
		/// </summary>
		public void WearWeapon(int amount = 1)
		{
			if (ItemType != ItemType.Weapon)
			{
				throw new InvalidArgumentException(ErrorCode.ItemNotWeapon, "Это не оружие.");
			}

			Durability = Math.Max(0, (Durability ?? 0) - amount);
		}

		private ItemArmorPart GetArmorPart(HumanBodyPart part)
		{
			if (ItemType != ItemType.Armor)
			{
				throw new InvalidArgumentException(ErrorCode.ItemNotArmor, "Это не броня.");
			}

			var armorPart = ArmorParts.FirstOrDefault(p => p.Part == part);
			NotFoundException.ThrowIfNull(armorPart, ErrorCode.ItemArmorPartNotFound, nameof(ItemArmorPart), nameof(ItemArmorPart.Part), part.ToString());

			return armorPart;
		}
	}
}
