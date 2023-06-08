using Witcher.Core.Abstractions;
using System.Linq;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - проломленный череп
	/// </summary>
	public class DifficultHead2CritEffect : CritEffect, ICrit
	{
		private const int IntAndDexModifier = -1;

		public DifficultHead2CritEffect() { }

		/// <summary>
		/// Конструктор эффекта проломленного черепа
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DifficultHead2CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			ApplyStatChanges(creature);
			Severity = Severity.Difficult | Severity.Unstabilizied;
			BodyPartLocation = BodyPartType.Head;
		}

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static DifficultHead2CritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			if (!creature.Effects.Any(x => x is BleedEffect))
				creature.Effects.Add(BleedEffect.Create(null, null, creature, "Secondary Bleed"));

			CheckExistingEffectAndRemoveStabilizedEffect<DifficultHead2CritEffect>(creature, aimedPart);
			return new DifficultHead2CritEffect(creature, aimedPart, name);
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature)
		{
			creature.Int = creature.GetRef() + IntAndDexModifier;
			creature.Dex = creature.GetDex() + IntAndDexModifier;

			creature.CreatureParts.FirstOrDefault(x => x.Id == CreaturePartId).DamageModifier++;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			creature.Int = creature.GetRef() - IntAndDexModifier;
			creature.Dex = creature.GetDex() - IntAndDexModifier;

			creature.CreatureParts.FirstOrDefault(x => x.Id == CreaturePartId).DamageModifier--;
		}

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			Severity = Severity.Difficult;
		}
	}
}
