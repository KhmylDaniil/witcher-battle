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
	/// Критический эффект - Потеря ноги
	/// </summary>
	public class DeadlyLegCritEffect : CritEffect, ISharedPenaltyCrit
	{
		/// <summary>
		/// Модификатор скорости
		/// </summary>
		public int SpeedModifier {get; private set; }

		/// <summary>
		/// Модификатор уклонения
		/// </summary>
		public int DodgeModifier { get; private set; }

		/// <summary>
		/// Модификатор атлетики
		/// </summary>
		public int AthleticsModifier { get; private set; }

		private DeadlyLegCritEffect() { }

		/// <summary>
		/// Конструктор эффекта потери ноги
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DeadlyLegCritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			SpeedModifier = (int)Math.Floor(creature.MaxSpeed * -0.75);
			
			DodgeModifier = (int)Math.Floor(creature.GetSkillMax(Skill.Dodge) * -0.75);

			AthleticsModifier = (int)Math.Floor(creature.GetSkillMax(Skill.Athletics) * -0.75);

			Severity = Severity.Deadly | Severity.Unstabilizied;
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
		/// <param name="name">название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static DeadlyLegCritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			if (!creature.Effects.Any(x => x is BleedEffect))
				creature.Effects.Add(BleedEffect.Create(null, null, creature, "Secondary Bleed"));

			CheckExistingEffectAndRemoveStabilizedEffect<DeadlyLegCritEffect>(creature, aimedPart);
            var effect = new DeadlyLegCritEffect(creature, aimedPart, name);

			ApplySharedPenalty(creature, effect);

			return effect;
		}

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			if (Severity == Severity.Deadly)
				return;

			Severity = Severity.Deadly;
		}

		/// <summary>
		/// Попробовать снять эффект
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="healer">Лекарь</param>
		/// <param name="patient">Цель</param>
		/// <param name="message">Сообщение</param>
		public override void Treat(IRollService rollService, Creature healer, Creature patient, ref StringBuilder message)
		{
			base.Treat(rollService, healer, patient, ref message);

			message.AppendLine("Конечность отсечена и не может быть восстановлена.");
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

			creature.Speed = creature.GetSpeed() - SpeedModifier;

			var dodge = creature.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Dodge);
			if (dodge is not null)
				dodge.SkillValue = dodge.GetValue() - DodgeModifier;

			var athletics = creature.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Athletics);
			if (athletics is not null)
				athletics.SkillValue = athletics.GetValue() - AthleticsModifier;
		}
	}
}
