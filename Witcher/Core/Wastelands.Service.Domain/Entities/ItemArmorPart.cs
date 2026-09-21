using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	/// <summary>
	/// Покрытие экземпляра брони — снапшот ItemTemplateArmorPart в момент добавления в инвентарь, плюс
	/// собственная текущая прочность этой части (изнашивается в бою, чинится мастером — см.
	/// Item.WearArmor/RepairArmorPart). Прочность — это и есть эффективное значение брони: чем больше
	/// часть изношена, тем меньше урона она поглощает (см. BattleCombatCalculator).
	/// </summary>
	public class ItemArmorPart : Entity
	{
		public long ItemId { get; private set; }

		public HumanBodyPart Part { get; private set; }

		/// <summary>Значение брони на этой части — оно же стартовая и максимальная прочность.</summary>
		public int Armor { get; private set; }

		public int CurrentDurability { get; private set; }

		private ItemArmorPart()
		{
		}

		// ItemId проставляет EF Core по связи Item.ArmorParts — тот же приём, что и у ItemAppliedCondition.
		internal ItemArmorPart(HumanBodyPart part, int armor)
		{
			Part = part;
			Armor = armor;
			CurrentDurability = armor;
		}

		/// <summary>Износ от одного попадания в эту часть — на 1 очко прочности, независимо от того, поглотила ли броня урон.</summary>
		public void Wear()
		{
			CurrentDurability = Math.Max(0, CurrentDurability - 1);
		}

		public void Repair(int durability)
		{
			InvalidArgumentException.ThrowIfNotInRange(durability, 0, Armor, nameof(durability));
			CurrentDurability = durability;
		}
	}
}
