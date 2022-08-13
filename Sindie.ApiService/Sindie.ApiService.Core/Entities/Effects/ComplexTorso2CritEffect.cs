using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System;
using System.Linq;
using System.Text;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - разрыв селезенки
	/// </summary>
	public class ComplexTorso2CritEffect : CritEffect, ICrit
	{
		/// <summary>
		/// Счетчик раундов
		/// </summary>
		public int RoundCounter { get; private set; }

		/// <summary>
		/// Тяжесть критического эффекта
		/// </summary>
		public Severity Severity { get; private set; } = Severity.Complex | Severity.Unstabilizied;

		/// <summary>
		/// Тип части тела
		/// </summary
		public Enums.BodyPartType BodyPartLocation { get; } = Enums.BodyPartType.Torso;

		public ComplexTorso2CritEffect() { }

		/// <summary>
		/// Конструктор эффекта разрыва селезенки
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private ComplexTorso2CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name) { }

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static ComplexTorso2CritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			if (!creature.Effects.Any(x => x is BleedEffect))
				creature.Effects.Add(BleedEffect.Create(null, null, creature, "Secondary Bleed"));

			return CheckExistingEffectAndRemoveStabilizedEffect<ComplexTorso2CritEffect>(creature, aimedPart)
				? new ComplexTorso2CritEffect(creature, aimedPart, name)
				: null;
		}

		/// <summary>
		/// Автоматически прекратить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void AutoEnd(Creature creature, ref StringBuilder message) { }

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(Creature creature, ref StringBuilder message)
		{
			RoundCounter++;

			if ((RoundCounter % 5 == 0 && Severity == (Severity.Unstabilizied | Severity.Complex))
				||
				(RoundCounter % 10 == 0 && Severity == Severity.Complex))
				StunCheck(creature, ref message);
		}

		/// <summary>
		/// Попробовать снять эффект
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Treat(IRollService rollService, Creature creature, ref StringBuilder message)
		{
			Heal heal = new(rollService);

			heal.TryStabilize(creature, creature, ref message, this);
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature) { }

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature) { }

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			Severity = Severity.Complex;
		}

		/// <summary>
		/// Наложение дизориентации
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		void StunCheck(Creature creature, ref StringBuilder message)
		{
			Random random = new();
			if (random.Next() >= creature.Stun)
			{
				var stun = StunEffect.Create(null, null, target: creature, "Ruptured Spleen-based Stun");

				if (stun is null)
					return;

				creature.Effects.Add(stun);
				message.AppendLine($"Из-за разрыва селезенки наложен эффект {Conditions.StunName}.");
			}
		}
	}
}
