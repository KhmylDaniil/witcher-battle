using System;
using System.Collections.Generic;
using Witcher.Core.Exceptions.EntityExceptions;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities
{
	public abstract class ItemTemplate : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		private Game _game;
		private int _price;
		private int _weight;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected ItemTemplate()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="isStackable">Можно ли положить в стак</param>
		/// <param name="price">Цена</param>
		/// <param name="weight">Вес</param>
		protected ItemTemplate(
			Game game,
			string name,
			string description,
			bool isStackable,
			int price,
			int weight)
		{
			Game = game;
			Name = name;
			Description = description;
			IsStackable = isStackable;
			Weight = weight;
			Price = price;
			Exemplars = new();
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
					throw new FieldOutOfRangeException<ItemTemplate>(nameof(Weight));
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
					throw new FieldOutOfRangeException<ItemTemplate>(nameof(Price));
				_price = value;
			}
		}

		/// <summary>
		/// Можно ли положить предмет в стак
		/// </summary>
		public bool IsStackable { get; set; }

		/// <summary>
		/// Тип предмета
		/// </summary>
		public ItemType ItemType { get; set; }

		#region navigation properties

		/// <summary>
		/// Игра
		/// </summary>
		public Game Game
		{
			get => _game;
			protected set
			{
				_game = value ?? throw new EntityNotIncludedException<ItemTemplate>(nameof(Game));
				GameId = value.Id;
			}
		}

		/// <summary>
		/// Экземпляры
		/// </summary>
		public List<Item> Exemplars { get; set; }

		#endregion navigation properties

		protected void ChangeItemTemplate(string name, string description, bool isStackable, int price, int weight)
		{
			Name = name;
			Description = description;
			IsStackable = isStackable;
			Price = price;
			Weight = weight;
		}
	}
}
