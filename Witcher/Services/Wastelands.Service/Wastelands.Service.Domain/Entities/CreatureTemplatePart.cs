using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	public class CreatureTemplatePart : Entity
	{
		public long CreatureTemplateId { get; private set; }

		public string Name { get; private set; }

		public BodyPartType BodyPartType { get; private set; }

		public double DamageModifier { get; private set; }

		public int HitPenalty { get; private set; }

		public int MinToHit { get; private set; }

		public int MaxToHit { get; private set; }

		public int Armor { get; private set; }

		private CreatureTemplatePart()
		{
		}

		// Часть шаблона существа наследует геометрию удара от части шаблона тела, на котором построен
		// CreatureTemplate; CreatureTemplateId проставляет EF Core по связи CreatureTemplate.Parts (тот же
		// приём, что и у BodyTemplatePart — см. её конструктор).
		internal CreatureTemplatePart(BodyTemplatePart sourcePart)
		{
			Name = sourcePart.Name;
			BodyPartType = sourcePart.BodyPartType;
			DamageModifier = sourcePart.DamageModifier;
			HitPenalty = sourcePart.HitPenalty;
			MinToHit = sourcePart.MinToHit;
			MaxToHit = sourcePart.MaxToHit;
			Armor = 0;
		}
	}
}
