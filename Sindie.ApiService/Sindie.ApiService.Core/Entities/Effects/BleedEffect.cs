using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System.Linq;
using System.Text;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Эффект кровотечения
	/// </summary>
	public sealed class BleedEffect : Effect
	{
		private BleedEffect() { }

		/// <summary>
		/// Конструктор эффекта кровотечения
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>

		private BleedEffect(Creature creature, Condition condition) : base(creature, condition) { }

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="attacker">Атакующий</param>
		/// <param name="target">Цель</param>
		/// <param name="condition">Состояние</param>
		/// <returns>Эффект</returns>
		public static BleedEffect Create(IRollService rollService, Creature attacker, Creature target, Condition condition)
			=> target.Effects.Any(x => x.EffectId == Conditions.BleedId)
				? null
				: new BleedEffect(target, condition);

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(ref Creature creature, ref StringBuilder message)
		{
			creature.HP -=2;
			message.AppendLine($"Существо {creature.Name} потеряло 2 хита из-за кровотечения. Осталось {creature.HP} хитов.");
		}

		/// <summary>
		/// Автоматически прекратить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void AutoEnd(ref Creature creature, ref StringBuilder message) { }

		/// <summary>
		/// Попробовать снять эффект
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Treat(IRollService rollService, ref Creature creature, ref StringBuilder message)
		{
			Heal heal = new(rollService);

			heal.Stabilize(creature, ref creature, ref message, this);
		}
	}
}
