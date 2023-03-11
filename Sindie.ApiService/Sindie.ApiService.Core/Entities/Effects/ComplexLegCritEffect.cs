using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - Перелом ноги
	/// </summary>
	public class ComplexLegCritEffect : CritEffect, ISharedPenaltyCrit
	{
		private const int Modifier = -3;
		private const int AfterTreatModifier = -2;
		private static readonly List<Skill> _affectedSkills = new() { Skill.Dodge, Skill.Athletics };

		private ComplexLegCritEffect() { }

		/// <summary>
		/// Конструктор эффекта перелома ноги
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private ComplexLegCritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			Severity = Severity.Complex | Severity.Unstabilizied;
			BodyPartLocation = Enums.BodyPartType.Leg;
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
		public static ComplexLegCritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			var effect = CheckExistingEffectAndRemoveStabilizedEffect<ComplexLegCritEffect>(creature, aimedPart)
				? new ComplexLegCritEffect(creature, aimedPart, name)
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
				? creature.Speed = creature.GetSpeed() - AfterTreatModifier
				: creature.Speed = creature.GetSpeed() - Modifier;
		}
	}
}
