using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Тело
	/// </summary>
	public class Body : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_character"/>
		/// </summary>
		public const string CharacterField = nameof(_character);

		/// <summary>
		/// Поле для <see cref="_slot"/>
		/// </summary>
		public const string SlotField = nameof(_slot);

		/// <summary>
		/// Поле для <see cref="_item"/>
		/// </summary>
		public const string ItemField = nameof(_item);

		private Character _character;
		private Slot _slot;
		private Item _item;

		/// <summary>
		/// Айди персонажа
		/// </summary>
		public Guid CharacterId { get; protected set; }

		/// <summary>
		/// Айди слота
		/// </summary>
		public Guid SlotId { get; protected set; }

		/// <summary>
		/// Айди предмета
		/// </summary>
		public Guid? ItemId { get; protected set; }

		#region navigation properties

		/// <summary>
		/// Персонаж
		/// </summary>
		public Character Character
		{
			get => _character;
			set
			{
				_character = value ?? throw new ApplicationException("Нужно передать персонажа");
				CharacterId = value.Id;
			}
		}

		/// <summary>
		/// Слот
		/// </summary>
		public Slot Slot
		{
			get => _slot;
			set
			{
				_slot = value ?? throw new ApplicationException("Нужно передать слот");
				SlotId = value.Id;
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
				_item = value;
				ItemId = value?.Id;
			}
		}
		#endregion navigation properties
	}
}
