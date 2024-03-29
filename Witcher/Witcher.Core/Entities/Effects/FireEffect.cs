﻿using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using System;
using System.Linq;
using System.Text;

namespace Witcher.Core.Entities.Effects
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
		/// <param name="name">Название</param>
		private FireEffect(Creature creature, string name) : base(creature, name) { }

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="attacker">Атакующий</param>
		/// <param name="target">Цель</param>
		/// <returns>Эффект</returns>
		public static FireEffect Create(IRollService rollService, Creature attacker, Creature target, string name)
			=> target.Effects.Any(x => x is FireEffect)
				? null
				: new FireEffect(target, name);

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(Creature creature, ref StringBuilder message)
		{
			int totalDamage = 0;
			
			foreach (var creaturePart in creature.CreatureParts)
				totalDamage += ApplyDamage(creaturePart);

			creature.HP -= totalDamage;
			message.AppendLine($"Существо {creature.Name} получает {totalDamage} урона от горения. Осталось {creature.HP} хитов.");
		}

		/// <summary>
		/// Попробовать снять эффект
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="healer">Лекарь</param>
		/// <param name="patient">Цель</param>
		/// <param name="message">Сообщение</param>
		public override void Treat(IRollService rollService, Creature healer, Creature patient, ref StringBuilder message)
		{
			message.AppendLine($"Эффект {Name} снят.");
			patient.Effects.Remove(this);
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
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns>Навык шаблона существа</returns>
		[Obsolete("Только для тестов")]
		public static FireEffect CreateForTest(
			Guid? id = default,
			Creature creature = default,
			string name = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new()
		{
			Id = id ?? Guid.NewGuid(),
			Creature = creature,
			Name = name ?? Enum.GetName(Condition.Fire),
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId
		};
	}
}
