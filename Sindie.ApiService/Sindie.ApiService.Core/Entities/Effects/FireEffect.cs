using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using System;
using System.Linq;
using System.Text;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Эффект горения
	/// </summary>
	public sealed class FireEffect : Effect
	{
		private FireEffect() { }

		/// <summary>
		/// Конструктор эффекта горения
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="condition">Состояние</param>
		private FireEffect(Creature creature, Condition condition) : base(creature, condition) { }

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="attacker">Атакующий</param>
		/// <param name="target">Цель</param>
		/// <param name="condition">Состояние</param>
		/// <returns>Эффект</returns>
		public static FireEffect Create(IRollService rollService, Creature attacker, Creature target, Condition condition)
			=> target.Effects.Any(x => x.EffectId == Conditions.FireId)
				? null
				: new FireEffect(target, condition);

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(ref Creature creature, ref StringBuilder message)
		{
			int totalDamage = 0;
			
			foreach (var creaturePart in creature.CreatureParts)
				totalDamage += ApplyDamage(creaturePart);

			creature.HP -= totalDamage;
			message.AppendLine($"Существо {creature.Name} получает {totalDamage} урона от горения. Осталось {creature.HP} хитов.");
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
			message.AppendLine($"Эффект {Name} снят.");
			creature.Effects.Remove(this);
		}

		/// <summary>
		/// Применение урона от горения
		/// </summary>
		/// <param name="creaturePart">Часть тела</param>
		/// <returns>Урон</returns>
		private static int ApplyDamage(CreaturePart creaturePart)
		{
			int damage = 5 - creaturePart.CurrentArmor;

			if (creaturePart.CurrentArmor > 0)
				creaturePart.CurrentArmor--;

			return damage < 0 ? 0 : damage;
		}

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="condition">Состояние</param>
		/// <param name="creature">Существо</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns>Навык шаблона существа</returns>
		[Obsolete("Только для тестов")]
		public static FireEffect CreateForTest(
			Guid? id = default,
			Condition condition = default,
			Creature creature = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new()
		{
			Id = id ?? Guid.NewGuid(),
			Condition = condition,
			Creature = creature,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId
		};
	}
}
