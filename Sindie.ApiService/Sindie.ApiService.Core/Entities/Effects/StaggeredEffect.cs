using Witcher.Core.Abstractions;
using System.Linq;
using System.Text;

namespace Witcher.Core.Entities.Effects
{
	/// <summary>
	/// Эффект ошеломления
	/// </summary>
	public sealed class StaggeredEffect : Effect
	{
		public const int AttackAndDefenseModifier = -2;

		private StaggeredEffect() { }

		/// <summary>
		/// Конструктор эффекта ошеломления
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">СНазвание</param>

		private StaggeredEffect(Creature creature, string name) : base(creature, name) { }

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="attacker">Атакующий</param>
		/// <param name="target">Цель</param>
		/// <param name="name">Название</param>
		/// <returns>Эффект</returns>
		public static StaggeredEffect Create(IRollService rollService, Creature attacker, Creature target, string name)
			=> target.Effects.Any(x => x is StaggeredEffect)
				? null
				: new StaggeredEffect(target, name);

		/// <summary>
		/// Автоматически прекратить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void AutoEnd(Creature creature, ref StringBuilder message)
			=> creature.Effects.Remove(this);

		/// <summary>
		/// Попробовать снять эффект
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="healer">Лекарь</param>
		/// <param name="patient">Цель</param>
		/// <param name="message">Сообщение</param>
		public override void Treat(IRollService rollService, Creature healer, Creature patient, ref StringBuilder message)
		{
			// Method intentionally left empty.
		}
	}
}
