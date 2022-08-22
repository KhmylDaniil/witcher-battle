﻿using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System;
using System.Linq;
using System.Text;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - Потеря хвоста
	/// </summary>
	public class DeadlyTailCritEffect : CritEffect, ISharedPenaltyCrit
	{
		/// <summary>
		/// Модификатор уклонения
		/// </summary>
		public int DodgeModifier { get; private set; }

		/// <summary>
		/// Модификатор атлетики
		/// </summary>
		public int AthleticsModifier { get; private set; }

		private DeadlyTailCritEffect() { }

		/// <summary>
		/// Конструктор эффекта потери хвоста
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DeadlyTailCritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			DodgeModifier = (int)Math.Floor(creature.GetSkillMax(Skills.DodgeId) * -0.75);

			AthleticsModifier = (int)Math.Floor(creature.GetSkillMax(Skills.AthleticsId) * -0.75);

			Severity = Severity.Deadly | Severity.Unstabilizied;
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
		public static DeadlyTailCritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			if (!creature.Effects.Any(x => x is BleedEffect))
				creature.Effects.Add(BleedEffect.Create(null, null, creature, "Secondary Bleed"));

			var effect = CheckExistingEffectAndRemoveStabilizedEffect<DeadlyTailCritEffect>(creature, aimedPart)
				? new DeadlyTailCritEffect(creature, aimedPart, name)
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
			PenaltyApplied = true;

			var dodge = creature.CreatureSkills.FirstOrDefault(x => x.SkillId == Skills.DodgeId);
			if (dodge is not null)
				dodge.SkillValue = dodge.GetValue() + DodgeModifier;

			var athletics = creature.CreatureSkills.FirstOrDefault(x => x.SkillId == Skills.AthleticsId);
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

			var dodge = creature.CreatureSkills.FirstOrDefault(x => x.SkillId == Skills.DodgeId);
			if (dodge is not null)
				dodge.SkillValue = dodge.GetValue() - DodgeModifier;

			var athletics = creature.CreatureSkills.FirstOrDefault(x => x.SkillId == Skills.AthleticsId);
			if (athletics is not null)
				athletics.SkillValue = athletics.GetValue() - AthleticsModifier;
		}
	}
}