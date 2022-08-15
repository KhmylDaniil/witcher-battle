using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Entities.Effects
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

			var effect = CheckExistingEffectAndRemoveStabilizedEffect<DeadlyArmCritEffect>(creature, aimedPart)
				? new DeadlyArmCritEffect(creature, aimedPart, name)
				: null;

			ApplySharedPenalty(creature, effect);

			return effect;
		}

		/// <summary>
		/// Автоматически прекратить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void AutoEnd(Creature creature, ref StringBuilder message) { }

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(Creature creature, ref StringBuilder message) { }

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
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Treat(IRollService rollService, Creature creature, ref StringBuilder message)
		{
			Heal heal = new(rollService);

			heal.TryStabilize(creature, creature, ref message, this);

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

			var creatureSkills = creature.CreatureSkills.Where(x => AffectedStats.Contains(x.StatName));

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

			var creatureSkills = creature.CreatureSkills.Where(x => AffectedStats.Contains(x.StatName));

			foreach (var skill in creatureSkills)
				skill.SkillValue = skill.GetValue() + skill.MaxValue / 2;
		}
	}
}
