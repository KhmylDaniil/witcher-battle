using System.Collections.Generic;
using System.Linq;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Броня
	/// </summary>
	public class Armor : Item
	{
		/// <summary>
		/// Части брони
		/// </summary>
		public List<ArmorPart> ArmorParts { get; set; }

		protected Armor() { }

		public Armor(Character character, ArmorTemplate armorTemplate, int quantity) : base(character, armorTemplate, quantity)
		{
			IsEquipped = false;

			ArmorParts = new();
			foreach (var part in armorTemplate.BodyTemplateParts)
				ArmorParts.Add(new ArmorPart(this, part, armorTemplate.Armor));
		}

		internal void ChangeArmorEquippedStatus(Character character)
		{
			foreach (var armorPart in ArmorParts)
				if (IsEquipped.Value)
					armorPart.CreaturePart = null;
				else
					armorPart.CreaturePart = character.CreatureParts.FirstOrDefault(x => x.Name == armorPart.Name);
		}
	}
}
