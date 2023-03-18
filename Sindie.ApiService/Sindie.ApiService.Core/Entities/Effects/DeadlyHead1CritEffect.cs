using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Logic;
using System;
using System.Linq;
using System.Text;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - повреждение глаза
	/// </summary>
	public class DeadlyHead1CritEffect : CritEffect, ICrit
	{
		private const int DexModifier = -4;
		private const int AfterTreatDexModifier = -2;

		private const int AwarenessModifier = -5;
		private const int AfterTreatAwarenessModifier = -3;

		public DeadlyHead1CritEffect() { }

		/// <summary>
		/// Конструктор эффекта повреждения глаза
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DeadlyHead1CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			ApplyStatChanges(creature);
			Severity = Severity.Deadly | Severity.Unstabilizied;
			BodyPartLocation = Enums.BodyPartType.Head;
		}
			

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static DeadlyHead1CritEffect Create(Creature creature, CreaturePart aimedPart, string name)
			=> CheckExistingEffectAndRemoveStabilizedEffect<DeadlyHead1CritEffect>(creature, aimedPart)
				? new DeadlyHead1CritEffect(creature, aimedPart, name)
				: null;

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature)
		{
			creature.Dex = creature.GetDex() + DexModifier;

			var awareness = creature.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Awareness);

			if (awareness != null)
				awareness.SkillValue = awareness.GetValue() + AwarenessModifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			var awareness = creature.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Awareness);

			if (IsStabile(Severity))
			{
				creature.Dex = creature.GetDex() - AfterTreatDexModifier;

				if (awareness != null)
					awareness.SkillValue = awareness.GetValue() - AfterTreatAwarenessModifier;
			}
			else
			{
				creature.Dex = creature.GetDex() - DexModifier;

				if (awareness != null)
					awareness.SkillValue = awareness.GetValue() - AwarenessModifier;
			}
		}

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			if (!IsStabile(Severity))

				Severity = Severity.Deadly;

			creature.Dex = creature.GetDex() - DexModifier + AfterTreatDexModifier;

			var awareness = creature.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Awareness);

			if (awareness != null)
				awareness.SkillValue = awareness.GetValue() - AwarenessModifier + AfterTreatAwarenessModifier;
		}
	}
}
