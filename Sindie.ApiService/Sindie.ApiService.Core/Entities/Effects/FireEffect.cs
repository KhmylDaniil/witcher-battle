using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using System.Linq;
using System.Text;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Эффект горения
	/// </summary>
	public class FireEffect : Effect
	{
		protected FireEffect()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>
		public FireEffect(Creature creature, Condition condition) : base(creature, condition)
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
		public static FireEffect Create(IRollService rollService, Creature attacker, Creature target, Condition condition)
		{
			FireEffect effect = target.Effects.FirstOrDefault(x => x.EffectId == Conditions.FireId) as FireEffect;

			return effect == null ? new FireEffect(target, condition) : effect;
		}

		public override void Run(ref Creature creature, ref StringBuilder message)
		{
			int totalDamage = 0;
			
			foreach (var creaturePart in creature.CreatureParts)
				totalDamage += ApplyDamage(creaturePart);

			creature.HP -= totalDamage;
			message.AppendLine($"Существо {creature.Name} получает {totalDamage} от горения. Осталось {creature.HP} хитов.");
		}

		public override void AutoEnd(ref Creature creature, ref StringBuilder message)
		{
		}

		public override void Treat(IRollService rollService, ref Creature creature, ref StringBuilder message)
		{
			throw new System.NotImplementedException();
		}

		private int ApplyDamage(CreaturePart creaturePart)
		{
			if (creaturePart.CurrentArmor > 0)
				creaturePart.CurrentArmor--;

			return creaturePart.CurrentArmor >= 5
				? 0
				: 5 - creaturePart.CurrentArmor;
		}
	}
}
