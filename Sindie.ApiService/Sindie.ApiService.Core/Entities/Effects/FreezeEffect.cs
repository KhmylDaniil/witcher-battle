using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using System.Linq;
using System.Text;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Эффект заморозки
	/// </summary>
	public class FreezeEffect : Effect
	{
		private const int SpeedModifier = -3;
		private const int RefModifier = -1;

		protected FreezeEffect()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>
		public FreezeEffect(Creature creature, Condition condition) : base(creature, condition)
		{
			creature.Ref += RefModifier;
			creature.Speed += SpeedModifier;
		}

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="attacker">Атакующий</param>
		/// <param name="target">Цель</param>
		/// <param name="condition">Состояние</param>
		/// <returns>Эффект</returns>
		public static FreezeEffect Create(IRollService rollService, Creature attacker, Creature target, Condition condition)
		{
			FreezeEffect effect = target.Effects.FirstOrDefault(x => x.EffectId == Conditions.FreezeId) as FreezeEffect;

			return effect == null ? new FreezeEffect(target, condition) : effect;
		}

		public override void Run(ref Creature creature, ref StringBuilder message)
		{
		}

		public override void AutoEnd(ref Creature creature, ref StringBuilder message)
		{
		}

		public override void Treat(IRollService rollService, ref Creature creature, ref StringBuilder message)
		{
			var skill = creature.CreatureSkills.FirstOrDefault(x => x.Id == Skills.PhysiqueId);

			int skillBase = skill == null
				? creature.Body
				: creature.SkillBase(Skills.PhysiqueId);

			if (rollService.BeatDifficulty(skillBase, 16))
			{
				creature.Ref -= RefModifier;
				creature.Speed -= SpeedModifier;
				message.AppendFormat($"Эффект {Name} снят. Скорость равна {creature.Speed}, рефлексы равны {creature.Ref}.");
				creature.Effects.Remove(this);
			}
			else
				message.AppendLine($"Не удалось снять эффект {Name}.");
		}
	}
}
