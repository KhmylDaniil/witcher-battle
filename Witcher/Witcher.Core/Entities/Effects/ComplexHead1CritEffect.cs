using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - Выбитые зубы
	/// </summary>
	public sealed class ComplexHead1CritEffect : CritEffect, ICrit
	{
		private const int SkillModifier = -3;
		private const int AfterTreatSkillModifier = -2;
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

		private ComplexHead1CritEffect() { }

		/// <summary>
		/// Конструктор эффекта выбитых зубов
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private ComplexHead1CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			ApplyStatChanges(creature);
			Severity = Severity.Complex | Severity.Unstabilizied;
			BodyPartLocation = Enums.BodyPartType.Head;
		}

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static ComplexHead1CritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			int teeth = new Random().Next(1, 10);

			return CheckExistingEffectAndRemoveStabilizedEffect<ComplexHead1CritEffect>(creature, aimedPart)
				? new ComplexHead1CritEffect(creature, aimedPart, name + $" {teeth} штук")
				: null;
		}

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			if (IsStabile(Severity))
				return;

			Severity = Severity.Complex;

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
				if (Severity == Severity.Complex)
					skill.SkillValue = skill.GetValue() - AfterTreatSkillModifier;
				else
					skill.SkillValue = skill.GetValue() - SkillModifier;
		}
	}
}
