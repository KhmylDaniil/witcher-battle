using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Logic;
using System.Linq;
using System.Text;

namespace Witcher.Core.Entities.Effects
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
		/// <param name="name">СНазвание</param>

		private BleedEffect(Creature creature, string name) : base(creature, name) { }

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="attacker">Атакующий</param>
		/// <param name="target">Цель</param>
		/// <param name="name">Название</param>
		/// <returns>Эффект</returns>
		public static BleedEffect Create(IRollService rollService, Creature attacker, Creature target, string name)
			=> target.Effects.Any(x => x is BleedEffect)
				? null
				: new BleedEffect(target, name);

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(Creature creature, ref StringBuilder message)
		{
			creature.HP -=2;
			message.AppendLine($"Существо {creature.Name} потеряло 2 хита из-за кровотечения. Осталось {creature.HP} хитов.");
		}
	}
}
