using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System;
using System.Linq;
using System.Text;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - Открытый перелом хвоста
	/// </summary>
	public class DifficultTailCritEffect : CritEffect, ISharedPenaltyCrit
	{
		/// <summary>
		/// Модификатор уклонения
		/// </summary>
		public int DodgeModifier { get; private set; }

		/// <summary>
		/// Модификатор уклонения после стабилизации
		/// </summary>
		public int AfterTreatDodgeModifier { get; private set; }

		/// <summary>
		/// Модификатор атлетики
		/// </summary>
		public int AthleticsModifier { get; private set; }

		/// <summary>
		/// Модификатор атлетики после стабилизации
		/// </summary>
		public int AfterTreatAthleticsModifier { get; private set; }

		private DifficultTailCritEffect() { }

		/// <summary>
		/// Конструктор эффекта открытого перелома хвоста
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DifficultTailCritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			DodgeModifier = (int)Math.Floor(creature.GetSkillMax(Skill.Dodge) * -0.75);
			AfterTreatDodgeModifier = (int)Math.Floor(creature.GetSkillMax(Skill.Dodge) * -0.5);

			AthleticsModifier = (int)Math.Floor(creature.GetSkillMax(Skill.Athletics) * -0.75);
			AfterTreatAthleticsModifier = (int)Math.Floor(creature.GetSkillMax(Skill.Athletics) * -0.5);

			Severity = Severity.Difficult | Severity.Unstabilizied;
			BodyPartLocation = Enums.BodyPartType.Tail;
		}

		/// <summary>
		/// Пенальти применено
		/// </summary>
		public bool PenaltyApplied { get; private set; }

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static DifficultTailCritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			if (!creature.Effects.Any(x => x is BleedEffect))
				creature.Effects.Add(BleedEffect.Create(null, null, creature, "Secondary Bleed"));

			var effect = CheckExistingEffectAndRemoveStabilizedEffect<DifficultTailCritEffect>(creature, aimedPart)
				? new DifficultTailCritEffect(creature, aimedPart, name)
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

			var dodge = creature.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Dodge);
			if (dodge is not null)
			{
				dodge.SkillValue = dodge.GetValue() - DodgeModifier;
				dodge.SkillValue = dodge.GetValue() + AfterTreatDodgeModifier;
			}

			var athletics = creature.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Athletics);
			if (athletics is not null)
			{
				athletics.SkillValue = athletics.GetValue() - AthleticsModifier;
				athletics.SkillValue = athletics.GetValue() + AfterTreatAthleticsModifier;
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

			var dodge = creature.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Dodge);
			if (dodge is not null)
				dodge.SkillValue = dodge.GetValue() + DodgeModifier;

			var athletics = creature.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Athletics);
			if (athletics is not null)
				athletics.SkillValue = athletics.GetValue() + AthleticsModifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			PenaltyApplied = false;

			var dodge = creature.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Dodge);
			var athletics = creature.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Athletics);

			if (Severity == Severity.Difficult)
			{

				if (dodge != null)
					dodge.SkillValue = dodge.GetValue() - AfterTreatDodgeModifier;

				if (athletics != null)
					athletics.SkillValue = athletics.GetValue() - AfterTreatAthleticsModifier;
			}
			else
			{
				if (dodge != null)
					dodge.SkillValue = dodge.GetValue() - DodgeModifier;

				if (athletics != null)
					athletics.SkillValue = athletics.GetValue() - AthleticsModifier;
			}
		}
	}
}
