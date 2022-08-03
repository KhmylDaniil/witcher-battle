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
	/// Критический эффект - Уродующий шрам
	/// </summary>
	public class SimpleHead1CritEffect : Effect
	{
		private const int SkillModifier = -2;
		private const int AfterTreatSkillModifier = -1;
		private readonly List<Guid> affectedSkills = new()
		{
			Skills.SpellId, 
			Skills.RitualCraftingId,
			Skills.HexWeavingId,
			Skills.CharismaId,
			Skills.PersuasionId,
			Skills.SeductionId,
			Skills.LeadershipId,
			Skills.DeceitId,
			Skills.SocialEtiquetteId,
			Skills.IntimidationId
		};

		/// <summary>
		/// Конструктор эффекта заморозки
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>
		public SimpleHead1CritEffect(Creature creature, Condition condition) : base(creature, condition)
			=> ApplyStatChanges(creature);

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="target">Цель</param>
		/// <param name="condition">Состояние</param>
		/// <returns>Эффект</returns>
		public static SimpleHead1CritEffect Create(Creature target, Condition condition)
		{
			SimpleHead1CritEffect effect = target.Effects.FirstOrDefault(x => x.EffectId == Crit.SimpleHead1Id) as SimpleHead1CritEffect;

			return effect ?? new SimpleHead1CritEffect(target, condition);
		}

		public override void AutoEnd(ref Creature creature, ref StringBuilder message)
		{
		}

		public override void Run(ref Creature creature, ref StringBuilder message)
		{
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

			if (heal.Stabilize(creature, ref creature, ref message, this))
			{
				RevertStatChanges(creature);
				message.AppendLine($"Эффект {Name} стабилизирован.");
				creature.Effects.Remove(this);
			}
			else
				message.AppendLine($"Не удалось стабилизировать эффект {Name}.");
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		private void ApplyStatChanges(Creature creature)
		{
			var creatureSkills = creature.CreatureSkills.Where(x => affectedSkills.Contains(x.SkillId));

			foreach (var skill in creatureSkills)
				skill.SkillValue += SkillModifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		private void RevertStatChanges(Creature creature)
		{
			var creatureSkills = creature.CreatureSkills.Where(x => affectedSkills.Contains(x.SkillId));

			foreach (var skill in creatureSkills)
			{
				skill.SkillValue -= SkillModifier;
				skill.SkillValue += AfterTreatSkillModifier;
			}	
		}
	}
}
