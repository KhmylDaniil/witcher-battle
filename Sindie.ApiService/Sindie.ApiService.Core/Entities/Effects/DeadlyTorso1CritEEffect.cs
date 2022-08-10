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
	/// Критический эффект - септический шок
	/// </summary>
	public class DeadlyTorso1CritEffect : CritEffect, ICrit
	{
		private const int Modifier = -3;
		private const int AfterTreatModifier = -1;
		
		private int _staModifier;
		private int _afterTreatStaModifier;

		private const string poisonName = "Septic shock-based poison.";

		/// <summary>
		/// Тяжесть критического эффекта
		/// </summary>
		public Severity Severity { get; private set; } = Severity.Deadly | Severity.Unstabilizied;

		/// <summary>
		/// Тип части тела
		/// </summary
		public Enums.BodyPartType BodyPartLocation { get; } = Enums.BodyPartType.Torso;

		public DeadlyTorso1CritEffect() { }

		/// <summary>
		/// Конструктор эффекта септическоо шока
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DeadlyTorso1CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			_staModifier = (int)Math.Floor(creature.MaxSta * -0.75);
			_afterTreatStaModifier = (int)Math.Floor(creature.MaxSta * -0.5);

			ApplyStatChanges(creature);
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
			if (creature.Effects.Any(x => x is PoisonEffect))
				creature.Effects.Add(PoisonEffect.Create(null, null, creature, poisonName));

			return CheckExistingEffectAndRemoveStabilizedEffect<DeadlyTorso1CritEffect>(creature, aimedPart)
				? new DeadlyTorso1CritEffect(creature, aimedPart, name)
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
			creature.Int = creature.GetInt() + Modifier;
			creature.Will = creature.GetWill() + Modifier;
			creature.Ref = creature.GetRef() + Modifier;
			creature.Dex = creature.GetDex() + Modifier;

			creature.MaxSta -= _staModifier;
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
				creature.MaxSta -= _afterTreatStaModifier;
			}
			else
			{
				creature.Int = creature.GetInt() - Modifier;
				creature.Will = creature.GetWill() - Modifier;
				creature.Ref = creature.GetRef() - Modifier;
				creature.Dex = creature.GetDex() - Modifier;
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

			creature.Int = creature.GetInt() - Modifier + AfterTreatModifier;
			creature.Will = creature.GetWill() - Modifier + AfterTreatModifier;
			creature.Ref = creature.GetRef() - Modifier + AfterTreatModifier;
			creature.Dex = creature.GetDex() - Modifier + AfterTreatModifier;
			creature.MaxSta -= _staModifier + _afterTreatStaModifier;

			var poison = creature.Effects.FirstOrDefault(x => x is PoisonEffect && x.Name.Equals(poisonName));

			if (poison != null)
				creature.Effects.Remove(poison);
		}
	}
}
