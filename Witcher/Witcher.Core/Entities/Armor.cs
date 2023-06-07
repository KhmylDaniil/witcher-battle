using System;
using System.Collections.Generic;
using System.Linq;

namespace Witcher.Core.Entities
{
	public class Armor : Item
	{
		public List<ArmorPart> ArmorParts { get; set; }

		protected Armor() { }

		public Armor(Character character, ArmorTemplate armorTemplate, int quantity) : base(character, armorTemplate, quantity)
		{
			IsEquipped = false;

			ArmorParts = new();
			foreach (CreaturePart creaturePart in character.CreatureParts
				.Where(x => armorTemplate.BodyTemplateParts.Select(x => x.BodyPartType).Contains(x.BodyPartType)))
			{
				ArmorParts.Add(new ArmorPart(this, creaturePart, armorTemplate.Armor));
			}
		}
	}
}
