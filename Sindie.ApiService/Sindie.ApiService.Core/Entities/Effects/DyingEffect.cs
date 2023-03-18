using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Logic;
using System;
using System.Linq;
using System.Text;

namespace Witcher.Core.Entities.Effects
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
		public override void Run(Creature creature, ref StringBuilder message)
		{
			if (new Random().Next(1, 10) >= creature.Stun + Counter)
			{
				creature.Effects.Add(DeadEffect.Create(null, null, creature, "Dead due failed death save"));
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
		public override void AutoEnd(Creature creature, ref StringBuilder message)
		{
			if (creature.HP >= 0)
				creature.Effects.Remove(this);
		}

		/// <summary>
		/// Испытание против внезапной смерти
		/// </summary>
		/// <param name="creature">Существо</param>
		public static bool DeathSave(Creature creature)
			=> new Random().Next(1, 10) >= creature.Stun;
	}
}
