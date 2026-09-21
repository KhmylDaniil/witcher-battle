using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	/// <summary>
	/// Шаблон предмета — создаётся мастером для игры, доступен только ему (см. GM-скоуп
	/// ItemTemplateRepository). Оружейные поля заполнены только когда <see cref="ItemType"/> ==
	/// Weapon; для остальных типов (Armor/Other) это пока просто название/описание/вес/стоимость —
	/// боевого поведения у них нет.
	/// </summary>
	public class ItemTemplate : Entity
	{
		public long GameId { get; private set; }

		public string Name { get; private set; }

		public string? Description { get; private set; }

		public ItemType ItemType { get; private set; }

		public int Weight { get; private set; }

		public int Cost { get; private set; }

		public Skill? AttackSkill { get; private set; }

		/// <summary>Возможна ли мультиатака (быстрая/сильная атака при экипировке) — см. CharacterItemService.EquipAsync.</summary>
		public bool? IsMultiAttack { get; private set; }

		public int? DamageDiceCount { get; private set; }

		public int? DamageModifier { get; private set; }

		public DamageType? DamageType { get; private set; }

		public WeaponKind? WeaponKind { get; private set; }

		public int? AttackRange { get; private set; }

		public int? HandsRequired { get; private set; }

		public int? Durability { get; private set; }

		public List<ItemTemplateAppliedCondition> AppliedConditions { get; private set; } = [];

		/// <summary>Заполнены только когда ItemType == Armor.</summary>
		public List<ItemTemplateArmorPart> ArmorParts { get; private set; } = [];

		public Dictionary<DamageType, DamageTypeModifier> DamageTypeModifiers { get; private set; } = [];

		private ItemTemplate()
		{
		}

		private ItemTemplate(long gameId, string name, string? description, ItemType itemType, int weight, int cost)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(gameId, nameof(gameId));
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
			InvalidArgumentException.ThrowIfLessThanZero(weight, nameof(weight));
			InvalidArgumentException.ThrowIfLessThanZero(cost, nameof(cost));

			GameId = gameId;
			Name = name;
			Description = description;
			ItemType = itemType;
			Weight = weight;
			Cost = cost;
		}

		public static ItemTemplate CreateWeapon(
			long gameId,
			string name,
			string? description,
			int weight,
			int cost,
			Skill attackSkill,
			bool isMultiAttack,
			int damageDiceCount,
			int damageModifier,
			DamageType damageType,
			WeaponKind weaponKind,
			int attackRange,
			int handsRequired,
			int durability)
		{
			var template = new ItemTemplate(gameId, name, description, ItemType.Weapon, weight, cost);
			template.SetWeaponFields(
				attackSkill, isMultiAttack, damageDiceCount, damageModifier, damageType, weaponKind, attackRange, handsRequired, durability);

			return template;
		}

		public static ItemTemplate CreateNonWeapon(long gameId, string name, string? description, ItemType itemType, int weight, int cost)
			=> new(gameId, name, description, itemType, weight, cost);

		public void ChangeWeapon(
			string name,
			string? description,
			int weight,
			int cost,
			Skill attackSkill,
			bool isMultiAttack,
			int damageDiceCount,
			int damageModifier,
			DamageType damageType,
			WeaponKind weaponKind,
			int attackRange,
			int handsRequired,
			int durability)
		{
			ChangeBaseFields(name, description, weight, cost);
			SetWeaponFields(
				attackSkill, isMultiAttack, damageDiceCount, damageModifier, damageType, weaponKind, attackRange, handsRequired, durability);
		}

		public void ChangeNonWeapon(string name, string? description, int weight, int cost)
			=> ChangeBaseFields(name, description, weight, cost);

		private void ChangeBaseFields(string name, string? description, int weight, int cost)
		{
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
			InvalidArgumentException.ThrowIfLessThanZero(weight, nameof(weight));
			InvalidArgumentException.ThrowIfLessThanZero(cost, nameof(cost));

			Name = name;
			Description = description;
			Weight = weight;
			Cost = cost;
		}

		private void SetWeaponFields(
			Skill attackSkill,
			bool isMultiAttack,
			int damageDiceCount,
			int damageModifier,
			DamageType damageType,
			WeaponKind weaponKind,
			int attackRange,
			int handsRequired,
			int durability)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(damageDiceCount, nameof(damageDiceCount));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(attackRange, nameof(attackRange));
			InvalidArgumentException.ThrowIfNotInRange(handsRequired, 1, 2, nameof(handsRequired));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(durability, nameof(durability));

			AttackSkill = attackSkill;
			IsMultiAttack = isMultiAttack;
			DamageDiceCount = damageDiceCount;
			DamageModifier = damageModifier;
			DamageType = damageType;
			WeaponKind = weaponKind;
			AttackRange = attackRange;
			HandsRequired = handsRequired;
			Durability = durability;
		}

		public ItemTemplateArmorPart AddArmorPart(HumanBodyPart part, int armor)
		{
			if (ArmorParts.Any(p => p.Part == part))
			{
				throw new InvalidArgumentException(ErrorCode.ItemTemplateArmorPartAlreadyExisted, "Эта часть тела уже покрыта в шаблоне брони.");
			}

			var armorPart = new ItemTemplateArmorPart(Id, part, armor);
			ArmorParts.Add(armorPart);

			return armorPart;
		}
	}
}
