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
	/// Критический эффект - травма сердца
	/// </summary>
	public class DeadlyTorso2CritEffect : CritEffect, ICrit
	{
		private int _speedModifier;
		private int _afterTreatSpeedModifier;

		private int _bodyModifier;
		private int _afterTreatBodyModifier;

		private int _staModifier;
		private int _afterTreatStaModifier;

		/// <summary>
		/// Тяжесть критического эффекта
		/// </summary>
		public Severity Severity { get; private set; } = Severity.Deadly | Severity.Unstabilizied;

		/// <summary>
		/// Тип части тела
		/// </summary
		public Enums.BodyPartType BodyPartLocation { get; } = Enums.BodyPartType.Torso;

		public DeadlyTorso2CritEffect() { }

		/// <summary>
		/// Конструктор эффекта травмы сердца
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DeadlyTorso2CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			_staModifier = (int)Math.Floor(creature.MaxSta * -0.75);
			_afterTreatStaModifier = (int)Math.Floor(creature.MaxSta * -0.5);

			_speedModifier = (int)Math.Floor(creature.MaxSpeed * -0.75);
			_afterTreatSpeedModifier = (int)Math.Floor(creature.MaxSpeed * -0.5);

			_bodyModifier = (int)Math.Floor(creature.MaxBody * -0.75);
			_afterTreatBodyModifier = (int)Math.Floor(creature.MaxBody * -0.5);

			ApplyStatChanges(creature);
		}

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static DeadlyTorso2CritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			//TODO если не мертв
			
			if (!creature.Effects.Any(x => x is BleedEffect))
				creature.Effects.Add(BleedEffect.Create(null, null, creature, "Secondary Bleed"));

			return CheckExistingEffectAndRemoveStabilizedEffect<DeadlyTorso2CritEffect>(creature, aimedPart)
				? new DeadlyTorso2CritEffect(creature, aimedPart, name)
				: null;
		}

		/// <summary>
		/// Автоматически прекратить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void AutoEnd(ref Creature creature, ref StringBuilder message) { }

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(ref Creature creature, ref StringBuilder message) { }

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
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature)
		{
			creature.Speed = creature.GetSpeed() + _speedModifier;
			creature.Body = creature.GetBody() + _bodyModifier;

			creature.MaxSta += _staModifier;
			if (creature.Sta > creature.MaxSta)
				creature.Sta = creature.MaxSta;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			if (IsStabile(Severity))
			{
				creature.Speed = creature.GetSpeed() + _afterTreatSpeedModifier;
				creature.Body = creature.GetBody() + _afterTreatBodyModifier;
				creature.MaxSta -= _afterTreatStaModifier;
			}
			else
			{
				creature.Speed = creature.GetSpeed() + _speedModifier;
				creature.Body = creature.GetBody() + _bodyModifier;
				creature.MaxSta -= _staModifier;
			}
		}

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			if (!IsStabile(Severity))
				return;

			Severity = Severity.Deadly;

			creature.Speed = creature.GetSpeed() - _speedModifier + _afterTreatSpeedModifier;
			creature.Body = creature.GetBody() - _bodyModifier + _afterTreatBodyModifier;
			creature.MaxSta -= _staModifier + _afterTreatStaModifier;
		}
	}
}
