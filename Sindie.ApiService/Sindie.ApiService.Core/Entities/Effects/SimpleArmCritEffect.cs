using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Sindie.ApiService.Core.BaseData.Crit;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - Вывих руки
	/// </summary>
	public class SimpleArmCritEffect : Effect, ICrit, IArmCrit
	{
		private const int SkillModifier = -2;
		private const int AfterTreatSkillModifier = -1;
		private readonly List<string> AffectedStats = new()
		{
			"Ref", "Dex", "Body", "Cra"
		};

		private SimpleArmCritEffect() { }

		/// <summary>
		/// Конструктор эффекта вывиха руки
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>
		private SimpleArmCritEffect(Creature creature, CreaturePart aimedPart, Condition condition) : base(creature, condition)
		{
			if (creature.LeadingArmId == aimedPart.Id)
				ApplyStatChanges(creature);
		}

		/// <summary>
		/// Тяжесть критического эффекта
		/// </summary>
		public Severity Severity { get; private set; } = Severity.Simple | Severity.Unstabilizied;

		public BodyPartTypes.BodyPartType BodyPartLocation { get; } = BodyPartTypes.BodyPartType.Arm;

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="target">Цель</param>
		/// <param name="condition">Состояние</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static SimpleArmCritEffect Create(Creature target, CreaturePart aimedPart, Condition condition)
			=> target.Effects.Any(x => x is IArmCrit && x.CreaturePartId == aimedPart.Id)
				? null
				: new SimpleArmCritEffect(target, aimedPart, condition);

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

		public void Stabilize(Creature creature)
		{
			if (Severity == Severity.Simple)
				return;

			Severity = Severity.Simple;

			if (CreaturePartId != creature.LeadingArmId)
				return;

			var creatureSkills = creature.CreatureSkills.Where(x => AffectedStats.Contains(x.StatName));

			foreach (var skill in creatureSkills)
			{
				skill.SkillValue -= SkillModifier;
				skill.SkillValue += AfterTreatSkillModifier;
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
			var creatureSkills = creature.CreatureSkills.Where(x => AffectedStats.Contains(x.StatName));

			foreach (var skill in creatureSkills)
				skill.SkillValue += SkillModifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			if (CreaturePartId != creature.LeadingArmId)
				return;

			var creatureSkills = creature.CreatureSkills.Where(x => AffectedStats.Contains(x.StatName));

			foreach (var skill in creatureSkills)
				if (Severity == Severity.Simple)
					skill.SkillValue -= AfterTreatSkillModifier;
				else
					skill.SkillValue -= SkillModifier;
		}
	}
}
