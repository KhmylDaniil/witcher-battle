using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	public class AbilityAppliedCondition : Entity
	{
		public const int MinApplyChance = 1;
		public const int MaxApplyChance = 100;

		public long AbilityId { get; private set; }

		public Condition Condition { get; private set; }

		public int ApplyChance { get; private set; }

		private AbilityAppliedCondition()
		{
		}

		public AbilityAppliedCondition(long abilityId, Condition condition, int applyChance)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(abilityId, nameof(abilityId));
			InvalidArgumentException.ThrowIfNotInRange(applyChance, MinApplyChance, MaxApplyChance, nameof(applyChance));

			AbilityId = abilityId;
			Condition = condition;
			ApplyChance = applyChance;
		}

		// AbilityId проставляет EF Core по связи Ability.AppliedConditions — используется только
		// Ability.ForEquippedWeapon, где Ability ещё не сохранена (Id == 0) и настоящий abilityId
		// заранее неизвестен. Тот же приём, что CreatureTemplatePart(BodyTemplatePart) для CreatureTemplate.Parts.
		internal AbilityAppliedCondition(Condition condition, int applyChance)
		{
			InvalidArgumentException.ThrowIfNotInRange(applyChance, MinApplyChance, MaxApplyChance, nameof(applyChance));

			Condition = condition;
			ApplyChance = applyChance;
		}

		public void ChangeCondition(Condition condition, int applyChance)
		{
			InvalidArgumentException.ThrowIfNotInRange(applyChance, MinApplyChance, MaxApplyChance, nameof(applyChance));

			Condition = condition;
			ApplyChance = applyChance;
		}
	}
}
