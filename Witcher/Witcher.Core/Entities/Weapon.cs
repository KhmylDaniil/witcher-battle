using Witcher.Core.Exceptions.EntityExceptions;

namespace Witcher.Core.Entities
{
	public class Weapon : Item
	{
		private int _currentDurability;

		protected Weapon() { }

		public Weapon(Character character, WeaponTemplate weaponTemplate, int quantity) : base(character, weaponTemplate, quantity)
		{
			CurrentDurability = weaponTemplate.Durability;
			IsEquipped = false;
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
	}
}
