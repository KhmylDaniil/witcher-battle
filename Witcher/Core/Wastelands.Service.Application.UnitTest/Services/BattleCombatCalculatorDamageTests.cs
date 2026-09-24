using FluentAssertions;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Services;
using Wastelands.Service.Application.UnitTest.TestSupport;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.UnitTest.Services
{
	[TestClass]
	public class BattleCombatCalculatorDamageTests
	{
		[TestMethod]
		public void ValidateDamageRoll_BelowMinimum_Throws()
		{
			var ability = TestBuilders.CharacterAbility(damageDiceCount: 2);

			var act = () => BattleCombatCalculator.ValidateDamageRoll(1, ability);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void ValidateDamageRoll_AboveMaximum_Throws()
		{
			var ability = TestBuilders.CharacterAbility(damageDiceCount: 2);

			var act = () => BattleCombatCalculator.ValidateDamageRoll(13, ability);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void ValidateDamageRoll_WithinRange_DoesNotThrow()
		{
			var ability = TestBuilders.CharacterAbility(damageDiceCount: 2);

			var act = () => BattleCombatCalculator.ValidateDamageRoll(7, ability);

			act.Should().NotThrow();
		}

		[TestMethod]
		public void CalculateDamage_CreatureDefender_ArmorAbsorbsPartOfDamage()
		{
			var template = TestBuilders.CreatureTemplate();
			var torso = template.Parts[1].WithId(5); // DamageModifier = 1
			torso.UpdateArmor(3);
			var creature = TestBuilders.Creature(1, template);
			var ability = TestBuilders.CreatureAbility(damageDiceCount: 1, damageModifier: 1, damageType: DamageType.Slashing);
			var attackerContext = TestBuilders.Context();
			var defenderContext = TestBuilders.Context(template: template, creature: creature);
			var attack = BattleAttackBuilder.AwaitingDamageRoll(ParticipantKind.Creature, defenderId: 9, targetCreaturePartId: 5, targetHumanBodyPart: null, damageRoll: 10);

			var damage = BattleCombatCalculator.CalculateDamage(attackerContext, defenderContext, ParticipantKind.Creature, ability, attack);

			// raw = 10 + 1(abilityMod) = 11; armorBefore = 3; absorbed = 3; afterArmor = 8; ×1(partMod) = 8
			damage.RawDamage.Should().Be(11);
			damage.ArmorBeforeHit.Should().Be(3);
			damage.ArmorAbsorbed.Should().Be(3);
			damage.ArmorAfterHit.Should().Be(2);
			damage.FinalDamage.Should().Be(8);
			damage.PartName.Should().Be("Торс");
			damage.WornArmorItemId.Should().BeNull();
		}

		[TestMethod]
		public void CalculateDamage_CreatureDefender_DamageFullyAbsorbed_FinalDamageIsZero()
		{
			var template = TestBuilders.CreatureTemplate();
			var torso = template.Parts[1].WithId(5);
			torso.UpdateArmor(10);
			var creature = TestBuilders.Creature(1, template);
			var ability = TestBuilders.CreatureAbility(damageDiceCount: 1, damageModifier: 0);
			var attackerContext = TestBuilders.Context();
			var defenderContext = TestBuilders.Context(template: template, creature: creature);
			var attack = BattleAttackBuilder.AwaitingDamageRoll(ParticipantKind.Creature, 9, 5, null, damageRoll: 2);

			var damage = BattleCombatCalculator.CalculateDamage(attackerContext, defenderContext, ParticipantKind.Creature, ability, attack);

			damage.FinalDamage.Should().Be(0);
		}

		[TestMethod]
		public void CalculateDamage_CreatureDefender_ExistingArmorWear_ReducesEffectiveArmor()
		{
			var template = TestBuilders.CreatureTemplate();
			var torso = template.Parts[1].WithId(5);
			torso.UpdateArmor(3);
			var creature = TestBuilders.Creature(1, template);
			creature.WearArmor(5); // одно предыдущее попадание — эффективная броня 3-1=2
			var ability = TestBuilders.CreatureAbility(damageDiceCount: 1, damageModifier: 0);
			var attackerContext = TestBuilders.Context();
			var defenderContext = TestBuilders.Context(template: template, creature: creature);
			var attack = BattleAttackBuilder.AwaitingDamageRoll(ParticipantKind.Creature, 9, 5, null, damageRoll: 10);

			var damage = BattleCombatCalculator.CalculateDamage(attackerContext, defenderContext, ParticipantKind.Creature, ability, attack);

			damage.ArmorBeforeHit.Should().Be(2);
		}

		[TestMethod]
		public void CalculateDamage_CreatureDefender_VulnerabilityDoublesDamage()
		{
			var template = TestBuilders.CreatureTemplate();
			var torso = template.Parts[1].WithId(5);
			template.DamageTypeModifiers[DamageType.Slashing] = DamageTypeModifier.Vulnerability;
			var creature = TestBuilders.Creature(1, template);
			var ability = TestBuilders.CreatureAbility(damageDiceCount: 1, damageModifier: 0, damageType: DamageType.Slashing);
			var attackerContext = TestBuilders.Context();
			var defenderContext = TestBuilders.Context(template: template, creature: creature);
			var attack = BattleAttackBuilder.AwaitingDamageRoll(ParticipantKind.Creature, 9, 5, null, damageRoll: 4);

			var damage = BattleCombatCalculator.CalculateDamage(attackerContext, defenderContext, ParticipantKind.Creature, ability, attack);

			// raw=4, no armor, ×1(partMod) = 4, ×2(vulnerability) = 8
			damage.FinalDamage.Should().Be(8);
		}

		[TestMethod]
		public void CalculateDamage_CreatureDefender_ImmunityZeroesDamage()
		{
			var template = TestBuilders.CreatureTemplate();
			template.Parts[1].WithId(5);
			template.DamageTypeModifiers[DamageType.Fire] = DamageTypeModifier.Immunity;
			var creature = TestBuilders.Creature(1, template);
			var ability = TestBuilders.CreatureAbility(damageDiceCount: 1, damageModifier: 0, damageType: DamageType.Fire);
			var attackerContext = TestBuilders.Context();
			var defenderContext = TestBuilders.Context(template: template, creature: creature);
			var attack = BattleAttackBuilder.AwaitingDamageRoll(ParticipantKind.Creature, 9, 5, null, damageRoll: 6);

			var damage = BattleCombatCalculator.CalculateDamage(attackerContext, defenderContext, ParticipantKind.Creature, ability, attack);

			damage.FinalDamage.Should().Be(0);
		}

		[TestMethod]
		public void CalculateDamage_CharacterDefender_NoArmorEquipped_ArmorBeforeHitIsZero()
		{
			var character = TestBuilders.Character();
			var ability = TestBuilders.CharacterAbility(damageDiceCount: 1, damageModifier: 0);
			var attackerContext = TestBuilders.Context();
			var defenderContext = TestBuilders.Context(character: character);
			var attack = BattleAttackBuilder.AwaitingDamageRoll(ParticipantKind.Character, 2, null, HumanBodyPart.Torso, damageRoll: 4);

			var damage = BattleCombatCalculator.CalculateDamage(attackerContext, defenderContext, ParticipantKind.Character, ability, attack);

			// raw=4, no armor, ×1(Torso.DamageModifier) = 4
			damage.ArmorBeforeHit.Should().Be(0);
			damage.FinalDamage.Should().Be(4);
			damage.WornArmorItemId.Should().BeNull();
		}

		[TestMethod]
		public void CalculateDamage_CharacterDefender_EquippedArmorCoveringHitPart_Absorbs()
		{
			var armorTemplate = TestBuilders.ArmorTemplate().WithId(1);
			armorTemplate.AddArmorPart(HumanBodyPart.Torso, armor: 3);
			var armorItem = TestBuilders.Item(1, armorTemplate).WithId(7);
			armorItem.Equip();
			var character = TestBuilders.Character();
			character.Items.Add(armorItem);
			var ability = TestBuilders.CharacterAbility(damageDiceCount: 1, damageModifier: 0);
			var attackerContext = TestBuilders.Context();
			var defenderContext = TestBuilders.Context(character: character);
			var attack = BattleAttackBuilder.AwaitingDamageRoll(ParticipantKind.Character, 2, null, HumanBodyPart.Torso, damageRoll: 5);

			var damage = BattleCombatCalculator.CalculateDamage(attackerContext, defenderContext, ParticipantKind.Character, ability, attack);

			// raw=5, armorBefore=3, absorbed=3, afterArmor=2, ×1(Torso.DamageModifier)=2
			damage.ArmorBeforeHit.Should().Be(3);
			damage.ArmorAbsorbed.Should().Be(3);
			damage.FinalDamage.Should().Be(2);
			damage.WornArmorItemId.Should().Be(7);
		}

		[TestMethod]
		public void CalculateDamage_CharacterDefender_UnequippedArmorOnSamePart_IsIgnored()
		{
			var armorTemplate = TestBuilders.ArmorTemplate().WithId(1);
			armorTemplate.AddArmorPart(HumanBodyPart.Torso, armor: 3);
			var armorItem = TestBuilders.Item(1, armorTemplate).WithId(7); // не экипирован
			var character = TestBuilders.Character();
			character.Items.Add(armorItem);
			var ability = TestBuilders.CharacterAbility(damageDiceCount: 1, damageModifier: 0);
			var attackerContext = TestBuilders.Context();
			var defenderContext = TestBuilders.Context(character: character);
			var attack = BattleAttackBuilder.AwaitingDamageRoll(ParticipantKind.Character, 2, null, HumanBodyPart.Torso, damageRoll: 4);

			var damage = BattleCombatCalculator.CalculateDamage(attackerContext, defenderContext, ParticipantKind.Character, ability, attack);

			damage.ArmorBeforeHit.Should().Be(0);
			damage.FinalDamage.Should().Be(4);
		}
	}
}
