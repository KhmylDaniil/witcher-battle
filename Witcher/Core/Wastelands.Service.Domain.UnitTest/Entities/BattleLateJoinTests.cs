using FluentAssertions;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.UnitTest.TestSupport;

namespace Wastelands.Service.Domain.UnitTest.Entities
{
	/// <summary>Добавление участников в уже идущий бой: без броска, в конец очереди инициативы.</summary>
	[TestClass]
	public class BattleLateJoinTests
	{
		private static Creature NewCreature(Battle battle, long id, string name) =>
			TestBuilders.Creature(battle.Id, TestBuilders.CreatureTemplate(TestBuilders.BodyTemplate()), name).WithId(id);

		private static Battle StartedBattleWithTwoCreatures()
		{
			var battle = new Battle(1, "Battle").WithId(1);
			battle.AddCreature(NewCreature(battle, 1, "A"));
			battle.AddCreature(NewCreature(battle, 2, "B"));
			battle.Creatures[0].SetInitiative(1);
			battle.Creatures[1].SetInitiative(2);
			battle.MarkStarted();
			return battle;
		}

		[TestMethod]
		public void AddCreature_BeforeStart_LeavesInitiativeForTheStartRoll()
		{
			var battle = new Battle(1, "Battle").WithId(1);
			var creature = NewCreature(battle, 1, "A");

			var startsTurnNow = battle.AddCreature(creature);

			creature.Initiative.Should().BeNull();
			startsTurnNow.Should().BeFalse();
		}

		[TestMethod]
		public void AddingDuringBattle_AppendsToInitiativeInOrderOfAddition()
		{
			var battle = StartedBattleWithTwoCreatures();
			var late = NewCreature(battle, 3, "Late");
			var hero = TestBuilders.BattleCharacter(battle.Id, TestBuilders.Character().WithId(10));

			battle.AddCreature(late).Should().BeFalse();
			battle.AddCharacter(hero).Should().BeFalse();

			late.Initiative.Should().Be(3);
			hero.Initiative.Should().Be(4);
			battle.CurrentInitiative.Should().Be(1);
		}

		[TestMethod]
		public void LateParticipant_TakesTurnAfterExistingOnes_ThenRoundWraps()
		{
			var battle = StartedBattleWithTwoCreatures();
			var late = NewCreature(battle, 3, "Late");
			battle.AddCreature(late);

			battle.AdvanceTurn();
			battle.AdvanceTurn();

			battle.CurrentInitiative.Should().Be(late.Initiative);
			battle.CurrentRound.Should().Be(1);

			battle.AdvanceTurn();

			battle.CurrentInitiative.Should().Be(1);
			battle.CurrentRound.Should().Be(2);
		}

		[TestMethod]
		public void AddingToBattleWhereEveryoneLeft_MakesNewcomerActive()
		{
			var battle = StartedBattleWithTwoCreatures();
			battle.RemoveCreature(battle.Creatures[0]);
			battle.RemoveCreature(battle.Creatures[0]);
			battle.CurrentInitiative.Should().BeNull();

			var late = NewCreature(battle, 3, "Late");
			var startsTurnNow = battle.AddCreature(late);

			startsTurnNow.Should().BeTrue();
			late.Initiative.Should().Be(1);
			battle.CurrentInitiative.Should().Be(1);
		}

		[TestMethod]
		public void LateParticipant_KeepsDenseNumberingAfterEarlierRemoval()
		{
			var battle = StartedBattleWithTwoCreatures();
			battle.RemoveCreature(battle.Creatures[1]);

			var late = NewCreature(battle, 3, "Late");
			battle.AddCreature(late);

			late.Initiative.Should().Be(2);
		}
	}
}
