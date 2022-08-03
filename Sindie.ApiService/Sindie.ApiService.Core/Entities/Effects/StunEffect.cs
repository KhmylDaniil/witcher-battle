using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using System;
using System.Linq;
using System.Text;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Эффект дизориентации
	/// </summary>
	public class StunEffect : Effect
	{
		protected StunEffect()
		{
		}

		/// <summary>
		/// Конструктор эффекта дизориентации
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>
		public StunEffect(Creature creature, Condition condition) : base(creature, condition)
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
		public static StunEffect Create(IRollService rollService, Creature attacker, Creature target, Condition condition)
		{
			StunEffect effect = target.Effects.FirstOrDefault(x => x.EffectId == Conditions.StunId) as StunEffect;

			return effect ?? new StunEffect(target, condition);
		}

		/// <summary>
		/// Автоматически прекратить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void AutoEnd(ref Creature creature, ref StringBuilder message)
		{
			Random random = new ();
			if (random.Next() < creature.Stun)
			{
				message.AppendFormat($"Эффект {Name} снят.");
				creature.Effects.Remove(this);
			}
			else
				message.AppendLine($"Не удалось снять эффект {Name}.");
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
		/// Попробовать снять эффект
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Treat(IRollService rollService, ref Creature creature, ref StringBuilder message)
		{
		}
	}
}
