using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System;
using System.Linq;
using System.Text;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Эффект смерти
	/// </summary>
	public sealed class DeadEffect : Effect
	{
		private DeadEffect() { }

		/// <summary>
		/// Конструктор эффекта смерти
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">СНазвание</param>

		private DeadEffect(Creature creature, string name) : base(creature, name) { }

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="attacker">Атакующий</param>
		/// <param name="target">Цель</param>
		/// <param name="name">Название</param>
		/// <returns>Эффект</returns>
		public static DeadEffect Create(IRollService rollService, Creature attacker, Creature target, string name)
			=> target.Effects.Any(x => x is DeadEffect)
				? null
				: new DeadEffect(target, name);

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
