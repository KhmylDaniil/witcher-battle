﻿using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Logic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - Перелом руки
	/// </summary>
	public class ComplexArmCritEffect : CritEffect, ISharedPenaltyCrit
	{
		private const int SkillModifier = -3;
		private const int AfterTreatSkillModifier = -2;
		private static readonly List<Stats> AffectedStats = new()
		{
			Stats.Ref, Stats.Dex, Stats.Body, Stats.Cra
		};

		private ComplexArmCritEffect() { }

		/// <summary>
		/// Конструктор эффекта перелома руки
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private ComplexArmCritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			Severity = Severity.Complex | Severity.Unstabilizied;
			BodyPartLocation = Enums.BodyPartType.Arm;
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
		public static ComplexArmCritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			var effect = CheckExistingEffectAndRemoveStabilizedEffect<ComplexArmCritEffect>(creature, aimedPart)
				? new ComplexArmCritEffect(creature, aimedPart, name)
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

			if (CreaturePartId != creature.LeadingArmId)
				return;

			var creatureSkills = creature.CreatureSkills.Where(x => AffectedStats.Contains(CorrespondingStat(x.Skill)));

			foreach (var skill in creatureSkills)
			{
				skill.SkillValue = skill.GetValue() - SkillModifier;
				skill.SkillValue = skill.GetValue() + AfterTreatSkillModifier;
			}

			SharedPenaltyMovedToAnotherCrit(creature, this);
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature)
		{
			if (CreaturePartId != creature.LeadingArmId)
				return;

			PenaltyApplied = true;

			var creatureSkills = creature.CreatureSkills.Where(x => AffectedStats.Contains(CorrespondingStat(x.Skill)));

			foreach (var skill in creatureSkills)
				skill.SkillValue = skill.GetValue() + SkillModifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			if (CreaturePartId != creature.LeadingArmId)
				return;

			PenaltyApplied = false;

			var creatureSkills = creature.CreatureSkills.Where(x => AffectedStats.Contains(CorrespondingStat(x.Skill)));

			foreach (var skill in creatureSkills)
				if (Severity == Severity.Complex)
					skill.SkillValue = skill.GetValue() - AfterTreatSkillModifier;
				else
					skill.SkillValue = skill.GetValue() - SkillModifier;
		}
	}
}
