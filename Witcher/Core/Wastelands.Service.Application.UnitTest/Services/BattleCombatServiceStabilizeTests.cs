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
	public class BattleCombatServiceStabilizeTests
	{
		private const long BattleId = 1;

		/// <summary>Medic (активен, Initiative=1) + Hero (Dying, CurrentHP=-5 -> difficulty=5, Initiative=2).</summary>
		private static (Battle Battle, Character MedicCharacter, BattleCharacter Medic, BattleCharacter Hero) CreateScenario()
		{
			var battle = new Battle(1, "Battle").WithId(BattleId);

			var medicCharacter = TestBuilders.Character(name: "Medic").WithId(10);
			var medic = TestBuilders.BattleCharacter(BattleId, medicCharacter);
			medic.SetInitiative(1);
			medic.Character = medicCharacter;

			var heroCharacter = TestBuilders.Character(name: "Hero", hp: 10).WithId(11);
			var hero = TestBuilders.BattleCharacter(BattleId, heroCharacter);
			hero.SetInitiative(2);
			hero.ApplyDamage(15); // CurrentHP = -5 -> difficulty = 5
			hero.AddCondition(Condition.Dying);
			hero.Character = heroCharacter;

			battle.Characters.Add(medic);
			battle.Characters.Add(hero);
			battle.MarkStarted();

			return (battle, medicCharacter, medic, hero);
		}

		[TestMethod]
		public async Task StabilizeAsync_TotalAtLeastDifficulty_Succeeds_SetsHpToOneAndRemovesDying()
		{
			var (battle, medicCharacter, _, hero) = CreateScenario();
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			fixture.SetContext(ParticipantKind.Character, 10, TestBuilders.Context(getSkillValue: _ => 5));
			var service = fixture.BuildService();

			// total = FirstAid(5) + roll(0) - penalty(0) = 5 >= difficulty(5)
			await service.StabilizeAsync(new() { BattleId = BattleId, TargetCharacterId = 11, Roll = 0 });

			hero.CurrentHP.Should().Be(1);
			hero.AppliedConditions.Should().NotContain(Condition.Dying);
		}

		[TestMethod]
		public async Task StabilizeAsync_TotalBelowDifficulty_Fails_LeavesTargetUnaffected()
		{
			var (battle, _, _, hero) = CreateScenario();
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			fixture.SetContext(ParticipantKind.Character, 10, TestBuilders.Context(getSkillValue: _ => 5));
			var service = fixture.BuildService();

			// total = 5 + (-100) = -95 < difficulty(5)
			await service.StabilizeAsync(new() { BattleId = BattleId, TargetCharacterId = 11, Roll = -100 });

			hero.CurrentHP.Should().Be(-5);
			hero.AppliedConditions.Should().Contain(Condition.Dying);
		}

		[TestMethod]
		public async Task StabilizeAsync_NormalAction_MarksActedThisTurnInsteadOfAdvancingTurn()
		{
			var (battle, _, medic, _) = CreateScenario();
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			fixture.SetContext(ParticipantKind.Character, 10, TestBuilders.Context(getSkillValue: _ => 5));
			var service = fixture.BuildService();

			await service.StabilizeAsync(new() { BattleId = BattleId, TargetCharacterId = 11, Roll = 0 });

			medic.HasActedThisTurn.Should().BeTrue();
			fixture.TurnProcessor.Verify(t => t.AdvanceTurnAsync(It.IsAny<Battle>()), Times.Never);
		}

		[TestMethod]
		public async Task StabilizeAsync_AsBonusAction_ChargesStaminaAppliesPenaltyAndAdvancesTurn()
		{
			var (battle, _, medic, hero) = CreateScenario();
			medic.MarkActedThisTurn(); // основное действие уже потрачено -> это будет доп. действие
			var staminaBefore = medic.CurrentSta;
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			fixture.SetContext(ParticipantKind.Character, 10, TestBuilders.Context(getSkillValue: _ => 8));
			var service = fixture.BuildService();

			// total = FirstAid(8) + roll(0) - bonusPenalty(3) = 5 >= difficulty(5)
			await service.StabilizeAsync(new() { BattleId = BattleId, TargetCharacterId = 11, Roll = 0 });

			medic.CurrentSta.Should().Be(staminaBefore - 3);
			hero.CurrentHP.Should().Be(1);
			fixture.TurnProcessor.Verify(t => t.AdvanceTurnAsync(battle), Times.Once);
		}

		[TestMethod]
		public async Task StabilizeAsync_TargetNotDying_Throws()
		{
			var (battle, _, _, hero) = CreateScenario();
			hero.RemoveCondition(Condition.Dying);
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			var service = fixture.BuildService();

			var act = () => service.StabilizeAsync(new() { BattleId = BattleId, TargetCharacterId = 11, Roll = 0 });

			(await act.Should().ThrowAsync<InvalidArgumentException>())
				.Which.ErrorCode.Should().Be(ErrorCode.ParticipantNotDying);
		}

		[TestMethod]
		public async Task StabilizeAsync_ActiveParticipantStunned_Throws()
		{
			var (battle, _, medic, _) = CreateScenario();
			medic.AddCondition(Condition.Stun);
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			var service = fixture.BuildService();

			var act = () => service.StabilizeAsync(new() { BattleId = BattleId, TargetCharacterId = 11, Roll = 0 });

			(await act.Should().ThrowAsync<InvalidArgumentException>())
				.Which.ErrorCode.Should().Be(ErrorCode.ParticipantIsStunned);
		}

		[TestMethod]
		public async Task StabilizeAsync_ActiveParticipantDying_Throws()
		{
			var (battle, _, medic, _) = CreateScenario();
			medic.AddCondition(Condition.Dying);
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			var service = fixture.BuildService();

			var act = () => service.StabilizeAsync(new() { BattleId = BattleId, TargetCharacterId = 11, Roll = 0 });

			(await act.Should().ThrowAsync<InvalidArgumentException>())
				.Which.ErrorCode.Should().Be(ErrorCode.ParticipantIsDying);
		}
	}
}
