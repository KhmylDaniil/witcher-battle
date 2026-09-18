using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	/// <summary>
	/// Накладываемое состояние экземпляра предмета — копируется из ItemTemplateAppliedCondition при
	/// создании Item (см. Item(long, ItemTemplate)), дальше живёт независимо от шаблона.
	/// </summary>
	public class ItemAppliedCondition : Entity
	{
		public long ItemId { get; private set; }

		public Condition Condition { get; private set; }

		public int ApplyChance { get; private set; }

		private ItemAppliedCondition()
		{
		}

		// ItemId проставляет EF Core по связи Item.AppliedConditions — тот же приём, каким
		// CreatureTemplatePart(BodyTemplatePart) копирует часть тела в новый шаблон существа.
		internal ItemAppliedCondition(Condition condition, int applyChance)
		{
			Condition = condition;
			ApplyChance = applyChance;
		}
	}
}
