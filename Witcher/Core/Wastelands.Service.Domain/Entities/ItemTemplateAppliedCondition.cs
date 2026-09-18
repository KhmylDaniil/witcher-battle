using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	public class ItemTemplateAppliedCondition : Entity
	{
		public const int MinApplyChance = 1;
		public const int MaxApplyChance = 100;

		public long ItemTemplateId { get; private set; }

		public Condition Condition { get; private set; }

		public int ApplyChance { get; private set; }

		private ItemTemplateAppliedCondition()
		{
		}

		public ItemTemplateAppliedCondition(long itemTemplateId, Condition condition, int applyChance)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(itemTemplateId, nameof(itemTemplateId));
			InvalidArgumentException.ThrowIfNotInRange(applyChance, MinApplyChance, MaxApplyChance, nameof(applyChance));

			ItemTemplateId = itemTemplateId;
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
