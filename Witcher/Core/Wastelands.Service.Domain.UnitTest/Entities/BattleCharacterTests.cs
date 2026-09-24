using FluentAssertions;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Domain.UnitTest.TestSupport;

namespace Wastelands.Service.Domain.UnitTest.Entities
{
	[TestClass]
	public class BattleCharacterTests
	{
		[TestMethod]
		public void Constructor_CopiesHpAndStaFromCharacter()
		{
			var character = TestBuilders.Character(hp: 20, sta: 15);

			var battleCharacter = TestBuilders.BattleCharacter(battleId: 1, character);

			battleCharacter.MaxHP.Should().Be(20);
			battleCharacter.CurrentHP.Should().Be(20);
			battleCharacter.MaxSta.Should().Be(15);
			battleCharacter.CurrentSta.Should().Be(15);
		}

		[TestMethod]
		public void ApplyDamage_CanDropCurrentHpBelowZero()
		{
			var battleCharacter = TestBuilders.BattleCharacter(1, TestBuilders.Character(hp: 10));

			battleCharacter.ApplyDamage(15);

			// В отличие от Creature, у персонажа HP не ограничен нулём снизу — отрицательные HP это
			// счётчик "насколько глубоко при смерти" (см. StabilizeAsync difficulty = |CurrentHP|).
			battleCharacter.CurrentHP.Should().Be(-5);
		}

		[TestMethod]
		public void ApplyDamage_NegativeDamage_Throws()
		{
			var battleCharacter = TestBuilders.BattleCharacter(1, TestBuilders.Character());

			var act = () => battleCharacter.ApplyDamage(-1);

			act.Should().Throw<InvalidArgumentException>();
		}

		[TestMethod]
		public void Stabilize_SetsHpToOneAndRemovesDyingCondition()
		{
			var battleCharacter = TestBuilders.BattleCharacter(1, TestBuilders.Character(hp: 10));
			battleCharacter.ApplyDamage(15);
			battleCharacter.AddCondition(Condition.Dying);
			battleCharacter.AddCondition(Condition.Bleed);

			battleCharacter.Stabilize();

			battleCharacter.CurrentHP.Should().Be(1);
			battleCharacter.AppliedConditions.Should().NotContain(Condition.Dying);
			battleCharacter.AppliedConditions.Should().Contain(Condition.Bleed);
		}

		[TestMethod]
		public void SpendStamina_FloorsAtZero()
		{
			var battleCharacter = TestBuilders.BattleCharacter(1, TestBuilders.Character(sta: 5));

			battleCharacter.SpendStamina(8);

			battleCharacter.CurrentSta.Should().Be(0);
		}

		[TestMethod]
		public void SetInitiative_WhenAlreadySet_Throws()
		{
			var battleCharacter = TestBuilders.BattleCharacter(1, TestBuilders.Character());
			battleCharacter.SetInitiative(1);

			var act = () => battleCharacter.SetInitiative(2);

			act.Should().Throw<InvalidArgumentException>();
			battleCharacter.Initiative.Should().Be(1);
		}

		[TestMethod]
		public void AddCondition_IsIdempotent()
		{
			var battleCharacter = TestBuilders.BattleCharacter(1, TestBuilders.Character());

			battleCharacter.AddCondition(Condition.Bleed);
			battleCharacter.AddCondition(Condition.Bleed);

			battleCharacter.AppliedConditions.Should().ContainSingle(c => c == Condition.Bleed);
		}

		[TestMethod]
		public void MarkActedThisTurn_SetsHasActedThisTurnTrue()
		{
			var battleCharacter = TestBuilders.BattleCharacter(1, TestBuilders.Character());
			battleCharacter.MarkActedThisTurn();

			battleCharacter.HasActedThisTurn.Should().BeTrue();
		}
	}
}
