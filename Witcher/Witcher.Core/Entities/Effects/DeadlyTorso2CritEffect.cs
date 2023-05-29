using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using System;
using System.Linq;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities.Effects
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

			CheckExistingEffectAndRemoveStabilizedEffect<DeadlyTorso2CritEffect>(creature, aimedPart);
			return new DeadlyTorso2CritEffect(creature, aimedPart, name);
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
