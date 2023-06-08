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
	/// Критический эффект - потеря руки
	/// </summary>
	public class DeadlyArmCritEffect : CritEffect, ISharedPenaltyCrit
	{
		private static readonly List<Stats> AffectedStats = new()
		{
			Stats.Ref, Stats.Dex, Stats.Body, Stats.Cra
		};

		private DeadlyArmCritEffect() { }

		/// <summary>
		/// Конструктор эффекта потери руки
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DeadlyArmCritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			Severity = Severity.Deadly | Severity.Unstabilizied;
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
		public static DeadlyArmCritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			if (!creature.Effects.Any(x => x is BleedEffect))
				creature.Effects.Add(BleedEffect.Create(null, null, creature, "Secondary Bleed"));

			CheckExistingEffectAndRemoveStabilizedEffect<DeadlyArmCritEffect>(creature, aimedPart);
			var effect = new DeadlyArmCritEffect(creature, aimedPart, name);

			ApplySharedPenalty(creature, effect);

			return effect;
		}

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
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
