using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using System;
using System.Text;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - контузия
	/// </summary>
	public class DifficultHead1CritEffect : CritEffect, ICrit
	{
		private const int Modifier = -2;
		private const int AfterTreatModifier = -1;

		/// <summary>
		/// Счетчик раундов
		/// </summary>
		public int RoundCounter { get; private set; }

		/// <summary>
		/// Раунд следующей проверки
		/// </summary>
		public int NextCheck { get; private set; }

		public DifficultHead1CritEffect() { }

		/// <summary>
		/// Конструктор эффекта контузии
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DifficultHead1CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			NextCheck = new Random().Next(1, 6);
			ApplyStatChanges(creature);

			Severity = Severity.Difficult | Severity.Unstabilizied;
			BodyPartLocation = Enums.BodyPartType.Head;
		}

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static DifficultHead1CritEffect Create(Creature creature, CreaturePart aimedPart, string name)
			=> CheckExistingEffectAndRemoveStabilizedEffect<DifficultHead1CritEffect>(creature, aimedPart)
				? new DifficultHead1CritEffect(creature, aimedPart, name)
				: null;

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(Creature creature, ref StringBuilder message)
		{
			RoundCounter++;

			if (RoundCounter == NextCheck)
			{
				NextCheck += new Random().Next(1, 6);
				StunCheck(creature, ref message);
			}
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature)
		{
			creature.Int = creature.GetInt() + Modifier;
			creature.Ref = creature.GetRef() + Modifier;
			creature.Dex = creature.GetDex() + Modifier;
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
				creature.Ref = creature.GetRef() - AfterTreatModifier;
				creature.Dex = creature.GetDex() - AfterTreatModifier;
			}
			else
			{
				creature.Int = creature.GetInt() - Modifier;
				creature.Ref = creature.GetRef() - Modifier;
				creature.Dex = creature.GetDex() - Modifier;
			}
		}

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			if (!IsStabile(Severity))
			
			Severity = Severity.Difficult;

			creature.Int = creature.GetInt() - Modifier + AfterTreatModifier;
			creature.Ref = creature.GetRef() - Modifier + AfterTreatModifier;
			creature.Dex = creature.GetDex() - Modifier + AfterTreatModifier;
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
				var stun = StunEffect.Create(null, null, target: creature, "Concussion-based Stun");

				if (stun is null)
					return;

				creature.Effects.Add(stun);
				message.AppendLine($"Из-за контузии наложен эффект {stun.Name}.");
			}
		}
	}
}
