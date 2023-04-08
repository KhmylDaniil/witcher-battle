using System;
using System.Collections.Generic;
using Witcher.Core.Exceptions.EntityExceptions;

namespace Witcher.Core.Entities
{
	public class Item : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		private Game _game;
		private int _price;
		private int _quantity;
		private int _weight;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected Item()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="isStackable">Можно ли положить в стак</param>
		/// <param name="quantity">Количество</param>
		/// <param name="price">Цена</param>
		/// <param name="weight">Вес</param>
		public Item(
			Game game,
			string name,
			string description,
			bool isStackable,
			int quantity,
			int price,
			int weight)
		{
			Game = game;
			Name = name;
			Description = description;
			IsStackable = isStackable;
			Quantity = quantity;
			Weight = weight;
			Price = price;
		}

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; protected set; }

		/// <summary>
		/// Наазвание
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Вес
		/// </summary>
		public int Weight
		{
			get => _weight;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<Item>(nameof(Weight));
				_weight = value;
			}
		}

		/// <summary>
		/// Стоимость
		/// </summary>
		public int Price
		{
			get => _price;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<Item>(nameof(Price));
				_price = value;
			}
		}

		/// <summary>
		/// Можно ли положить предмет в стак
		/// </summary>
		public bool IsStackable { get; set; }

		/// <summary>
		/// Количество
		/// </summary>
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
		/// Игра
		/// </summary>
		public Game Game
		{
			get => _game;
			protected set
			{
				_game = value ?? throw new EntityNotIncludedException<Ability>(nameof(Game));
				GameId = value.Id;
			}
		}

		/// <summary>
		/// Персонажи
		/// </summary>
		public List<Character> Characters { get; set; } = new();

		#endregion navigation properties
	}
}
