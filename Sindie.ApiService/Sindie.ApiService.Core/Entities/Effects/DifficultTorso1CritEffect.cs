using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System.Linq;
using System.Text;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - сосущая рана грудной клетки
	/// </summary>
	public class DifficultTorso1CritEffect : CritEffect, ICrit
	{
		private const int BodyAndSpeedModifier = -3;
		private const int AfterTreatBodyAndSpeedModifier = -2;
		private const string sufflocationName = "Sucking chest wound-based sufflocation.";

		public DifficultTorso1CritEffect() { }

		/// <summary>
		/// Конструктор эффекта сосущей раны грудной клетки
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DifficultTorso1CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			ApplyStatChanges(creature);
			Severity = Severity.Difficult | Severity.Unstabilizied;
			BodyPartLocation = Enums.BodyPartType.Torso;
		}

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static DifficultTorso1CritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			if (!creature.Effects.Any(x => x is SufflocationEffect))
				creature.Effects.Add(SufflocationEffect.Create(null, null, creature, sufflocationName));

			return CheckExistingEffectAndRemoveStabilizedEffect<DifficultTorso1CritEffect>(creature, aimedPart)
				? new DifficultTorso1CritEffect(creature, aimedPart, name)
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
			creature.Body = creature.GetBody() + BodyAndSpeedModifier;
			creature.Speed = creature.GetSpeed() + BodyAndSpeedModifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			if (Severity == Severity.Difficult)
			{
				creature.Body = creature.GetBody() - AfterTreatBodyAndSpeedModifier;
				creature.Speed = creature.GetSpeed() - AfterTreatBodyAndSpeedModifier;
			}
			else
			{
				creature.Body = creature.GetBody() - BodyAndSpeedModifier;
				creature.Speed = creature.GetSpeed() - BodyAndSpeedModifier;
			}
		}

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			if (Severity == Severity.Difficult)
				return;

			Severity = Severity.Difficult;

			creature.Body = creature.GetBody() - BodyAndSpeedModifier;
			creature.Speed = creature.GetSpeed() - BodyAndSpeedModifier;

			creature.Body = creature.GetBody() + AfterTreatBodyAndSpeedModifier;
			creature.Speed = creature.GetSpeed() + AfterTreatBodyAndSpeedModifier;

			var sufflocation = creature.Effects.FirstOrDefault(x => x is SufflocationEffect && x.Name.Equals(sufflocationName));

			if (sufflocation != null)
				creature.Effects.Remove(sufflocation);
		}
	}
}
