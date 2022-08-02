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
		/// Конструктор эффекта заморозки
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>
		public FreezeEffect(Creature creature, Condition condition) : base(creature, condition)
			=> ApplyStatChanges(creature);

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

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(ref Creature creature, ref StringBuilder message)
		{
		}

		/// <summary>
		/// Автоматически прекратить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void AutoEnd(ref Creature creature, ref StringBuilder message)
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
			var skill = creature.CreatureSkills.FirstOrDefault(x => x.SkillId == Skills.PhysiqueId);

			int skillBase = skill == null
				? creature.Body
				: creature.SkillBase(Skills.PhysiqueId);

			if (rollService.BeatDifficulty(skillBase, 16))
			{
				RevertStatChanges(creature);
				message.AppendFormat($"Эффект {Name} снят. Скорость равна {creature.Speed}, рефлексы равны {creature.Ref}.");
				creature.Effects.Remove(this);
			}
			else
				message.AppendLine($"Не удалось снять эффект {Name}.");
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		private void ApplyStatChanges(Creature creature)
		{
			creature.Speed += SpeedModifier;
			creature.Ref += RefModifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		private void RevertStatChanges(Creature creature)
		{
			creature.Speed = creature.GetSpeed() - SpeedModifier;
			creature.Ref = creature.GetRef() - RefModifier;
		}
	}
}
