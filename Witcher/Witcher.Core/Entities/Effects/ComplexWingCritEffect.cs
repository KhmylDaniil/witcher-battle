using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - Перелом крыла
	/// </summary>
	public class ComplexWingCritEffect : CritEffect, ISharedPenaltyCrit
	{
		private const int Modifier = -3;
		private const int AfterTreatModifier = -2;
		private static readonly List<Skill> _affectedSkills = new() { Skill.Dodge, Skill.Athletics };

		private ComplexWingCritEffect() { }

		/// <summary>
		/// Конструктор эффекта перелома крыла
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private ComplexWingCritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			Severity = Severity.Complex | Severity.Unstabilizied;
			BodyPartLocation = Enums.BodyPartType.Wing;
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
		public static ComplexWingCritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			var effect = CheckExistingEffectAndRemoveStabilizedEffect<ComplexWingCritEffect>(creature, aimedPart)
				? new ComplexWingCritEffect(creature, aimedPart, name)
				: null;

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

			Severity = Severity.Complex;

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
				if (Severity == Severity.Complex)
					skill.SkillValue = skill.GetValue() - AfterTreatModifier;
				else
					skill.SkillValue = skill.GetValue() - Modifier;

			creature.Speed = Severity == Severity.Complex
				? creature.GetSpeed() - AfterTreatModifier
				: creature.GetSpeed() - Modifier;
		}
	}
}
