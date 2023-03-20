using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using System;
using System.Linq;
using System.Text;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities.Effects
{
	/// <summary>
	/// Эффект заморозки
	/// </summary>
	public sealed class FreezeEffect : Effect
	{
		private const int SpeedModifier = -3;
		private const int RefModifier = -1;

		private FreezeEffect() { }

		/// <summary>
		/// Конструктор эффекта заморозки
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		private FreezeEffect(Creature creature, string name) : base(creature, name)
			=> ApplyStatChanges(creature);

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="attacker">Атакующий</param>
		/// <param name="target">Цель</param>
		/// <param name="name">Название</param>
		/// <returns>Эффект</returns>
		public static FreezeEffect Create(IRollService rollService, Creature attacker, Creature target, string name)
			=> target.Effects.Any(x => x is FreezeEffect)
				? null
				: new FreezeEffect(target, name);

		/// <summary>
		/// Попробовать снять эффект
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="healer">Лекарь</param>
		/// <param name="patient">Цель</param>
		/// <param name="message">Сообщение</param>
		public override void Treat(IRollService rollService, Creature healer, Creature patient, ref StringBuilder message)
		{
			if (rollService.BeatDifficulty(patient.SkillBase(Skill.Physique), 16))
			{
				RevertStatChanges(patient);
				message.AppendFormat($"Эффект {Name} снят. Скорость равна {patient.Speed}, рефлексы равны {patient.Ref}.");
				patient.Effects.Remove(this);
			}
			else
				message.AppendLine($"Не удалось снять эффект {Name}.");
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		private void ApplyStatChanges(Creature creature)
		{
			creature.Speed = creature.GetSpeed() + SpeedModifier;
			creature.Ref = creature.GetRef() + RefModifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		private void RevertStatChanges(Creature creature)
		{
			creature.Speed = creature.GetSpeed() - SpeedModifier;
			creature.Ref = creature.GetRef() - RefModifier;
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
		public static FreezeEffect CreateForTest(
			Guid? id = default,
			Creature creature = default,
			string name = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		{
			FreezeEffect effect = new()
			{
				Id = id ?? Guid.NewGuid(),
				Creature = creature,
				Name = name ?? Enum.GetName(Condition.Freeze),
				CreatedOn = createdOn,
				ModifiedOn = modifiedOn,
				CreatedByUserId = createdByUserId
			};

			effect.ApplyStatChanges(effect.Creature);
			return effect;
		}
	}
}
