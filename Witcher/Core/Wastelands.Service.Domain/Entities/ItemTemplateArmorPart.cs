using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	/// <summary>Покрытие шаблона брони: значение брони и максимальная прочность для одной части тела.</summary>
	public class ItemTemplateArmorPart : Entity
	{
		public long ItemTemplateId { get; private set; }

		public HumanBodyPart Part { get; private set; }

		public int ArmorValue { get; private set; }

		public int MaxDurability { get; private set; }

		private ItemTemplateArmorPart()
		{
		}

		public ItemTemplateArmorPart(long itemTemplateId, HumanBodyPart part, int armorValue, int maxDurability)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(itemTemplateId, nameof(itemTemplateId));
			InvalidArgumentException.ThrowIfLessThanZero(armorValue, nameof(armorValue));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(maxDurability, nameof(maxDurability));

			ItemTemplateId = itemTemplateId;
			Part = part;
			ArmorValue = armorValue;
			MaxDurability = maxDurability;
		}

		public void Change(int armorValue, int maxDurability)
		{
			InvalidArgumentException.ThrowIfLessThanZero(armorValue, nameof(armorValue));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(maxDurability, nameof(maxDurability));

			ArmorValue = armorValue;
			MaxDurability = maxDurability;
		}
	}
}
