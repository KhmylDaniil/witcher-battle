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
	public class BattleCombatServiceDyingTests
	{
		private const long BattleId = 1;

		/// <summary>Hero (Dying, активен, Initiative=1) + Medic (Initiative=2) — бой уже начат.</summary>
		private static (Battle Battle, Character HeroCharacter, BattleCharacter Hero, BattleCharacter Medic) CreateDyingScenario()
		{
			var battle = new Battle(1, "Battle").WithId(BattleId);

			var heroCharacter = TestBuilders.Character(name: "Hero", stat: 8).WithId(10); // Stun = (8+8)/2 = 8
			var hero = TestBuilders.BattleCharacter(BattleId, heroCharacter);
			hero.SetInitiative(1);
			hero.AddCondition(Condition.Dying);
			hero.Character = heroCharacter;

			var medicCharacter = TestBuilders.Character(name: "Medic").WithId(11);
			var medic = TestBuilders.BattleCharacter(BattleId, medicCharacter);
			medic.SetInitiative(2);
			medic.Character = medicCharacter;

			battle.Characters.Add(hero);
			battle.Characters.Add(medic);
			battle.MarkStarted();

			return (battle, heroCharacter, hero, medic);
		}

		[TestMethod]
		public async Task RollDyingSaveAsync_RollAtLeastStun_Survives_KeepsCharacterAndAdvancesTurn()
		{
			var (battle, heroCharacter, hero, _) = CreateDyingScenario();
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			fixture.SetContext(ParticipantKind.Character, hero.CharacterId, TestBuilders.Context(character: heroCharacter));
			var service = fixture.BuildService();

			await service.RollDyingSaveAsync(new() { BattleId = BattleId, Roll = 8 }); // 8 >= Stun(8)

			battle.Characters.Should().Contain(hero);
			hero.AppliedConditions.Should().Contain(Condition.Dying);
			fixture.TurnProcessor.Verify(t => t.AdvanceTurnAsync(battle), Times.Once);
			fixture.TurnProcessor.Verify(t => t.ProcessCurrentTurnAsync(It.IsAny<Battle>()), Times.Never);
			battle.LogEntries.Should().Contain(e => e.Message.Contains("остаётся при смерти"));
		}

		[TestMethod]
		public async Task RollDyingSaveAsync_RollBelowStun_Dies_RemovesCharacterAndProcessesCurrentTurn()
		{
			var (battle, heroCharacter, hero, medic) = CreateDyingScenario();
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			fixture.SetContext(ParticipantKind.Character, hero.CharacterId, TestBuilders.Context(character: heroCharacter));
			var service = fixture.BuildService();

			await service.RollDyingSaveAsync(new() { BattleId = BattleId, Roll = 0 }); // 0 < Stun(8)

			battle.Characters.Should().NotContain(hero);
			battle.Characters.Should().ContainSingle().Which.Should().Be(medic);
			fixture.TurnProcessor.Verify(t => t.ProcessCurrentTurnAsync(battle), Times.Once);
			fixture.TurnProcessor.Verify(t => t.AdvanceTurnAsync(It.IsAny<Battle>()), Times.Never);
			battle.LogEntries.Should().Contain(e => e.Message.Contains("умирает и выбывает"));
		}

		[TestMethod]
		public async Task RollDyingSaveAsync_ActiveCharacterNotDying_Throws()
		{
			var (battle, _, hero, _) = CreateDyingScenario();
			hero.RemoveCondition(Condition.Dying);
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			var service = fixture.BuildService();

			var act = () => service.RollDyingSaveAsync(new() { BattleId = BattleId, Roll = 8 });

			(await act.Should().ThrowAsync<InvalidArgumentException>())
				.Which.ErrorCode.Should().Be(ErrorCode.ParticipantNotDying);
		}

		[TestMethod]
		public async Task RollDyingSaveAsync_AttackAlreadyInProgress_Throws()
		{
			var (battle, _, _, _) = CreateDyingScenario();
			battle.StartAttack(new BattleAttack(BattleId, ParticipantKind.Character, 10, 1, 1, ParticipantKind.Character, 11, false));
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			var service = fixture.BuildService();

			var act = () => service.RollDyingSaveAsync(new() { BattleId = BattleId, Roll = 8 });

			(await act.Should().ThrowAsync<InvalidArgumentException>())
				.Which.ErrorCode.Should().Be(ErrorCode.AttackAlreadyInProgress);
		}
	}
}
