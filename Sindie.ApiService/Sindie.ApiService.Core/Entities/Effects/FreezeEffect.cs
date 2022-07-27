using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using System.Linq;
using System.Text;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Эффект заморозки
	/// </summary>
	public class FreezeEffect : Effect, IStatChangingEffect
	{
		public int Int { get; private set; }

		public int Ref { get; private set; }

		public int Dex { get; private set; }

		public int Body { get; private set; }

		public int Emp { get; private set; }

		public int Cra { get; private set; }

		public int Will { get; private set; }

		public int Speed { get; private set; }

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
			Speed = -3;
			Ref = -1;

			creature.Ref += Ref;
			creature.Speed += Speed;
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
			throw new System.NotImplementedException();
		}
	}
}
