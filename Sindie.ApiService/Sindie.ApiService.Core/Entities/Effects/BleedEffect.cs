using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using System.Linq;
using System.Text;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Эффект кровотечения
	/// </summary>
	public class BleedEffect : Effect
	{
		protected BleedEffect()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>

		public BleedEffect(Creature creature, Condition condition) : base(creature, condition)
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
		public static BleedEffect Create(IRollService rollService, Creature attacker, Creature target, Condition condition)
		{
			BleedEffect effect = target.Effects.FirstOrDefault(x => x.EffectId == Conditions.BleedId) as BleedEffect;

			return effect == null ? new BleedEffect(target, condition) : effect;
		}

		public override void Run(ref Creature creature, ref StringBuilder message)
		{
			creature.HP -=2;
			message.AppendLine($"Существо {creature.Name} потеряло 2 хита из-за кровотечения. Осталось {creature.HP} хитов.");
		}

		public override void AutoEnd(ref Creature creature, ref StringBuilder message)
		{
		}

		public override void Treat(IRollService rollService, ref Creature creature, ref StringBuilder message)
		{
			throw new System.NotImplementedException();
		}
	}
}
