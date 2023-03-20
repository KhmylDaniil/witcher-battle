using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Logic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - Открытый перелом руки
	/// </summary>
	public class DifficultArmCritEffect : CritEffect, ISharedPenaltyCrit
	{
		private static readonly List<Stats> AffectedStats = new()
		{
			Stats.Ref, Stats.Dex, Stats.Body, Stats.Cra
		};

		private DifficultArmCritEffect() { }

		/// <summary>
		/// Конструктор эффекта открытого перелома руки
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DifficultArmCritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			Severity = Severity.Difficult | Severity.Unstabilizied;
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
		public static DifficultArmCritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			if (!creature.Effects.Any(x => x is BleedEffect))
				creature.Effects.Add(BleedEffect.Create(null, null, creature, "Secondary Bleed"));

			var effect = CheckExistingEffectAndRemoveStabilizedEffect<DifficultArmCritEffect>(creature, aimedPart)
				? new DifficultArmCritEffect(creature, aimedPart, name)
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

			Severity = Severity.Difficult;

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
				skill.SkillValue = skill.GetValue() - skill.MaxValue / 2;
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
				skill.SkillValue = skill.GetValue() + skill.MaxValue / 2;
		}
	}
}
