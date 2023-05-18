using System;
using Witcher.Core.Exceptions;
using Witcher.Core.Exceptions.EntityExceptions;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities
{
	public abstract class Item : EntityBase
	{
		private Character _character;
		private ItemTemplate _itemTemplate;
		private int _quantity;

		public const string CharacterField = nameof(_character);
		public const string ItemTemplateField = nameof(_itemTemplate);

		protected Item() { }

		protected Item(Character character, ItemTemplate itemTemplate, int quantity)
		{
			Character = character;
			ItemTemplate = itemTemplate;
			Quantity = quantity;
			ItemType = itemTemplate.ItemType;
		}

		public Guid CharacterId { get; protected set; }

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

		/// <summary>
		/// Тип предмета
		/// </summary>
		public ItemType ItemType { get; set; }

		public bool? IsEquipped { get; set; }

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
		/// Персонаж
		/// </summary>
		public Character Character
		{
			get => _character;
			protected set
			{
				_character = value ?? throw new EntityNotIncludedException<Item>(nameof(Character));
				CharacterId = value.Id;
			}
		}

		#endregion navigation properties

		public static Item CreateItem(Character character, ItemTemplate itemTemplate, int quantity)
		{
			return itemTemplate.ItemType switch
			{
				ItemType.Weapon => new Weapon(character, (WeaponTemplate)itemTemplate, quantity),
				_ => throw new LogicBaseException("Шаблон предмета не принадлежит к известным типам предметов.")
			};
		}
	}
}
