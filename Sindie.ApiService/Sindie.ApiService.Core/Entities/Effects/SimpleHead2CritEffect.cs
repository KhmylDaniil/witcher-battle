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
	/// Критический эффект - Треснувшая челюсть
	/// </summary>
	public sealed class SimpleHead2CritEffect : CritEffect, ICrit
	{
		private const int SkillModifier = -2;
		private const int AfterTreatSkillModifier = -1;
		private static readonly List<Skill> affectedSkills = new()
		{
			Skill.Spell, 
			Skill.RitualCrafting,
			Skill.HexWeaving,
			Skill.Charisma,
			Skill.Persuasion,
			Skill.Seduction,
			Skill.Leadership,
			Skill.Deceit,
			Skill.SocialEtiquette,
			Skill.Intimidation
		};

		private SimpleHead2CritEffect() { }

		/// <summary>
		/// Конструктор эффекта треснувшей челюсти
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private SimpleHead2CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			ApplyStatChanges(creature);
			Severity = Severity.Simple | Severity.Unstabilizied;
			BodyPartLocation = Enums.BodyPartType.Head;
		}

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static SimpleHead2CritEffect Create(Creature creature, CreaturePart aimedPart, string name)
			=> CheckExistingEffectAndRemoveStabilizedEffect<SimpleHead2CritEffect>(creature, aimedPart)
				? new SimpleHead2CritEffect(creature, aimedPart, name)
				: null;

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
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			if (Severity == Severity.Simple)
				return;

			Severity = Severity.Simple;

			var creatureSkills = creature.CreatureSkills.Where(x => affectedSkills.Contains(x.Skill));

			foreach (var skill in creatureSkills)
			{
				skill.SkillValue = skill.GetValue() - SkillModifier;
				skill.SkillValue = skill.GetValue() + AfterTreatSkillModifier;
			}
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature)
		{
			var creatureSkills = creature.CreatureSkills.Where(x => affectedSkills.Contains(x.Skill));

			foreach (var skill in creatureSkills)
				skill.SkillValue = skill.GetValue() + SkillModifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			var creatureSkills = creature.CreatureSkills.Where(x => affectedSkills.Contains(x.Skill));

			foreach (var skill in creatureSkills)
				if (Severity == Severity.Simple)
					skill.SkillValue = skill.GetValue() - AfterTreatSkillModifier;
				else
					skill.SkillValue = skill.GetValue() - SkillModifier;
		}
	}
}
