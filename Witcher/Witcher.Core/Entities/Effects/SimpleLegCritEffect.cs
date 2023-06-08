using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using System.Collections.Generic;
using System.Linq;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - Вывих ноги
	/// </summary>
	public class SimpleLegCritEffect : CritEffect, ISharedPenaltyCrit
	{
		private const int Modifier = -2;
		private const int AfterTreatModifier = -1;
		private static readonly List<Skill> _affectedSkills = new() { Skill.Dodge, Skill.Athletics };
			
		private SimpleLegCritEffect() { }

		/// <summary>
		/// Конструктор эффекта вывиха ноги
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private SimpleLegCritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			Severity = Severity.Simple | Severity.Unstabilizied;
			BodyPartLocation = BodyPartType.Leg;
		}

		/// <summary>
		/// Пенальти применено
		/// </summary>
		public bool PenaltyApplied { get; private set; }

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static SimpleLegCritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			CheckExistingEffectAndRemoveStabilizedEffect<SimpleLegCritEffect>(creature, aimedPart);

			var effect = new SimpleLegCritEffect(creature, aimedPart, name);

			ApplySharedPenalty(creature, effect);

			return effect;
		}

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			if (IsStabile(Severity) || !PenaltyApplied)
				return;

			Severity = Enums.Severity.Simple;

			creature.Speed = creature.GetSpeed() - Modifier;
			creature.Speed = creature.GetSpeed() + AfterTreatModifier;

			var creatureSkills = creature.CreatureSkills.Where(x => _affectedSkills.Contains(x.Skill));

			foreach (var skill in creatureSkills)
			{
				skill.SkillValue = skill.GetValue() - Modifier;
				skill.SkillValue = skill.GetValue() + AfterTreatModifier;
			}

			SharedPenaltyMovedToAnotherCrit(creature, this);
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature)
		{
			PenaltyApplied = true;
			
			creature.Speed = creature.GetSpeed() + Modifier;
			
			var creatureSkills = creature.CreatureSkills.Where(x => _affectedSkills.Contains(x.Skill));

			foreach (var skill in creatureSkills)
				skill.SkillValue = skill.GetValue() + Modifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			PenaltyApplied = false;

			var creatureSkills = creature.CreatureSkills.Where(x => _affectedSkills.Contains(x.Skill));

			foreach (var skill in creatureSkills)
				if (Severity == Severity.Simple)
					skill.SkillValue = skill.GetValue() - AfterTreatModifier;
				else
					skill.SkillValue = skill.GetValue() - Modifier;

			creature.Speed = Severity == Severity.Simple
				? creature.Speed = creature.GetSpeed() - AfterTreatModifier
				: creature.Speed = creature.GetSpeed() - Modifier;
		}
	}
}
