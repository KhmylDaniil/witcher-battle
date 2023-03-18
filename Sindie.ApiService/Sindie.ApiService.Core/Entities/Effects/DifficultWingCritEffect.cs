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
	/// Критический эффект - Открытый перелом крыла
	/// </summary>
	public class DifficultWingCritEffect : CritEffect, ISharedPenaltyCrit
	{
		/// <summary>
		/// Модификатор скорости
		/// </summary>
		public int SpeedModifier { get; private set; }

		/// <summary>
		/// Модификатор скорости после стабилизации
		/// </summary>
		public int AfterTreatSpeedModifier { get; private set; }

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

		private DifficultWingCritEffect() { }

		/// <summary>
		/// Конструктор эффекта открытого перелома крыла
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DifficultWingCritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			SpeedModifier = (int)Math.Floor(creature.MaxSpeed * -0.75);
			AfterTreatSpeedModifier = (int)Math.Floor(creature.MaxSpeed * -0.5);

			DodgeModifier = (int)Math.Floor(creature.GetSkillMax(Skill.Dodge) * -0.75);
			AfterTreatDodgeModifier = (int)Math.Floor(creature.GetSkillMax(Skill.Dodge) * -0.5);

			AthleticsModifier = (int)Math.Floor(creature.GetSkillMax(Skill.Athletics) * -0.75);
			AfterTreatAthleticsModifier = (int)Math.Floor(creature.GetSkillMax(Skill.Athletics) * -0.5);

			Severity = Severity.Difficult | Severity.Unstabilizied;
			BodyPartLocation = BodyPartType.Wing;
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
		public static DifficultWingCritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			if (!creature.Effects.Any(x => x is BleedEffect))
				creature.Effects.Add(BleedEffect.Create(null, null, creature, "Secondary Bleed"));

			var effect = CheckExistingEffectAndRemoveStabilizedEffect<DifficultWingCritEffect>(creature, aimedPart)
				? new DifficultWingCritEffect(creature, aimedPart, name)
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

			creature.Speed = creature.GetSpeed() - SpeedModifier;
			creature.Speed = creature.GetSpeed() + AfterTreatSpeedModifier;

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

			creature.Speed = creature.GetSpeed() + SpeedModifier;

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
				creature.Speed -= AfterTreatSpeedModifier;

				if (dodge != null)
					dodge.SkillValue = dodge.GetValue() - AfterTreatDodgeModifier;

				if (athletics != null)
					athletics.SkillValue = athletics.GetValue() - AfterTreatAthleticsModifier;
			}
			else
			{
				creature.Speed = creature.GetSpeed() - SpeedModifier;

				if (dodge != null)
					dodge.SkillValue = dodge.GetValue() - DodgeModifier;

				if (athletics != null)
					athletics.SkillValue = athletics.GetValue() - AthleticsModifier;
			}
		}
	}
}
