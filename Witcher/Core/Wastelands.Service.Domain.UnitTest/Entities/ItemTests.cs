using FluentAssertions;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Domain.UnitTest.TestSupport;

namespace Wastelands.Service.Domain.UnitTest.Entities
{
	[TestClass]
	public class ItemTests
	{
		[TestMethod]
		public void Constructor_SnapshotsWeaponFieldsFromTemplate()
		{
			var template = TestBuilders.MeleeWeaponTemplate(name: "Sword", damageModifier: 3, durability: 5);

			var item = TestBuilders.Item(characterId: 1, template);

			item.Name.Should().Be("Sword");
			item.WeaponKind.Should().Be(WeaponKind.Melee);
			item.DamageModifier.Should().Be(3);
			item.Durability.Should().Be(5);
			item.IsEquipped.Should().BeFalse();
		}

		[TestMethod]
		public void Equip_ThenEquipAgain_Throws()
		{
			var item = TestBuilders.Item(1, TestBuilders.MeleeWeaponTemplate());
			item.Equip();

			var act = item.Equip;

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void Unequip_WhenNotEquipped_Throws()
		{
			var item = TestBuilders.Item(1, TestBuilders.MeleeWeaponTemplate());

			var act = item.Unequip;

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void Unequip_AfterEquip_TogglesBack()
		{
			var item = TestBuilders.Item(1, TestBuilders.MeleeWeaponTemplate());
			item.Equip();

			item.Unequip();

			item.IsEquipped.Should().BeFalse();
		}

		[TestMethod]
		public void RepairWeapon_OnNonWeaponItem_Throws()
		{
			var item = TestBuilders.Item(1, TestBuilders.ArmorTemplate());

			var act = () => item.RepairWeapon(5);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void RepairWeapon_SetsDurabilityDirectly()
		{
			var item = TestBuilders.Item(1, TestBuilders.MeleeWeaponTemplate(durability: 5));
			item.WearWeapon();
			item.WearWeapon();

			item.RepairWeapon(5);

			item.Durability.Should().Be(5);
		}

		[TestMethod]
		public void WearWeapon_OnNonWeaponItem_Throws()
		{
			var item = TestBuilders.Item(1, TestBuilders.ArmorTemplate());

			var act = item.WearWeapon;

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void WearWeapon_DecrementsDurabilityByOne()
		{
			var item = TestBuilders.Item(1, TestBuilders.MeleeWeaponTemplate(durability: 5));

			item.WearWeapon();

			item.Durability.Should().Be(4);
		}

		[TestMethod]
		public void WearWeapon_NeverGoesBelowZero()
		{
			var item = TestBuilders.Item(1, TestBuilders.MeleeWeaponTemplate(durability: 1));

			item.WearWeapon();
			item.WearWeapon();
			item.WearWeapon();

			item.Durability.Should().Be(0);
		}

		[TestMethod]
		public void WearArmor_OnNonArmorItem_Throws()
		{
			var item = TestBuilders.Item(1, TestBuilders.MeleeWeaponTemplate());

			var act = () => item.WearArmor(HumanBodyPart.Torso);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void WearArmor_OnPartNotCoveredByTemplate_Throws()
		{
			var template = TestBuilders.ArmorTemplate().WithId(1);
			template.AddArmorPart(HumanBodyPart.Torso, armor: 3);
			var item = TestBuilders.Item(1, template);

			var act = () => item.WearArmor(HumanBodyPart.Head);

			act.Should().Throw<NotFoundException>();
		}

		[TestMethod]
		public void WearArmor_OnCoveredPart_ReducesThatPartsDurabilityByOne()
		{
			var template = TestBuilders.ArmorTemplate().WithId(1);
			template.AddArmorPart(HumanBodyPart.Torso, armor: 3);
			var item = TestBuilders.Item(1, template);

			item.WearArmor(HumanBodyPart.Torso);

			item.ArmorParts.Single(p => p.Part == HumanBodyPart.Torso).CurrentDurability.Should().Be(2);
		}
	}
}
