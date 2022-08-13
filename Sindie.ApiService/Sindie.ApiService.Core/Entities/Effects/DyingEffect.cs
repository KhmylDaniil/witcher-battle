using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System;
using System.Linq;
using System.Text;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Эффект при смерти
	/// </summary>
	public sealed class DyingEffect : Effect
	{
		private DyingEffect() { }

		/// <summary>
		/// Конструктор эффекта при смрти
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">СНазвание</param>

		private DyingEffect(Creature creature, string name) : base(creature, name) { }

		/// <summary>
		/// Модификатор испытания против смерти
		/// </summary>
		public int Counter { get; private set; }

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="attacker">Атакующий</param>
		/// <param name="target">Цель</param>
		/// <param name="name">Название</param>
		/// <returns>Эффект</returns>
		public static DyingEffect Create(IRollService rollService, Creature attacker, Creature target, string name)
			=> target.Effects.Any(x => x is DyingEffect)
				? null
				: new DyingEffect(target, name);

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(ref Creature creature, ref StringBuilder message)
		{
			if (new Random().Next(1, 10) >= creature.Stun + Counter)
			{
				creature.HP = -100;
				return;
			}
				Counter--;
				message.AppendLine($"Персонаж {creature.Name} цепляется за жизнь.");
		}

		/// <summary>
		/// Автоматически прекратить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void AutoEnd(ref Creature creature, ref StringBuilder message)
		{
			if (creature.HP >= 0)
				creature.Effects.Remove(this);
		}

		/// <summary>
		/// Попробовать снять эффект
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Treat(IRollService rollService, ref Creature creature, ref StringBuilder message)
		{
			//Heal heal = new(rollService);

			//heal.TryStabilize(creature, ref creature, ref message, this);
		}

		///// <summary>
		///// Испытание против внезапной смерти
		///// </summary>
		///// <param name="creature">Существо</param>
		//public static bool DeathSave(Creature creature)
		//{
		//	var modifier = (creature.Effects.FirstOrDefault(x => x is DyingEffect) as DyingEffect)?.Counter;
			
		//	if (new Random().Next(1, 10) >= creature.Stun + modifier)
		//		return false;
		//	}
		//	Counter--;
		//	message.AppendLine($"Персонаж {creature.Name} цепляется за жизнь.");
		//}


		//=> new Random().Next(1, 10) >= creature.Stun;
	}
}
