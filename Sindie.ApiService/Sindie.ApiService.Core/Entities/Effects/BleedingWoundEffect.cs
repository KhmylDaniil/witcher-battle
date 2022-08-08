using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System;
using System.Linq;
using System.Text;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Эффект кровавой раны
	/// </summary>
	public sealed class BleedingWoundEffect : Effect
	{
		/// <summary>
		/// Результат броска кровавой раны
		/// </summary>
		public int Severity { get; private set; }

		/// <summary>
		/// Урон кровавой раны
		/// </summary>
		public int Damage { get; private set; }

		private BleedingWoundEffect() { }

		/// <summary>
		/// Конструктор эффекта кровавой раны
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="severity">Результат броска</param>
		/// <param name="name">Название</param>
		private BleedingWoundEffect(Creature creature, string name, int severity) : base(creature, name)
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
		/// <param name="name">Название</param>
		/// <returns>Эффект</returns>
		public static BleedingWoundEffect Create(IRollService rollService, Creature attacker, Creature target, string name)
		{
			if (!rollService.BeatDifficulty(attacker.SkillBase(Skills.BleedingWoundId), 15, out int severity))
				return null;

			var existingEffect = target.Effects.FirstOrDefault(x => x is BleedingWoundEffect) as BleedingWoundEffect;

			if (existingEffect is null)
				return new BleedingWoundEffect(target, name, severity);

			if (existingEffect.Severity < severity)
				existingEffect.Severity = severity;

			return null;
		}

		public override string ToString()
			=> $"effect {Conditions.BleedingWoundName} with severity {Severity} and {Damage} damage";

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(ref Creature creature, ref StringBuilder message)
		{
			creature.HP -= Damage;
			message.AppendLine($"Существо {creature.Name} получило {Damage} урона из-за кровавой раны. Осталось {creature.HP} хитов.");
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
			Heal heal = new(rollService);

			heal.TryStabilize(creature, ref creature, ref message, this);
		}

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns>Навык шаблона существа</returns>
		[Obsolete("Только для тестов")]
		public static BleedingWoundEffect CreateForTest(
			Guid? id = default,
			Creature creature = default,
			string name = default,
			int severity = default,
			int damage = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new()
		{
			Id = id ?? Guid.NewGuid(),
			Creature = creature,
			Name = name ?? Conditions.BleedingWoundName,
			Severity = severity,
			Damage = damage,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId
		};
	}
}
