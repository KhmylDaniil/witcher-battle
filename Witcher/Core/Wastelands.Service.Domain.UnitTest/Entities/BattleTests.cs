using FluentAssertions;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Domain.UnitTest.TestSupport;

namespace Wastelands.Service.Domain.UnitTest.Entities
{
	[TestClass]
	public class BattleTests
	{
		private static Battle CreateStartedBattleWithCharacters(int count, out BattleCharacter[] characters)
		{
			var battle = new Battle(gameId: 1, name: "Test battle").WithId(1);
			characters = new BattleCharacter[count];
			for (var i = 0; i < count; i++)
			{
				var character = TestBuilders.Character(name: $"Char{i}").WithId(i + 1);
				var battleCharacter = TestBuilders.BattleCharacter(battle.Id, character);
				battleCharacter.SetInitiative(i + 1);
				battle.Characters.Add(battleCharacter);
				characters[i] = battleCharacter;
			}

			battle.MarkStarted();
			return battle;
		}

		[TestMethod]
		public void MarkStarted_SetsInProgressAndFirstInitiative()
		{
			var battle = new Battle(1, "Battle");

			battle.MarkStarted();

			battle.CurrentInitiative.Should().Be(1);
		}

		[TestMethod]
		public void MarkStarted_WhenAlreadyStarted_Throws()
		{
			var battle = new Battle(1, "Battle");
			battle.MarkStarted();

			var act = battle.MarkStarted;

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void AdvanceTurn_MovesToNextParticipant_WithoutIncrementingRound()
		{
			var battle = CreateStartedBattleWithCharacters(3, out _);

			battle.AdvanceTurn();

			battle.CurrentInitiative.Should().Be(2);
			battle.CurrentRound.Should().Be(1);
		}

		[TestMethod]
		public void AdvanceTurn_PastLastParticipant_WrapsToFirstAndIncrementsRound()
		{
			var battle = CreateStartedBattleWithCharacters(3, out _);
			battle.AdvanceTurn();
			battle.AdvanceTurn();

			battle.AdvanceTurn();

			battle.CurrentInitiative.Should().Be(1);
			battle.CurrentRound.Should().Be(2);
		}

		[TestMethod]
		public void AdvanceTurn_ResetsHasActedThisTurnForAllCharacters()
		{
			var battle = CreateStartedBattleWithCharacters(2, out var characters);
			characters[0].MarkActedThisTurn();
			characters[1].MarkActedThisTurn();

			battle.AdvanceTurn();

			characters[0].HasActedThisTurn.Should().BeFalse();
			characters[1].HasActedThisTurn.Should().BeFalse();
		}

		[TestMethod]
		public void AdvanceTurn_BeforeBattleStarted_Throws()
		{
			var battle = new Battle(1, "Battle");

			var act = battle.AdvanceTurn;

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void RemoveCharacter_WhenNotActive_KeepsActiveParticipantAndRenumbersRest()
		{
			var battle = CreateStartedBattleWithCharacters(3, out var characters);
			// Активен characters[0] (Initiative=1). Удаляем characters[1] (Initiative=2), она не активна.

			battle.RemoveCharacter(characters[1]);

			battle.Characters.Should().HaveCount(2);
			battle.Characters.Should().NotContain(characters[1]);
			characters[0].Initiative.Should().Be(1);
			characters[2].Initiative.Should().Be(2);
			// Активный участник не должен был смениться (тот же CharacterId=1).
			battle.CurrentInitiative.Should().Be(1);
		}

		[TestMethod]
		public void RemoveCharacter_WhenActive_HandsOffTurnToNextParticipant()
		{
			var battle = CreateStartedBattleWithCharacters(3, out var characters);
			// Активен characters[0] (Initiative=1) — удаляем именно его.

			battle.RemoveCharacter(characters[0]);

			battle.Characters.Should().HaveCount(2);
			// Оставшиеся переиндексированы плотно 1..N, сохраняя относительный порядок.
			characters[1].Initiative.Should().Be(1);
			characters[2].Initiative.Should().Be(2);
			// Ход передан дальше — теперь активен бывший characters[1], который стал Initiative=1.
			battle.CurrentInitiative.Should().Be(1);
		}

		[TestMethod]
		public void RemoveCharacter_LastRemainingParticipant_ClearsCurrentInitiative()
		{
			var battle = CreateStartedBattleWithCharacters(1, out var characters);

			battle.RemoveCharacter(characters[0]);

			battle.Characters.Should().BeEmpty();
			battle.CurrentInitiative.Should().BeNull();
		}

		[TestMethod]
		public void RemoveCharacter_NotInBattle_IsNoOp()
		{
			var battle = CreateStartedBattleWithCharacters(1, out _);
			var strayCharacter = TestBuilders.Character().WithId(99);
			var stray = TestBuilders.BattleCharacter(battle.Id, strayCharacter);
			stray.SetInitiative(1);

			battle.RemoveCharacter(stray);

			battle.Characters.Should().HaveCount(1);
		}

		[TestMethod]
		public void StartAttack_WhenAttackAlreadyInProgress_Throws()
		{
			var battle = new Battle(1, "Battle");
			battle.StartAttack(new BattleAttack(1, ParticipantKind.Character, 1, 1, 1, ParticipantKind.Character, 2, false));

			var act = () => battle.StartAttack(new BattleAttack(1, ParticipantKind.Character, 1, 1, 1, ParticipantKind.Character, 2, false));

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void ClearAttack_RemovesCurrentAttack()
		{
			var battle = new Battle(1, "Battle");
			battle.StartAttack(new BattleAttack(1, ParticipantKind.Character, 1, 1, 1, ParticipantKind.Character, 2, false));

			battle.ClearAttack();

			battle.Attack.Should().BeNull();
		}
	}
}
