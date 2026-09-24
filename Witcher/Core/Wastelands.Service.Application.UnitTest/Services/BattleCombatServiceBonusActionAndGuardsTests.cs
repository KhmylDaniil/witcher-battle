using FluentAssertions;
using Moq;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.UnitTest.TestSupport;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.UnitTest.Services
{
	[TestClass]
	public class BattleCombatServiceBonusActionAndGuardsTests
	{
		private const long BattleId = 1;
		private const long AttackerId = 10;
		private const long DefenderId = 11;
		private const long AbilityId = 99;

		/// <summary>Attacker (активен, Initiative=1) + Defender (Initiative=2) — бой уже начат.</summary>
		private static (Battle Battle, Character AttackerCharacter, BattleCharacter Attacker) CreateScenario()
		{
			var battle = new Battle(1, "Battle").WithId(BattleId);

			var attackerCharacter = TestBuilders.Character(name: "Attacker").WithId(AttackerId);
			var ability = TestBuilders.CharacterAbility(name: "Punch").WithId(AbilityId);
			attackerCharacter.Abilities.Add(ability);
			var attacker = TestBuilders.BattleCharacter(BattleId, attackerCharacter);
			attacker.SetInitiative(1);
			attacker.Character = attackerCharacter;

			var defenderCharacter = TestBuilders.Character(name: "Defender").WithId(DefenderId);
			var defender = TestBuilders.BattleCharacter(BattleId, defenderCharacter);
			defender.SetInitiative(2);
			defender.Character = defenderCharacter;

			battle.Characters.Add(attacker);
			battle.Characters.Add(defender);
			battle.MarkStarted();

			return (battle, attackerCharacter, attacker);
		}

		private static BattleCombatServiceFixture CreateFixture(Battle battle, Character attackerCharacter)
		{
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			fixture.SetContext(ParticipantKind.Character, AttackerId, TestBuilders.Context(abilities: attackerCharacter.Abilities));
			return fixture;
		}

		[TestMethod]
		public async Task StartAttackAsync_FirstActionThisTurn_DoesNotChargeStaminaOrMarkBonusAction()
		{
			var (battle, attackerCharacter, attacker) = CreateScenario();
			var staminaBefore = attacker.CurrentSta;
			var fixture = CreateFixture(battle, attackerCharacter);
			var service = fixture.BuildService();

			await service.StartAttackAsync(new() { BattleId = BattleId, AbilityId = AbilityId, DefenderKind = ParticipantKind.Character, DefenderId = DefenderId });

			battle.Attack!.IsBonusAction.Should().BeFalse();
			attacker.CurrentSta.Should().Be(staminaBefore);
		}

		[TestMethod]
		public async Task StartAttackAsync_SecondActionThisTurn_ChargesStaminaAndMarksBonusAction()
		{
			var (battle, attackerCharacter, attacker) = CreateScenario();
			attacker.MarkActedThisTurn();
			var staminaBefore = attacker.CurrentSta;
			var fixture = CreateFixture(battle, attackerCharacter);
			var service = fixture.BuildService();

			await service.StartAttackAsync(new() { BattleId = BattleId, AbilityId = AbilityId, DefenderKind = ParticipantKind.Character, DefenderId = DefenderId });

			battle.Attack!.IsBonusAction.Should().BeTrue();
			attacker.CurrentSta.Should().Be(staminaBefore - 3);
		}

		[TestMethod]
		public async Task StartAttackAsync_SecondActionThisTurn_InsufficientStamina_Throws()
		{
			var (battle, attackerCharacter, attacker) = CreateScenario();
			attacker.MarkActedThisTurn();
			attacker.SpendStamina(attacker.CurrentSta - 2); // остаётся 2 < StaminaCost(3)
			var fixture = CreateFixture(battle, attackerCharacter);
			var service = fixture.BuildService();

			var act = () => service.StartAttackAsync(new() { BattleId = BattleId, AbilityId = AbilityId, DefenderKind = ParticipantKind.Character, DefenderId = DefenderId });

			(await act.Should().ThrowAsync<InvalidArgumentException>())
				.Which.ErrorCode.Should().Be(ErrorCode.NotEnoughStaminaForBonusAction);
		}

		[TestMethod]
		public async Task StartAttackAsync_ActiveParticipantStunned_Throws()
		{
			var (battle, attackerCharacter, attacker) = CreateScenario();
			attacker.AddCondition(Condition.Stun);
			var fixture = CreateFixture(battle, attackerCharacter);
			var service = fixture.BuildService();

			var act = () => service.StartAttackAsync(new() { BattleId = BattleId, AbilityId = AbilityId, DefenderKind = ParticipantKind.Character, DefenderId = DefenderId });

			(await act.Should().ThrowAsync<InvalidArgumentException>())
				.Which.ErrorCode.Should().Be(ErrorCode.ParticipantIsStunned);
		}

		[TestMethod]
		public async Task StartAttackAsync_ActiveParticipantDying_Throws()
		{
			var (battle, attackerCharacter, attacker) = CreateScenario();
			attacker.AddCondition(Condition.Dying);
			var fixture = CreateFixture(battle, attackerCharacter);
			var service = fixture.BuildService();

			var act = () => service.StartAttackAsync(new() { BattleId = BattleId, AbilityId = AbilityId, DefenderKind = ParticipantKind.Character, DefenderId = DefenderId });

			(await act.Should().ThrowAsync<InvalidArgumentException>())
				.Which.ErrorCode.Should().Be(ErrorCode.ParticipantIsDying);
		}

		[TestMethod]
		public async Task SkipTurnAsync_ActiveParticipantStunned_Throws()
		{
			var (battle, attackerCharacter, attacker) = CreateScenario();
			attacker.AddCondition(Condition.Stun);
			var fixture = CreateFixture(battle, attackerCharacter);
			var service = fixture.BuildService();

			var act = () => service.SkipTurnAsync(BattleId);

			(await act.Should().ThrowAsync<InvalidArgumentException>())
				.Which.ErrorCode.Should().Be(ErrorCode.ParticipantIsStunned);
		}

		[TestMethod]
		public async Task SkipTurnAsync_ActiveParticipantDying_Throws()
		{
			var (battle, attackerCharacter, attacker) = CreateScenario();
			attacker.AddCondition(Condition.Dying);
			var fixture = CreateFixture(battle, attackerCharacter);
			var service = fixture.BuildService();

			var act = () => service.SkipTurnAsync(BattleId);

			(await act.Should().ThrowAsync<InvalidArgumentException>())
				.Which.ErrorCode.Should().Be(ErrorCode.ParticipantIsDying);
		}

		[TestMethod]
		public async Task SkipTurnAsync_NotStunnedNorDying_AdvancesTurn()
		{
			var (battle, attackerCharacter, _) = CreateScenario();
			var fixture = CreateFixture(battle, attackerCharacter);
			var service = fixture.BuildService();

			await service.SkipTurnAsync(BattleId);

			fixture.TurnProcessor.Verify(t => t.AdvanceTurnAsync(battle), Times.Once);
		}

		[TestMethod]
		public async Task AttemptRemoveConditionAsync_CharacterNormalAction_RemovesConditionAndMarksActedInsteadOfAdvancing()
		{
			var (battle, attackerCharacter, attacker) = CreateScenario();
			attacker.AddCondition(Condition.Bleed);
			var fixture = CreateFixture(battle, attackerCharacter);
			var service = fixture.BuildService();

			// FirstAid(5) + roll(10) - penalty(0) = 15 >= difficulty(14)
			await service.AttemptRemoveConditionAsync(new()
			{
				BattleId = BattleId,
				Condition = Condition.Bleed,
				Skill = Skill.FirstAid,
				TargetKind = ParticipantKind.Character,
				TargetId = AttackerId,
				Roll = 10,
			});

			attacker.AppliedConditions.Should().NotContain(Condition.Bleed);
			attacker.HasActedThisTurn.Should().BeTrue();
			fixture.TurnProcessor.Verify(t => t.AdvanceTurnAsync(It.IsAny<Battle>()), Times.Never);
		}

		[TestMethod]
		public async Task AttemptRemoveConditionAsync_RollBelowDifficulty_LeavesConditionApplied()
		{
			var (battle, attackerCharacter, attacker) = CreateScenario();
			attacker.AddCondition(Condition.Bleed);
			var fixture = CreateFixture(battle, attackerCharacter);
			var service = fixture.BuildService();

			// FirstAid(5) + roll(0) = 5 < difficulty(14)
			await service.AttemptRemoveConditionAsync(new()
			{
				BattleId = BattleId,
				Condition = Condition.Bleed,
				Skill = Skill.FirstAid,
				TargetKind = ParticipantKind.Character,
				TargetId = AttackerId,
				Roll = 0,
			});

			attacker.AppliedConditions.Should().Contain(Condition.Bleed);
		}

		[TestMethod]
		public async Task AttemptRemoveConditionAsync_SelfOnlySkillTargetingOther_Throws()
		{
			var (battle, attackerCharacter, attacker) = CreateScenario();
			attacker.AddCondition(Condition.Poison);
			var fixture = CreateFixture(battle, attackerCharacter);
			var service = fixture.BuildService();

			var act = () => service.AttemptRemoveConditionAsync(new()
			{
				BattleId = BattleId,
				Condition = Condition.Poison,
				Skill = Skill.Endurance, // self-only
				TargetKind = ParticipantKind.Character,
				TargetId = DefenderId, // не сам активный участник
				Roll = 20,
			});

			(await act.Should().ThrowAsync<InvalidArgumentException>())
				.Which.ErrorCode.Should().Be(ErrorCode.ConditionRemovalTargetInvalid);
		}

		[TestMethod]
		public async Task ClearConditionAsync_AutoClearableCondition_AlwaysSucceedsAndMarksActed()
		{
			var (battle, attackerCharacter, attacker) = CreateScenario();
			attacker.AddCondition(Condition.Prone);
			var fixture = CreateFixture(battle, attackerCharacter);
			var service = fixture.BuildService();

			await service.ClearConditionAsync(new() { BattleId = BattleId, Condition = Condition.Prone });

			attacker.AppliedConditions.Should().NotContain(Condition.Prone);
			attacker.HasActedThisTurn.Should().BeTrue();
		}

		[TestMethod]
		public async Task ClearConditionAsync_ConditionNotAutoClearable_Throws()
		{
			var (battle, attackerCharacter, attacker) = CreateScenario();
			attacker.AddCondition(Condition.Bleed);
			var fixture = CreateFixture(battle, attackerCharacter);
			var service = fixture.BuildService();

			var act = () => service.ClearConditionAsync(new() { BattleId = BattleId, Condition = Condition.Bleed });

			(await act.Should().ThrowAsync<InvalidArgumentException>())
				.Which.ErrorCode.Should().Be(ErrorCode.ConditionRemovalRuleNotFound);
		}
	}
}
