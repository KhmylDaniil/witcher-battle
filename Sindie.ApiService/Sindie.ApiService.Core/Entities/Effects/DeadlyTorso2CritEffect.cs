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
		/// <summary>
		/// Модификатор скорости
		/// </summary>
		public int SpeedModifier { get; private set; }

		/// <summary>
		/// Модификатор скорости после стабилизации
		/// </summary>
		public int AfterTreatSpeedModifier { get; private set; }

		/// <summary>
		/// Модификатор телосложения
		/// </summary>
		public int BodyModifier { get; private set; }

		/// <summary>
		/// Модификатор телосложения после стабилизации
		/// </summary>
		public int AfterTreatBodyModifier { get; private set; }

		/// <summary>
		/// Модификатор стамины
		/// </summary>
		public int StaModifier { get; private set; }

		/// <summary>
		/// Модификатор стамины после стабилизации
		/// </summary>
		public int AfterTreatStaModifier { get; private set; }

		public DeadlyTorso2CritEffect() { }

		/// <summary>
		/// Конструктор эффекта травмы сердца
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DeadlyTorso2CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			StaModifier = (int)Math.Floor(creature.MaxSta * -0.75);
			AfterTreatStaModifier = (int)Math.Floor(creature.MaxSta * -0.5);

			SpeedModifier = (int)Math.Floor(creature.MaxSpeed * -0.75);
			AfterTreatSpeedModifier = (int)Math.Floor(creature.MaxSpeed * -0.5);

			BodyModifier = (int)Math.Floor(creature.MaxBody * -0.75);
			AfterTreatBodyModifier = (int)Math.Floor(creature.MaxBody * -0.5);

			ApplyStatChanges(creature);

			Severity = Severity.Deadly | Severity.Unstabilizied;
			BodyPartLocation = Enums.BodyPartType.Torso;
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
			if (!DyingEffect.DeathSave(creature))
			{
				creature.Effects.Add(DeadEffect.Create(null, null, creature, "Dead due heart damage"));
				return null;
			}	
			
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
		public override void AutoEnd(Creature creature, ref StringBuilder message) { }

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(Creature creature, ref StringBuilder message) { }

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
		public void ApplyStatChanges(Creature creature)
		{
			creature.Speed = creature.GetSpeed() + SpeedModifier;
			creature.Body = creature.GetBody() + BodyModifier;

			creature.MaxSta += StaModifier;
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
				creature.Speed = creature.GetSpeed() + AfterTreatSpeedModifier;
				creature.Body = creature.GetBody() + AfterTreatBodyModifier;
				creature.MaxSta -= AfterTreatStaModifier;
			}
			else
			{
				creature.Speed = creature.GetSpeed() + SpeedModifier;
				creature.Body = creature.GetBody() + BodyModifier;
				creature.MaxSta -= StaModifier;
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

			creature.Speed = creature.GetSpeed() - SpeedModifier + AfterTreatSpeedModifier;
			creature.Body = creature.GetBody() - BodyModifier + AfterTreatBodyModifier;
			creature.MaxSta -= StaModifier + AfterTreatStaModifier;
		}
	}
}
