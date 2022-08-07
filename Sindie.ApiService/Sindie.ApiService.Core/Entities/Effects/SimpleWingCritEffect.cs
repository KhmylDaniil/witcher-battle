using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - Вывих крыла
	/// </summary>
	public class SimpleWingCritEffect : Effect, IWingCrit
	{
		private const int Modifier = -2;
		private const int AfterTreatModifier = -1;
		private readonly List<Guid> _affectedSkills = new () {	Skills.DodgeId, Skills.AthleticsId	};

		private SimpleWingCritEffect() { }

		/// <summary>
		/// Конструктор эффекта вывиха крыла
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>
		private SimpleWingCritEffect(Creature creature, CreaturePart aimedPart, Condition condition) : base(creature, condition) { }

		/// <summary>
		/// Тяжесть критического эффекта
		/// </summary>
		public Crit.Severity Severity { get; private set;} = Crit.Severity.Simple | Crit.Severity.Unstabilizied;

		/// <summary>
		/// Тип части тела
		/// </summary
		public BodyPartTypes.BodyPartType BodyPartLocation { get; } = BodyPartTypes.BodyPartType.Wing;

		/// <summary>
		/// Пенальти применено
		/// </summary>
		public bool PenaltyApplied { get; private set; }

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="target">Цель</param>
		/// <param name="condition">Состояние</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static SimpleWingCritEffect Create(Creature target, CreaturePart aimedPart, Condition condition)
		{
			if (!target.Effects.Any(x => x is IWingCrit && x.CreaturePartId == aimedPart.Id))
				return null;

			var effect = new SimpleWingCritEffect(target, aimedPart, condition);

			Heal.ApplyPenalty(target, effect);

			return effect;
		}

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
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			if (Severity == Crit.Severity.Simple)
				return;

			Severity = Crit.Severity.Simple;

			Heal.UpdatePenalty(creature, this);

			creature.Speed -= Modifier;
			creature.Speed += AfterTreatModifier;

			var creatureSkills = creature.CreatureSkills.Where(x => _affectedSkills.Contains(x.SkillId));

			foreach (var skill in creatureSkills)
			{
				skill.SkillValue -= Modifier;
				skill.SkillValue += AfterTreatModifier;
			}
		}

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
			PenaltyApplied = true;
			
			creature.Speed += Modifier;

			var creatureSkills = creature.CreatureSkills.Where(x => _affectedSkills.Contains(x.SkillId));

			foreach (var skill in creatureSkills)
				skill.SkillValue += Modifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			PenaltyApplied = false;

			var creatureSkills = creature.CreatureSkills.Where(x => _affectedSkills.Contains(x.SkillId));

			foreach (var skill in creatureSkills)
				if (Severity == Crit.Severity.Simple)
					skill.SkillValue -= AfterTreatModifier;
				else
					skill.SkillValue -= Modifier;

			creature.Speed = Severity == Crit.Severity.Simple
				? -AfterTreatModifier
				: -Modifier;
		}
	}
}
