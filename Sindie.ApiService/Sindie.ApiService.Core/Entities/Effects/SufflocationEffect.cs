using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using System.Linq;
using System.Text;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Эффект удушья
	/// </summary>
	public sealed class SufflocationEffect : Effect
	{
		private SufflocationEffect() { }

		/// <summary>
		/// Конструктор эффекта удушья
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">СНазвание</param>

		private SufflocationEffect(Creature creature, string name) : base(creature, name) { }

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="attacker">Атакующий</param>
		/// <param name="target">Цель</param>
		/// <param name="name">Название</param>
		/// <returns>Эффект</returns>
		public static SufflocationEffect Create(IRollService rollService, Creature attacker, Creature target, string name)
			=> target.Effects.Any(x => x is SufflocationEffect)
				? null
				: new SufflocationEffect(target, name);

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(ref Creature creature, ref StringBuilder message)
		{
			creature.HP -= 3;
			message.AppendLine($"Существо {creature.Name} потеряло 3 хита из-за удушья. Осталось {creature.HP} хитов.");
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
			message.AppendFormat($"Эффект {Conditions.SufflocationName} снят.");
				creature.Effects.Remove(this);
		}
	}
}
