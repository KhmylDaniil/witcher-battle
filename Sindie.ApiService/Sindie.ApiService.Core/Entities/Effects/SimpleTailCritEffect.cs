using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - Вывих хвоста
	/// </summary>
	public class SimpleTailCritEffect : CritEffect, ISharedPenaltyCrit
	{
		private const int Modifier = -2;
		private const int AfterTreatModifier = -1;
		private readonly List<Guid> _affectedSkills = new() {	Skills.DodgeId, Skills.AthleticsId	};

		private SimpleTailCritEffect() { }

		/// <summary>
		/// Конструктор эффекта вывиха хвоста
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private SimpleTailCritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name) { }

		/// <summary>
		/// Тяжесть критического эффекта
		/// </summary>
		public Severity Severity { get; private set; } = Severity.Simple | Severity.Unstabilizied;

		/// <summary>
		/// Тип части тела
		/// </summary
		public Enums.BodyPartType BodyPartLocation { get; } = Enums.BodyPartType.Tail;

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
		public static SimpleTailCritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			var effect = CheckExistingEffectAndRemoveStabilizedEffect<SimpleTailCritEffect>(creature, aimedPart)
				? new SimpleTailCritEffect(creature, aimedPart, name)
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
			if (IsStabile(Severity) || !PenaltyApplied)
				return;

			Severity = Severity.Simple;

			var creatureSkills = creature.CreatureSkills.Where(x => _affectedSkills.Contains(x.SkillId));

			foreach (var skill in creatureSkills)
			{
				skill.SkillValue = skill.GetValue() - Modifier;
				skill.SkillValue = skill.GetValue() + AfterTreatModifier;
			}

			SharedPenaltyMovedToAnotherCrit(creature, this);
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
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature)
		{
			PenaltyApplied = true;

			var creatureSkills = creature.CreatureSkills.Where(x => _affectedSkills.Contains(x.SkillId));

			foreach (var skill in creatureSkills)
				skill.SkillValue = skill.GetValue() + Modifier;
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
				if (Severity == Severity.Simple)
					skill.SkillValue = skill.GetValue() - AfterTreatModifier;
				else
					skill.SkillValue = skill.GetValue() - Modifier;
		}
	}
}
