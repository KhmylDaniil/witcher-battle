using FluentAssertions;
using Wastelands.Service.Application.Services;
using Wastelands.Service.Application.UnitTest.TestSupport;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.UnitTest.Services
{
	[TestClass]
	public class BattleCombatCalculatorResolveHitTests
	{
		[TestMethod]
		public void ResolveHit_AttackExceedsDefense_SucceedsAndResolvesTargetedPart()
		{
			var ability = TestBuilders.CharacterAbility(attackSkill: Skill.Brawl, attackModifier: 2);
			var attackerContext = TestBuilders.Context(getSkillValue: _ => 5);
			var defenderContext = TestBuilders.Context(getSkillValue: _ => 4);
			var attack = BattleAttackBuilder.Fresh(ParticipantKind.Character, defenderId: 2);
			attack.SetTargetHumanBodyPart(HumanBodyPart.Torso); // HitPenalty = 1
			attack.SetAttackRoll(6);
			attack.SetDefenderChoice(Skill.Dodge, 3);

			var hit = BattleCombatCalculator.ResolveHit(attackerContext, defenderContext, ability, attack);

			// attackTotal = 5(skill) - 1(hitPenalty) - 0(bonus) + 0(cond) + 6(roll) + 2(abilityMod) = 12
			// defenseTotal = 4(skill) - 0(parry) + 0(cond) + 3(roll) = 7
			hit.AttackTotal.Should().Be(12);
			hit.DefenseTotal.Should().Be(7);
			hit.Succeeded.Should().BeTrue();
			hit.ResolvedHumanBodyPart.Should().Be(HumanBodyPart.Torso);
			hit.ResolvedCreaturePartId.Should().BeNull();
		}

		[TestMethod]
		public void ResolveHit_DefenseAtLeastAttack_Misses_AndDoesNotResolvePart()
		{
			var ability = TestBuilders.CharacterAbility();
			var attackerContext = TestBuilders.Context(getSkillValue: _ => 3);
			var defenderContext = TestBuilders.Context(getSkillValue: _ => 10);
			var attack = BattleAttackBuilder.Fresh();
			attack.SetTargetHumanBodyPart(HumanBodyPart.Torso);
			attack.SetAttackRoll(1);
			attack.SetDefenderChoice(Skill.Dodge, 1);

			var hit = BattleCombatCalculator.ResolveHit(attackerContext, defenderContext, ability, attack);

			hit.Succeeded.Should().BeFalse();
			hit.ResolvedHumanBodyPart.Should().BeNull();
		}

		[TestMethod]
		public void ResolveHit_EqualTotals_CountsAsMiss()
		{
			// succeeded = attackTotal > defenseTotal (строго больше) — равенство защите засчитывается защитнику.
			var ability = TestBuilders.CharacterAbility(attackModifier: 0);
			var attackerContext = TestBuilders.Context(getSkillValue: _ => 5);
			var defenderContext = TestBuilders.Context(getSkillValue: _ => 5);
			var attack = BattleAttackBuilder.Fresh();
			attack.SetTargetHumanBodyPart(HumanBodyPart.Torso); // hitPenalty 1
			attack.SetAttackRoll(6); // attackTotal = 5 - 1(hitPenalty) + 6 + 0 = 10
			attack.SetDefenderChoice(Skill.Dodge, 5); // defenseTotal = 5 + 5 = 10

			var hit = BattleCombatCalculator.ResolveHit(attackerContext, defenderContext, ability, attack);

			hit.AttackTotal.Should().Be(10);
			hit.DefenseTotal.Should().Be(10);
			hit.Succeeded.Should().BeFalse();
		}

		[TestMethod]
		public void ResolveHit_BonusAction_SubtractsRollPenaltyFromAttack()
		{
			var ability = TestBuilders.CharacterAbility();
			var attackerContext = TestBuilders.Context(getSkillValue: _ => 5);
			var defenderContext = TestBuilders.Context(getSkillValue: _ => 0);
			var attack = BattleAttackBuilder.Fresh(isBonusAction: true);
			attack.SetTargetHumanBodyPart(HumanBodyPart.Torso); // hitPenalty 1
			attack.SetAttackRoll(6);
			attack.SetDefenderChoice(Skill.Dodge, 0);

			var hit = BattleCombatCalculator.ResolveHit(attackerContext, defenderContext, ability, attack);

			// attackTotal = 5 - 1(hitPenalty) - 3(bonusActionPenalty) + 6 + 0 = 7
			hit.AttackTotal.Should().Be(7);
		}

		[TestMethod]
		public void ResolveHit_Parry_SubtractsRollPenaltyFromDefense()
		{
			var ability = TestBuilders.CharacterAbility();
			var attackerContext = TestBuilders.Context(getSkillValue: _ => 0);
			var defenderContext = TestBuilders.Context(getSkillValue: _ => 5);
			var attack = BattleAttackBuilder.Fresh();
			attack.SetTargetHumanBodyPart(HumanBodyPart.Torso);
			attack.SetAttackRoll(0);
			attack.SetDefenderChoice(Skill.Sword, 4, isParry: true);

			var hit = BattleCombatCalculator.ResolveHit(attackerContext, defenderContext, ability, attack);

			// defenseTotal = 5(skill) - 3(parryPenalty) + 4(roll) = 6
			hit.DefenseTotal.Should().Be(6);
		}

		[TestMethod]
		public void ResolveHit_StunnedDefender_FixesDefenseAtTenAndIgnoresDefensiveSkill()
		{
			var ability = TestBuilders.CharacterAbility();
			var attackerContext = TestBuilders.Context(getSkillValue: _ => 5);
			// Оглушённый защитник не выбирает защитный навык вовсе (DefensiveSkill == null) — не должно упасть.
			var defenderContext = TestBuilders.Context();
			var attack = BattleAttackBuilder.Fresh();
			attack.SetTargetHumanBodyPart(HumanBodyPart.Torso);
			attack.SetAttackRoll(1);

			var hit = BattleCombatCalculator.ResolveHit(attackerContext, defenderContext, ability, attack, defenderIsStunned: true);

			hit.DefenseTotal.Should().Be(10);
			hit.DefenseRoll.Should().Be(0);
		}

		[TestMethod]
		public void ResolveHit_ConditionModifiers_ApplyToRespectiveSides()
		{
			var ability = TestBuilders.CharacterAbility();
			var attackerContext = TestBuilders.Context(getSkillValue: _ => 5);
			var defenderContext = TestBuilders.Context(getSkillValue: _ => 5);
			var attack = BattleAttackBuilder.Fresh();
			attack.SetTargetHumanBodyPart(HumanBodyPart.Torso);
			attack.SetAttackRoll(5);
			attack.SetDefenderChoice(Skill.Dodge, 5);

			var hit = BattleCombatCalculator.ResolveHit(
				attackerContext, defenderContext, ability, attack, attackerConditionModifier: -2, defenderConditionModifier: -1);

			// attackTotal = 5 - 1(hitPenalty) - 2(cond) + 5 + 0 = 7
			// defenseTotal = 5 - 1(cond) + 5 = 9
			hit.AttackTotal.Should().Be(7);
			hit.DefenseTotal.Should().Be(9);
		}

		[TestMethod]
		public void ResolveHit_CreatureDefender_ExplicitTargetPart_UsesItsHitPenaltyAndResolvesOnSuccess()
		{
			var template = TestBuilders.CreatureTemplate();
			var torso = template.Parts[1].WithId(5); // HitPenalty = 1 (см. DefaultHumanBodyTemplatePartsDraft)
			var ability = TestBuilders.CreatureAbility();
			var attackerContext = TestBuilders.Context(getSkillValue: _ => 5);
			var defenderContext = TestBuilders.Context(template: template);
			var attack = BattleAttackBuilder.Fresh(ParticipantKind.Creature, defenderId: 9);
			attack.SetTargetPart(torso.Id);
			attack.SetAttackRoll(6);
			attack.SetDefenderChoice(Skill.Dodge, 0);

			var hit = BattleCombatCalculator.ResolveHit(attackerContext, defenderContext, ability, attack);

			hit.Succeeded.Should().BeTrue();
			hit.ResolvedCreaturePartId.Should().Be(5);
			hit.ResolvedHumanBodyPart.Should().BeNull();
		}
	}
}
