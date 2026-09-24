using FluentAssertions;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.UnitTest.Entities
{
	[TestClass]
	public class BattleAttackTests
	{
		private static BattleAttack CreateAttack(int attacksAllowed = 1)
			=> new(battleId: 1, ParticipantKind.Character, attackerId: 1, abilityId: 1, attacksAllowed, ParticipantKind.Character, defenderId: 2, isBonusAction: false);

		[TestMethod]
		public void Constructor_SetsAwaitingChoicesPhase()
		{
			var attack = CreateAttack();

			attack.Phase.Should().Be(BattleAttackPhase.AwaitingChoices);
		}

		[TestMethod]
		public void ConfirmAttacker_TwiceInARow_Throws()
		{
			var attack = CreateAttack();
			attack.ConfirmAttacker();

			var act = attack.ConfirmAttacker;

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void SetAttackRoll_AfterAttackerConfirmed_Throws()
		{
			var attack = CreateAttack();
			attack.ConfirmAttacker();

			var act = () => attack.SetAttackRoll(5);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void SetDefenderChoice_AfterDefenderConfirmed_Throws()
		{
			var attack = CreateAttack();
			attack.SetDefenderChoice(Skill.Dodge, 5);
			attack.ConfirmDefender();

			var act = () => attack.SetDefenderChoice(Skill.Acrobatics, 5);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void SetDefenderChoice_BeforeConfirm_CanBeOverwritten()
		{
			var attack = CreateAttack();
			attack.SetDefenderChoice(Skill.Dodge, 5);

			attack.SetDefenderChoice(Skill.Acrobatics, 7);

			attack.DefensiveSkill.Should().Be(Skill.Acrobatics);
			attack.DefenseRoll.Should().Be(7);
		}

		[TestMethod]
		public void ConfirmDefender_WithoutDefensiveSkillChosen_Throws()
		{
			var attack = CreateAttack();

			var act = () => attack.ConfirmDefender(defenderIsStunned: false);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void ConfirmDefender_WhenDefenderStunned_SkipsDefensiveSkillRequirement()
		{
			var attack = CreateAttack();

			var act = () => attack.ConfirmDefender(defenderIsStunned: true);

			act.Should().NotThrow();
			attack.DefenderConfirmed.Should().BeTrue();
		}

		[TestMethod]
		public void MarkHitResolved_BeforeBothConfirmed_Throws()
		{
			var attack = CreateAttack();
			attack.ConfirmAttacker();
			// Защитник ещё не подтвердил.

			var act = () => attack.MarkHitResolved(true, null, null, 5, 10, 5, 8);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void MarkHitResolved_OnSuccess_MovesToAwaitingDamageRoll()
		{
			var attack = CreateAttack();
			attack.ConfirmAttacker();
			attack.SetDefenderChoice(Skill.Dodge, 5);
			attack.ConfirmDefender();

			attack.MarkHitResolved(succeeded: true, null, null, 8, 12, 5, 8);

			attack.Phase.Should().Be(BattleAttackPhase.AwaitingDamageRoll);
			attack.AttacksUsed.Should().Be(0);
		}

		[TestMethod]
		public void MarkHitResolved_OnMiss_MovesToSwingResolvedAndCountsAttack()
		{
			var attack = CreateAttack();
			attack.ConfirmAttacker();
			attack.SetDefenderChoice(Skill.Dodge, 5);
			attack.ConfirmDefender();

			attack.MarkHitResolved(succeeded: false, null, null, 2, 4, 5, 12);

			attack.Phase.Should().Be(BattleAttackPhase.SwingResolved);
			attack.AttacksUsed.Should().Be(1);
		}

		[TestMethod]
		public void SetDamageRoll_OutsideAwaitingDamageRollPhase_Throws()
		{
			var attack = CreateAttack();

			var act = () => attack.SetDamageRoll(5);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void MarkDamageResolved_RequiringStunSave_MovesToAwaitingStunSave()
		{
			var attack = CreateAttack();
			attack.ConfirmAttacker();
			attack.SetDefenderChoice(Skill.Dodge, 5);
			attack.ConfirmDefender();
			attack.MarkHitResolved(true, null, null, 8, 12, 5, 8);

			attack.MarkDamageResolved(requiresStunSave: true);

			attack.Phase.Should().Be(BattleAttackPhase.AwaitingStunSave);
			attack.AttacksUsed.Should().Be(1);
		}

		[TestMethod]
		public void MarkDamageResolved_NotRequiringStunSave_MovesToSwingResolved()
		{
			var attack = CreateAttack();
			attack.ConfirmAttacker();
			attack.SetDefenderChoice(Skill.Dodge, 5);
			attack.ConfirmDefender();
			attack.MarkHitResolved(true, null, null, 8, 12, 5, 8);

			attack.MarkDamageResolved(requiresStunSave: false);

			attack.Phase.Should().Be(BattleAttackPhase.SwingResolved);
		}

		[TestMethod]
		public void PrepareNextSwing_BeforeCurrentSwingResolved_Throws()
		{
			var attack = CreateAttack(attacksAllowed: 2);

			var act = () => attack.PrepareNextSwing(ParticipantKind.Character, 3);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void PrepareNextSwing_WhenNoAttacksRemaining_Throws()
		{
			var attack = CreateAttack(attacksAllowed: 1);
			attack.ConfirmAttacker();
			attack.SetDefenderChoice(Skill.Dodge, 5);
			attack.ConfirmDefender();
			attack.MarkHitResolved(false, null, null, 2, 4, 5, 12); // AttacksUsed -> 1 == AttacksAllowed

			var act = () => attack.PrepareNextSwing(ParticipantKind.Character, 3);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void PrepareNextSwing_ResetsPerSwingStateForNextDefender()
		{
			var attack = CreateAttack(attacksAllowed: 2);
			attack.ConfirmAttacker();
			attack.SetDefenderChoice(Skill.Dodge, 5, isParry: true);
			attack.ConfirmDefender();
			attack.MarkHitResolved(false, null, null, 2, 4, 5, 12); // AttacksUsed -> 1 < AttacksAllowed(2)

			attack.PrepareNextSwing(ParticipantKind.Creature, 7);

			attack.Phase.Should().Be(BattleAttackPhase.AwaitingChoices);
			attack.DefenderKind.Should().Be(ParticipantKind.Creature);
			attack.DefenderId.Should().Be(7);
			attack.DefensiveSkill.Should().BeNull();
			attack.IsParry.Should().BeFalse();
			attack.AttackerConfirmed.Should().BeFalse();
			attack.DefenderConfirmed.Should().BeFalse();
		}
	}
}
