using Witcher.Core.Abstractions;
using System.Text;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - потеря головы
	/// </summary>
	public class DeadlyHead2CritEffect : CritEffect, ICrit
	{
		public DeadlyHead2CritEffect() { }

		/// <summary>
		/// Конструктор эффекта потери головы
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DeadlyHead2CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			Severity = Severity.Deadly | Severity.Unstabilizied;
			BodyPartLocation = BodyPartType.Head;
		}

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static DeadlyHead2CritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			creature.Effects.Add(DeadEffect.Create(null, null, creature, "Dead due decapitation"));

			CheckExistingEffectAndRemoveStabilizedEffect<DeadlyHead2CritEffect>(creature, aimedPart);
			return new DeadlyHead2CritEffect(creature, aimedPart, name);
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
			message.AppendLine("Невозможно вылечить или стабилизировать этот эффект");
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature)
		{
			// Method intentionally left empty.
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			// Method intentionally left empty.
		}

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			// Method intentionally left empty.
		}
	}
}
