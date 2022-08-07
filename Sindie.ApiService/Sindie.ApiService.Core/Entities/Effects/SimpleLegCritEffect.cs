using Sindie.ApiService.Core.Abstractions;
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
	/// Критический эффект - Вывих ноги
	/// </summary>
	public class SimpleLegCritEffect : Effect, ICrit, ILegCrit
	{
		private const int Modifier = -2;
		private const int AfterTreatModifier = -1;
		private readonly List<Guid> _affectedSkills = new() { Skills.DodgeId, Skills.AthleticsId };
			
		private SimpleLegCritEffect() { }

		/// <summary>
		/// Конструктор эффекта вывиха ноги
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>
		private SimpleLegCritEffect(Creature creature, CreaturePart aimedPart, Condition condition) : base(creature, condition) { }

		/// <summary>
		/// Тяжесть критического эффекта
		/// </summary>
		public Severity Severity { get; private set; } = Severity.Simple | Severity.Unstabilizied;

		/// <summary>
		/// Тип части тела
		/// </summary
		public BodyPartTypes.BodyPartType BodyPartLocation { get; } = BodyPartTypes.BodyPartType.Leg;

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
		public static SimpleLegCritEffect Create(Creature target, CreaturePart aimedPart, Condition condition)
		{
			if (!target.Effects.Any(x => x is ILegCrit && x.CreaturePartId == aimedPart.Id))
				return null;

			var effect = new SimpleLegCritEffect(target, aimedPart, condition);

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

			creature.Speed -= Modifier;
			creature.Speed += AfterTreatModifier;

			var creatureSkills = creature.CreatureSkills.Where(x => _affectedSkills.Contains(x.SkillId));

			foreach (var skill in creatureSkills)
				if (Severity == Severity.Simple)
					skill.SkillValue -= AfterTreatModifier;
				else
					skill.SkillValue -= Modifier;

			creature.Speed = Severity == Severity.Simple
				? - AfterTreatModifier
				: - Modifier;
		}
	}
}
