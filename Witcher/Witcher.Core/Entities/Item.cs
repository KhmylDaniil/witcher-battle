using System;
using Witcher.Core.BaseData;
using Witcher.Core.Exceptions;
using Witcher.Core.Exceptions.EntityExceptions;

namespace Witcher.Core.Entities
{
	public abstract class Item : EntityBase
	{
		private Bag _bag;
		private ItemTemplate _itemTemplate;
		private int _quantity;

		public const string BagField = nameof(_bag);
		public const string ItemTemplateField = nameof(_itemTemplate);

		protected Item() { }

		protected Item(Bag bag, ItemTemplate itemTemplate, int quantity)
		{
			Bag = bag;
			ItemTemplate = itemTemplate;
			Quantity = quantity;
		}

		public Guid BagId { get; protected set; }

		public Guid ItemTemplateId { get; protected set; }

		public int Quantity
		{
			get => _quantity;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<Item>(nameof(Quantity));
				_quantity = value;
			}
		}

		#region navigation properties

		/// <summary>
		/// Шаблон предмета
		/// </summary>
		public ItemTemplate ItemTemplate
		{
			get => _itemTemplate;
			protected set
			{
				_itemTemplate = value ?? throw new EntityNotIncludedException<Item>(nameof(ItemTemplate));
				ItemTemplateId = value.Id;
			}
		}

		/// <summary>
		/// Сумка
		/// </summary>
		public Bag Bag
		{
			get => _bag;
			protected set
			{
				_bag = value ?? throw new EntityNotIncludedException<Item>(nameof(Bag));
				BagId = value.Id;
			}
		}

		#endregion navigation properties

		public static Item CreateItem(Bag bag, ItemTemplate itemTemplate, int quantity)
		{
			return itemTemplate switch
			{
				WeaponTemplate weaponTemplate => new Weapon(bag, weaponTemplate, quantity),
				_ => throw new LogicBaseException("Шаблон предмета не принадлежит к известным типам предметов.")
			};
		}
	}
}
