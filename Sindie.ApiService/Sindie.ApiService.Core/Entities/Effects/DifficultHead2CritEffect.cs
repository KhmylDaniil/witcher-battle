using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System.Linq;
using System.Text;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - проломленный череп
	/// </summary>
	public class DifficultHead2CritEffect : CritEffect, ICrit
	{
		private const int IntAndDexModifier = -1;

		/// <summary>
		/// Тяжесть критического эффекта
		/// </summary>
		public Severity Severity { get; private set; } = Severity.Difficult | Severity.Unstabilizied;

		/// <summary>
		/// Тип части тела
		/// </summary
		public BodyPartType BodyPartLocation { get; } = BodyPartType.Head;

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
			BodyPartLocation = Enums.BodyPartType.Head;
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

			return CheckExistingEffectAndRemoveStabilizedEffect<DifficultHead2CritEffect>(creature, aimedPart)
				? new DifficultHead2CritEffect(creature, aimedPart, name)
				: null;
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature)
		{
			creature.Int = creature.GetRef() + IntAndDexModifier;
			creature.Dex = creature.GetDex() + IntAndDexModifier;

			creature.CreatureParts.FirstOrDefault(x => x.Id == this.CreaturePartId).DamageModifier++;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			creature.Int = creature.GetRef() - IntAndDexModifier;
			creature.Dex = creature.GetDex() - IntAndDexModifier;

			creature.CreatureParts.FirstOrDefault(x => x.Id == this.CreaturePartId).DamageModifier--;
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
