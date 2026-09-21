using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	/// <summary>
	/// Покрытие шаблона брони — одна часть тела и одно значение брони. Прочность экземпляра брони на
	/// этой части (см. ItemArmorPart) стартует равной этому же значению — отдельного параметра
	/// "максимальная прочность" нет, он всегда равен значению брони.
	/// </summary>
	public class ItemTemplateArmorPart : Entity
	{
		public long ItemTemplateId { get; private set; }

		public HumanBodyPart Part { get; private set; }

		public int Armor { get; private set; }

		private ItemTemplateArmorPart()
		{
		}

		public ItemTemplateArmorPart(long itemTemplateId, HumanBodyPart part, int armor)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(itemTemplateId, nameof(itemTemplateId));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(armor, nameof(armor));

			ItemTemplateId = itemTemplateId;
			Part = part;
			Armor = armor;
		}

		public void Change(int armor)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(armor, nameof(armor));

			Armor = armor;
		}
	}
}
