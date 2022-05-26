using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Предметы в сумке
	/// </summary>
	public class BagItem : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_bag"/>
		/// </summary>
		public const string BagField = nameof(_bag);

		/// <summary>
		/// Поле для <see cref="_item"/>
		/// </summary>
		public const string ItemField = nameof(_item);

		private Bag _bag;
		private Item _item;
		private int _quantityItem;
		private int _maxQuantityItem;
		private int _stack;
		private int _blocked;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected BagItem()
		{
		}

		/// <summary>
		/// Конструктор для предмета в сумке
		/// </summary>
		/// <param name="bag">Сумка</param>
		/// <param name="item">Предмет</param>
		/// <param name="quantityItem">Количество предметов в стеке</param>
		/// <param name="maxQuantityItem">Максимальное количество предметов в стеке</param>
		/// <param name="stack">Стек</param>
		/// <param name="blocked">Количество заблокированных предметов</param>
		public BagItem(
			Bag bag,
			Item item,
			int quantityItem,
			int maxQuantityItem,
			int stack,
			int blocked = default)
		{
			Bag = bag;
			Item = item;
			QuantityItem = quantityItem;
			MaxQuantityItem = maxQuantityItem;
			Stack = stack;
			Blocked = blocked;
		}

		/// <summary>
		/// Айди сумки
		/// </summary>
		public Guid BagId { get; protected set; }

		/// <summary>
		/// Айди предмета
		/// </summary>
		public Guid ItemId { get; protected set; }

		/// <summary>
		/// Вес
		/// </summary>
		public double Weight { get; protected set; }

		/// <summary>
		/// Количество предметов в стеке
		/// </summary>
		public int QuantityItem
		{
			get => _quantityItem;
			set
			{
				if (value < 0)
					throw new ExceptionFieldOutOfRange<BagItem>(nameof(QuantityItem));
				_quantityItem = value;
			}
		}

		/// <summary>
		/// Максимальное количество предметов в стеке
		/// </summary>
		public int MaxQuantityItem
		{
			get => _maxQuantityItem;
			set
			{
				if (value < 0)
					throw new ExceptionFieldOutOfRange<BagItem>(nameof(MaxQuantityItem));
				_maxQuantityItem = value;
			}
		}

		/// <summary>
		/// Стек
		/// </summary>
		public int Stack
		{
			get => _stack;
			set
			{
				if (value < 0)
					throw new ExceptionFieldOutOfRange<BagItem>(nameof(Stack));
				_stack = value;
			}
		}

		/// <summary>
		/// Количество заблокированных предметов
		/// </summary>
		public int Blocked
		{
			get => _blocked;
			set
			{
				if (value < 0 || value > QuantityItem)
					throw new ExceptionFieldOutOfRange<BagItem>(nameof(Blocked));
				_blocked = value;
			}
		}

		#region navigation properties

		/// <summary>
		/// Сумка
		/// </summary>
		public Bag Bag
		{
			get => _bag;
			set
			{
				_bag = value ?? throw new ApplicationException("Необходимо передать сумку");
				BagId = value.Id;
			}
		}

		/// <summary>
		/// Предмет
		/// </summary>
		public Item Item
		{
			get => _item;
			set
			{
				_item = value ?? throw new ApplicationException("Необходимо передать предмет");
				ItemId = value.Id;
				Weight = value.Weight;
			}
		}
		#endregion navigation properties
	}
}
