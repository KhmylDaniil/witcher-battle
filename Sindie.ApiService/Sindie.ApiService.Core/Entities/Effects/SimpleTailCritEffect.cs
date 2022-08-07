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
	/// Критический эффект - Вывих хвоста
	/// </summary>
	public class SimpleTailCritEffect : Effect, ICrit, ITailCrit
	{
		private const int Modifier = -2;
		private const int AfterTreatModifier = -1;
		private readonly List<Guid> _affectedSkills = new() {	Skills.DodgeId, Skills.AthleticsId	};

		private SimpleTailCritEffect() { }

		/// <summary>
		/// Конструктор эффекта вывиха хвоста
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>
		private SimpleTailCritEffect(Creature creature, CreaturePart aimedPart, Condition condition) : base(creature, condition)
		{
			if (!creature.Effects.Any(x => x is ITailCrit))
				ApplyStatChanges(creature);
		}

		/// <summary>
		/// Тяжесть критического эффекта
		/// </summary>
		public Severity Severity { get; private set; } = Severity.Simple | Severity.Unstabilizied;

		/// <summary>
		/// Тип части тела
		/// </summary
		public BodyPartTypes.BodyPartType BodyPartLocation { get; } = BodyPartTypes.BodyPartType.Tail;

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="target">Цель</param>
		/// <param name="condition">Состояние</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static SimpleTailCritEffect Create(Creature target, CreaturePart aimedPart, Condition condition)
			=> target.Effects.Any(x => x is ITailCrit)
				? null
				: new SimpleTailCritEffect(target, aimedPart, condition);

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
			if (Severity == Severity.Simple)
				return;

			Severity = Severity.Simple;

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
			var creatureSkills = creature.CreatureSkills.Where(x => _affectedSkills.Contains(x.SkillId));

			foreach (var skill in creatureSkills)
				if (Severity == Severity.Simple)
					skill.SkillValue -= AfterTreatModifier;
				else
					skill.SkillValue -= Modifier;
		}
	}
}
