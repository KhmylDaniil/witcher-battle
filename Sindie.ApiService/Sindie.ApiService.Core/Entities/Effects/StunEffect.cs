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
	public sealed class StunEffect : Effect
	{
		private StunEffect() { }

		/// <summary>
		/// Конструктор эффекта дизориентации
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		private StunEffect(Creature creature, string name) : base(creature, name) { }

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="attacker">Атакующий</param>
		/// <param name="target">Существо</param>
		/// <param name="name">Название</param>
		/// <returns>Эффект</returns>
		public static StunEffect Create(IRollService rollService, Creature attacker, Creature target, string name)
			=> target.Effects.Any(x => x is StunEffect)
				? null
				: new StunEffect(target, name);

		/// <summary>
		/// Автоматически прекратить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void AutoEnd(Creature creature, ref StringBuilder message)
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
		/// Попробовать снять эффект
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="healer">Лекарь</param>
		/// <param name="patient">Цель</param>
		/// <param name="message">Сообщение</param>
		public override void Treat(IRollService rollService, Creature healer, Creature patient, ref StringBuilder message) { }
	}
}
