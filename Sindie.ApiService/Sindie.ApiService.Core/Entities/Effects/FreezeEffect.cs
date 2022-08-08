using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using System;
using System.Linq;
using System.Text;

namespace Sindie.ApiService.Core.Entities.Effects
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
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(ref Creature creature, ref StringBuilder message) { }

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
			if (rollService.BeatDifficulty(creature.SkillBase(Skills.PhysiqueId), 16))
			{
				RevertStatChanges(creature);
				message.AppendFormat($"Эффект {Conditions.FreezeName} снят. Скорость равна {creature.Speed}, рефлексы равны {creature.Ref}.");
				creature.Effects.Remove(this);
			}
			else
				message.AppendLine($"Не удалось снять эффект {Conditions.FreezeName}.");
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		private void ApplyStatChanges(Creature creature)
		{
			creature.Speed += SpeedModifier;
			creature.Ref += RefModifier;
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
				Name = name ?? Conditions.FreezeName,
				CreatedOn = createdOn,
				ModifiedOn = modifiedOn,
				CreatedByUserId = createdByUserId
			};

			effect.ApplyStatChanges(effect.Creature);
			return effect;
		}
	}
}
