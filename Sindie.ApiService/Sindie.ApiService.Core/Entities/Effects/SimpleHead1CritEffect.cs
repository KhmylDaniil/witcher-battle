﻿using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Sindie.ApiService.Core.BaseData.Crit;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - Уродующий шрам
	/// </summary>
	public sealed class SimpleHead1CritEffect : Effect, ICrit
	{
		private const int SkillModifier = -3;
		private const int AfterTreatSkillModifier = -1;
		private readonly List<Guid> affectedSkills = new()
		{
			Skills.CharismaId,
			Skills.PersuasionId,
			Skills.SeductionId,
			Skills.LeadershipId,
			Skills.DeceitId,
			Skills.SocialEtiquetteId,
			Skills.IntimidationId
		};

		private SimpleHead1CritEffect() { }

		/// <summary>
		/// Конструктор эффекта уродующего шрама
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>
		private SimpleHead1CritEffect(Creature creature, Condition condition) : base(creature, condition)
			=> ApplyStatChanges(creature);

		/// <summary>
		/// Тяжесть критического эффекта
		/// </summary>
		public Severity Severity { get; private set; } = Severity.Simple | Severity.Unstabilizied;

		/// <summary>
		/// Тип части тела
		/// </summary
		public BodyPartTypes.BodyPartType BodyPartLocation { get; } = BodyPartTypes.BodyPartType.Head;

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="target">Цель</param>
		/// <param name="condition">Состояние</param>
		/// <returns>Эффект</returns>
		public static SimpleHead1CritEffect Create(Creature target, CreaturePart aimedPart, Condition condition)
			=> target.Effects.Any(x => x.EffectId == Crit.SimpleHead1Id)
				? null
				: new SimpleHead1CritEffect(target, condition);

		/// <summary>
		/// Автоматически прекратить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void AutoEnd(ref Creature creature, ref StringBuilder message) { }

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(ref Creature creature, ref StringBuilder message) { }

		/// <summary>
		/// Попробовать снять эффект
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Treat(IRollService rollService, ref Creature creature, ref StringBuilder message)
		{
			Heal heal = new(rollService);

			heal.TryStabilize(creature, ref creature, ref message, this);
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature)
		{
			var creatureSkills = creature.CreatureSkills.Where(x => affectedSkills.Contains(x.SkillId));

			foreach (var skill in creatureSkills)
				skill.SkillValue += SkillModifier;
		}

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			if (Severity == Severity.Simple)
				return;

			Severity = Severity.Simple;

			var creatureSkills = creature.CreatureSkills.Where(x => affectedSkills.Contains(x.SkillId));

			foreach (var skill in creatureSkills)
			{
				skill.SkillValue -= SkillModifier;
				skill.SkillValue += AfterTreatSkillModifier;
			}

		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			var creatureSkills = creature.CreatureSkills.Where(x => affectedSkills.Contains(x.SkillId));

			foreach (var skill in creatureSkills)
				if (Severity == Severity.Simple)
					skill.SkillValue -= AfterTreatSkillModifier;
				else
					skill.SkillValue -= SkillModifier;
		}
	}
}
