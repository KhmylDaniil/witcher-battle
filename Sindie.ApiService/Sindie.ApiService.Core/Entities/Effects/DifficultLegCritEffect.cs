using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System.Linq;
using System.Text;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - Открытый перелом ноги
	/// </summary>
	public class DifficultLegCritEffect : CritEffect, ISharedPenaltyCrit
	{
		private int _speedModifier;
		private int _dodgeModifier;
		private int _athleticsModifier;

		private int _afterTreatSpeedModifier;
		private int _afterTreatDodgeModifier;
		private int _afterTreatAthleticsModifier;

		private DifficultLegCritEffect() { }

		/// <summary>
		/// Конструктор эффекта открытого перелома ноги
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DifficultLegCritEffect(Creature creature, CreaturePart aimedPart, string name) : base (creature, aimedPart, name)
		{
			_speedModifier = creature.CreatureTemplate.Speed / 4 * -3;
			_afterTreatSpeedModifier = creature.CreatureTemplate.Speed / -2;

			_dodgeModifier = creature.SkillBase(Skills.DodgeId) / 4 * -3;
			_afterTreatDodgeModifier = creature.SkillBase(Skills.DodgeId) / -2;

			_athleticsModifier = creature.SkillBase(Skills.AthleticsId) / 4 * -3;
			_afterTreatAthleticsModifier = creature.SkillBase(Skills.AthleticsId) / -2;
		}

		/// <summary>
		/// Тяжесть критического эффекта
		/// </summary>
		public Severity Severity { get; private set; } = Severity.Difficult | Severity.Unstabilizied;

		/// <summary>
		/// Тип части тела
		/// </summary
		public Enums.BodyPartType BodyPartLocation { get; } = Enums.BodyPartType.Leg;

		/// <summary>
		/// Пенальти применено
		/// </summary>
		public bool PenaltyApplied { get; private set; }

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static DifficultLegCritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			if (!creature.Effects.Any(x => x is BleedEffect))
				creature.Effects.Add(BleedEffect.Create(null, null, creature, "Secondary Bleed"));

			if (creature.Effects.Any(x => x is DifficultLegCritEffect crit && crit.CreaturePartId == aimedPart.Id))
				return null;

			var effect = new DifficultLegCritEffect(creature, aimedPart, name);

			ApplySharedPenalty(creature, effect);

			return effect;
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
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			if (Severity == Severity.Complex)
				return;

			Severity = Severity.Complex;

			creature.Speed -= _speedModifier;
			creature.Speed += _afterTreatSpeedModifier;
			//TODO уточнить как будет работать, если не будет скилла
			var dodge = creature.CreatureSkills.FirstOrDefault(x => x.Id == Skills.DodgeId);
			if (dodge is not null)
			{
				dodge.SkillValue -= _dodgeModifier;
				dodge.SkillValue += _afterTreatDodgeModifier;
			}

			var athletics = creature.CreatureSkills.FirstOrDefault(x => x.Id == Skills.AthleticsId);
			if (athletics is not null)
			{
				athletics.SkillValue -= _athleticsModifier;
				athletics.SkillValue += _afterTreatAthleticsModifier;
			}

			SharedPenaltyMovedToAnotherCrit(creature, this);
		}

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
			PenaltyApplied = true;

			creature.Speed += _speedModifier;

			var dodge = creature.CreatureSkills.FirstOrDefault(x => x.SkillId == Skills.DodgeId);
			if (dodge is not null)
				dodge.SkillValue += _dodgeModifier;

			var athletics = creature.CreatureSkills.FirstOrDefault(x => x.SkillId == Skills.AthleticsId);
			if (athletics is not null)
				athletics.SkillValue += _athleticsModifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			PenaltyApplied = false;

			var dodge = creature.CreatureSkills.FirstOrDefault(x => x.Id == Skills.DodgeId);
			var athletics = creature.CreatureSkills.FirstOrDefault(x => x.Id == Skills.AthleticsId);

			if (Severity == Severity.Difficult)
			{
				creature.Speed -= _afterTreatSpeedModifier;

				if (dodge != null)
					dodge.SkillValue -= _afterTreatDodgeModifier;

				if (athletics != null)
					athletics.SkillValue -= _afterTreatAthleticsModifier;
			}
			else
			{
				creature.Speed -= _speedModifier;

				if (dodge != null)
					dodge.SkillValue -= _dodgeModifier;

				if (athletics != null)
					athletics.SkillValue -= _athleticsModifier;
			}
		}
	}
}
