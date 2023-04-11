using Witcher.Core.Exceptions.EntityExceptions;
using System;

namespace Witcher.Core.Entities
{
	public class Weapon : Item
	{
		private Character _equippedByCharacter;
		public const string EquippedByCharacterField = nameof(_equippedByCharacter);
		private int _currentDurability;

		protected Weapon() { }

		public Weapon(Bag bag, WeaponTemplate weaponTemplate, int quantity) : base(bag, weaponTemplate, quantity)
		{
			CurrentDurability = weaponTemplate.Durability;
		}

		public int CurrentDurability
		{
			get => _currentDurability;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<Weapon>(nameof(CurrentDurability));
				_currentDurability = value;
			}
		}

		public Guid? EquippedByCharacterId { get; protected set; }

		#region navigation properties
		public Character EquippedByCharacter
		{ 
			get => _equippedByCharacter;
			set
			{
				_equippedByCharacter = value;
				EquippedByCharacterId = value?.Id;
			}
		}

		#endregion navigation properties
	}
}
