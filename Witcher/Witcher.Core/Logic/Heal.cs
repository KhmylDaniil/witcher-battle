using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Entities;
using Witcher.Core.Entities.Effects;
using Witcher.Core.Exceptions.EntityExceptions;
using System;
using System.Text;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Exceptions;

namespace Witcher.Core.Logic
{
	/// <summary>
	/// Действие лечения
	/// </summary>
	public sealed class Heal
	{
		private readonly IRollService _rollService;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="rollService"></param>
		public Heal(IRollService rollService)
		{
			_rollService = rollService;
		}

		/// <summary>
		/// Стабилизировать критический эффект или эффект кровотечения
		/// </summary>
		/// <param name="healer">Лекарь</param>
		/// <param name="patient">Пациент</param>
		/// <param name="message">Сообщение</param>
		/// <param name="effect">Эффект</param>
		public void TryStabilize(Creature healer, Creature patient, ref StringBuilder message, Effect effect)
		{
			int healingBase = CalculateHealingBase(healer);

			int stabilizationDifficulty = CalculateStabilizationDifficulty(effect);

			if (_rollService.BeatDifficulty(healingBase, stabilizationDifficulty))
			{
				message.AppendLine($"Эффект {effect.Name} стабилизирован.");

				if (effect is ICrit)
					(effect as ICrit).Stabilize(patient);
				else
					patient.Effects.Remove(effect);
			}
			else
				message.AppendLine($"Не удалось стабилизировать эффект {effect.Name}.");

			static int CalculateHealingBase(Creature healer)
				=> Math.Max(healer.SkillBase(Skill.HealingHands), healer.SkillBase(Skill.FirstAid));

			int CalculateStabilizationDifficulty(Effect effect)
			{
				if (effect is ICrit critEffect)
					return (int)critEffect.Severity - (int)Severity.Unstabilizied;

				if (effect is BleedEffect)
					return 15;

				if (effect is BleedingWoundEffect bleedingWoundEffect)
					return bleedingWoundEffect.Severity;

				if (effect is DyingEffect)
					return patient.HP * -1;

				throw new LogicBaseException($"Эффект {effect.Name} не относится к стабилизируемым эффектам. Попытка стабилизации невозможна.");
			}
		}
	}
}
