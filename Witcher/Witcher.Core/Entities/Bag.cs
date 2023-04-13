using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using Witcher.Core.Contracts.BagRequests;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Entities
{
	public class Bag : EntityBase
	{
		private Character _character;
		private int _maxWeight;
		private int _totalWeight;

		/// <summary>
		/// Поле для <see cref="_character"/>
		/// </summary>
		public const string CharacterField = nameof(_character);

		protected Bag() { }

		public Bag(Character character)
		{
			Character = character;
			MaxWeight = DefineMaxWeight(character);
			Items = new();
		}

		public Guid CharacterId { get; protected set; }

		/// <summary>
		/// Макимальный вес
		/// </summary>
		public int MaxWeight
		{
			get => _maxWeight;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<Bag>(nameof(MaxWeight));
				_maxWeight = value;
			}
		}

		/// <summary>
		/// Общий вес
		/// </summary>
		public int TotalWeight
		{
			get => _totalWeight;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<Bag>(nameof(TotalWeight));
				_totalWeight = value;
			}
		}

		#region navigation properties

		/// <summary>
		/// Перрсонаж
		/// </summary>
		public Character Character
		{
			get => _character;
			protected set
			{
				_character = value ?? throw new EntityNotIncludedException<Bag>(nameof(Character));
				CharacterId = value.Id;
			}
		}

		/// <summary>
		/// Экземпляры
		/// </summary>
		public List<Item> Items { get; set; }

		#endregion navigation properties

		private int DefineMaxWeight(Character character) => character.Body * 10;

		internal void AddItems(ItemTemplate itemTemplate, int quantity)
		{
			if (itemTemplate.IsStackable && Items.FirstOrDefault(x => x.ItemTemplateId == itemTemplate.Id) is Item currentItem && currentItem is not null)
			{
				currentItem.Quantity += quantity;
				return;
			}

			var switcher = itemTemplate.IsStackable ? quantity : 1;

			for (int i = switcher; i <= quantity; i++)
				Items.Add(Item.CreateItem(this, itemTemplate, switcher));
		}

		internal void RemoveItems(Item item, int quantity)
		{
			if (item.Quantity < quantity)
				throw new RequestFieldIncorrectDataException<RemoveItemFromBagCommand>(nameof(quantity), "Превышено текущее количество предметов.");
			else if (item.Quantity > quantity)
				item.Quantity -= quantity;
			else
				Items.Remove(item);
		}
	}
}
