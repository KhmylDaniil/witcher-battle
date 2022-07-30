using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using System.Linq;
using System.Text;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Эффект кровавой раны
	/// </summary>
	public class BleedingWoundEffect : Effect
	{
		/// <summary>
		/// Результат броска кровавой раны
		/// </summary>
		public int Severity { get; }

		/// <summary>
		/// Урон кровавой раны
		/// </summary>
		public int Damage { get; }


		protected BleedingWoundEffect()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>
		/// <param name="severity">Результат броска</param>
		public BleedingWoundEffect(Creature creature, Condition condition, int severity) : base(creature, condition)
		{
			Severity = severity;
			Damage = (severity - 15) / 2;
		}

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="attacker">Атакующий</param>
		/// <param name="target">Цель</param>
		/// <param name="condition">Состояние</param>
		/// <returns>Эффект</returns>
		public static BleedingWoundEffect Create(IRollService rollService, Creature attacker, Creature target, Condition condition)
		{
			BleedingWoundEffect effect = target.Effects.FirstOrDefault(x => x.EffectId == Conditions.BleedingWoundId) as BleedingWoundEffect;
			
			int effectSeverity = effect == null ? 0 : effect.Severity;

			if (rollService.BeatDifficulty(attacker.SkillBase(Skills.BleedingWoundId), 15, out int severity)
				&& effectSeverity < severity)
				effect = new BleedingWoundEffect(target, condition, severity);

			return effect;
		}

		public override string ToString()
		{
			return $"effect {Name} with severity {Severity} and {Damage} damage";
		}

		public override void Run(ref Creature creature, ref StringBuilder message)
		{
			creature.HP -= Damage;
			message.AppendLine($"Существо {creature.Name} получило {Damage} урона из-за кровавой раны. Осталось {creature.HP} хитов.");
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
