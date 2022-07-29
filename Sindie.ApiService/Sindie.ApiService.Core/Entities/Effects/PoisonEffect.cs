using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Эффект отравления
	/// </summary>
	public class PoisonEffect : Effect
	{
		protected PoisonEffect()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>

		public PoisonEffect(Creature creature, Condition condition) : base(creature, condition)
		{
		}

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="attacker">Атакующий</param>
		/// <param name="target">Цель</param>
		/// <param name="condition">Состояние</param>
		/// <returns>Эффект</returns>
		public static PoisonEffect Create(IRollService rollService, Creature attacker, Creature target, Condition condition)
		{
			PoisonEffect effect = target.Effects.FirstOrDefault(x => x.EffectId == Conditions.PoisonId) as PoisonEffect;

			return effect == null ? new PoisonEffect(target, condition) : effect;
		}

		public void Run(Creature creature)
		{
			creature.HP -= 3;
		}

		public override void Run(ref Creature creature, ref StringBuilder message)
		{
			creature.HP -= 3;
			message.AppendLine($"Существо {creature.Name} потеряло 3 хита из-за отравления. Осталось {creature.HP} хитов.");
		}

		public override void AutoEnd(ref Creature creature, ref StringBuilder message)
		{
		}

		public override void Treat(IRollService rollService, ref Creature creature, ref StringBuilder message)
		{
			var skill = creature.CreatureSkills.FirstOrDefault(x => x.Id == Skills.EnduranceId);

			int skillBase = skill == null
				? creature.Body
				: creature.SkillBase(Skills.PhysiqueId);

			if (rollService.BeatDifficulty(skillBase, 15))
			{
				message.AppendFormat($"Эффект {Name} снят.");
				creature.Effects.Remove(this);
			}
			else
				message.AppendLine($"Не удалось снять эффект {Name}.");
		}
	}
}
