using System;
using System.Collections.Generic;
using Witcher.Core.Exceptions.EntityExceptions;

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
	}
}
