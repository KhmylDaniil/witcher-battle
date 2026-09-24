using FluentAssertions;
using Wastelands.Service.Application.Services;
using Wastelands.Service.Application.UnitTest.TestSupport;
using Wastelands.Service.Domain.Drafts;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.UnitTest.Services
{
	[TestClass]
	public class BattleCombatCalculatorCriticalHitAndConditionsTests
	{
		[TestMethod]
		public void TryResolveCriticalHit_ExcessBelowSeven_ReturnsNull()
		{
			var attack = BattleAttackBuilder.AwaitingDamageRoll(
				ParticipantKind.Character, 2, null, HumanBodyPart.Torso, damageRoll: 3, attackTotal: 10, defenseTotal: 4); // excess = 6
			var defenderContext = TestBuilders.Context();

			var crit = BattleCombatCalculator.TryResolveCriticalHit(attack, defenderContext, DamageType.Slashing);

			crit.Should().BeNull();
		}

		[TestMethod]
		public void TryResolveCriticalHit_ExcessSeven_ReturnsSimpleWithBonusDamageThree()
		{
			var attack = BattleAttackBuilder.AwaitingDamageRoll(
				ParticipantKind.Character, 2, null, HumanBodyPart.Torso, damageRoll: 3, attackTotal: 17, defenseTotal: 10); // excess = 7
			var defenderContext = TestBuilders.Context();

			var crit = BattleCombatCalculator.TryResolveCriticalHit(attack, defenderContext, DamageType.Slashing);

			crit.Should().NotBeNull();
			crit!.Value.Severity.Should().Be(CriticalWoundSeverity.Simple);
			crit.Value.BonusDamage.Should().Be(3);
		}

		[TestMethod]
		public void TryResolveCriticalHit_ExcessTen_ReturnsMediumWithBonusDamageFive()
		{
			var attack = BattleAttackBuilder.AwaitingDamageRoll(
				ParticipantKind.Character, 2, null, HumanBodyPart.Torso, damageRoll: 3, attackTotal: 20, defenseTotal: 10); // excess = 10
			var defenderContext = TestBuilders.Context();

			var crit = BattleCombatCalculator.TryResolveCriticalHit(attack, defenderContext, DamageType.Slashing);

			crit!.Value.Severity.Should().Be(CriticalWoundSeverity.Medium);
			crit.Value.BonusDamage.Should().Be(5);
		}

		[TestMethod]
		public void TryResolveCriticalHit_ExcessThirteen_ReturnsDifficultWithBonusDamageEight()
		{
			var attack = BattleAttackBuilder.AwaitingDamageRoll(
				ParticipantKind.Character, 2, null, HumanBodyPart.Torso, damageRoll: 3, attackTotal: 23, defenseTotal: 10); // excess = 13
			var defenderContext = TestBuilders.Context();

			var crit = BattleCombatCalculator.TryResolveCriticalHit(attack, defenderContext, DamageType.Slashing);

			crit!.Value.Severity.Should().Be(CriticalWoundSeverity.Difficult);
			crit.Value.BonusDamage.Should().Be(8);
		}

		[TestMethod]
		public void TryResolveCriticalHit_CharacterDefender_SlotKeyMatchesCatalogForHumanPart()
		{
			var attack = BattleAttackBuilder.AwaitingDamageRoll(
				ParticipantKind.Character, 2, null, HumanBodyPart.Head, damageRoll: 3, attackTotal: 20, defenseTotal: 10);
			var defenderContext = TestBuilders.Context();

			var crit = BattleCombatCalculator.TryResolveCriticalHit(attack, defenderContext, DamageType.Fire);

			crit!.Value.SlotKey.Should().Be(CriticalWoundCatalog.SlotKey(HumanBodyPart.Head, DamageType.Fire));
		}

		[TestMethod]
		public void TryResolveCriticalHit_CreatureDefender_SlotKeyMatchesCatalogForPartId()
		{
			var template = TestBuilders.CreatureTemplate();
			template.Parts[1].WithId(5);
			var attack = BattleAttackBuilder.AwaitingDamageRoll(
				ParticipantKind.Creature, 9, 5, null, damageRoll: 3, attackTotal: 20, defenseTotal: 10);
			var defenderContext = TestBuilders.Context(template: template);

			var crit = BattleCombatCalculator.TryResolveCriticalHit(attack, defenderContext, DamageType.Piercing);

			crit!.Value.SlotKey.Should().Be(CriticalWoundCatalog.SlotKey(5L, DamageType.Piercing));
		}

		[TestMethod]
		public void RollAppliedConditions_AbilityWithNoConditions_ReturnsEmpty()
		{
			var ability = TestBuilders.CharacterAbility();

			var applied = BattleCombatCalculator.RollAppliedConditions(ability);

			applied.Should().BeEmpty();
		}

		[TestMethod]
		public void RollAppliedConditions_HundredPercentChance_AlwaysApplies()
		{
			var ability = TestBuilders.CharacterAbility();
			ability.AppliedConditions.Add(new AbilityAppliedCondition(abilityId: 1, Condition.Bleed, applyChance: 100));

			var applied = BattleCombatCalculator.RollAppliedConditions(ability);

			applied.Should().ContainSingle().Which.Should().Be(Condition.Bleed);
		}

		[TestMethod]
		public void RollAppliedConditions_EachConditionRolledIndependently_MultipleAtHundredPercentAllApply()
		{
			var ability = TestBuilders.CharacterAbility();
			ability.AppliedConditions.Add(new AbilityAppliedCondition(1, Condition.Bleed, 100));
			ability.AppliedConditions.Add(new AbilityAppliedCondition(1, Condition.Staggered, 100));

			var applied = BattleCombatCalculator.RollAppliedConditions(ability);

			applied.Should().BeEquivalentTo([Condition.Bleed, Condition.Staggered]);
		}
	}
}
