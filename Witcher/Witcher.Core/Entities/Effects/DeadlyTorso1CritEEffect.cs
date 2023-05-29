using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Logic;
using System;
using System.Linq;
using System.Text;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - септический шок
	/// </summary>
	public class DeadlyTorso1CritEffect : CritEffect, ICrit
	{
		private const int Modifier = -3;
		private const int AfterTreatModifier = -1;

		/// <summary>
		/// Модификатор стамины
		/// </summary>
		public int StaModifier { get; private set; }

		/// <summary>
		/// Модификатор стамины после стабилизации
		/// </summary>
		public int AfterTreatStaModifier { get; private set; }

		private const string poisonName = "Septic shock-based poison.";

		public DeadlyTorso1CritEffect() { }

		/// <summary>
		/// Конструктор эффекта септическоо шока
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DeadlyTorso1CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			StaModifier = (int)Math.Floor(creature.MaxSta * -0.75);
			AfterTreatStaModifier = (int)Math.Floor(creature.MaxSta * -0.5);

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
		public static DeadlyTorso1CritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			if (!creature.Effects.Any(x => x is PoisonEffect))
				creature.Effects.Add(PoisonEffect.Create(null, null, creature, poisonName));

			CheckExistingEffectAndRemoveStabilizedEffect<DeadlyTorso1CritEffect>(creature, aimedPart);
			return new DeadlyTorso1CritEffect(creature, aimedPart, name);
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature)
		{
			creature.Int = creature.GetInt() + Modifier;
			creature.Will = creature.GetWill() + Modifier;
			creature.Ref = creature.GetRef() + Modifier;
			creature.Dex = creature.GetDex() + Modifier;

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
				creature.Int = creature.GetInt() - AfterTreatModifier;
				creature.Will = creature.GetWill() - AfterTreatModifier;
				creature.Ref = creature.GetRef() - AfterTreatModifier;
				creature.Dex = creature.GetDex() - AfterTreatModifier;
				creature.MaxSta -= AfterTreatStaModifier;
			}
			else
			{
				creature.Int = creature.GetInt() - Modifier;
				creature.Will = creature.GetWill() - Modifier;
				creature.Ref = creature.GetRef() - Modifier;
				creature.Dex = creature.GetDex() - Modifier;
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

			creature.Int = creature.GetInt() - Modifier + AfterTreatModifier;
			creature.Will = creature.GetWill() - Modifier + AfterTreatModifier;
			creature.Ref = creature.GetRef() - Modifier + AfterTreatModifier;
			creature.Dex = creature.GetDex() - Modifier + AfterTreatModifier;
			creature.MaxSta -= StaModifier + AfterTreatStaModifier;

			var poison = creature.Effects.FirstOrDefault(x => x is PoisonEffect && x.Name.Equals(poisonName));

			if (poison != null)
				creature.Effects.Remove(poison);
		}
	}
}
