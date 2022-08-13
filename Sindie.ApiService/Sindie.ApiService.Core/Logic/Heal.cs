using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Entities.Effects;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sindie.ApiService.Core.Logic
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
		public void TryStabilize(Creature healer, ref Creature patient, ref StringBuilder message, Effect effect)
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
				message.AppendLine($"Не удалось стабилизировать эффект {effect.ToString()}.");

			static int CalculateHealingBase(Creature healer)
				=> Math.Max(healer.SkillBase(Skills.HealingHandsId), healer.SkillBase(Skills.FirstAidId));

			int CalculateStabilizationDifficulty(Effect effect)
			{
				if (effect is ICrit critEffect)
					return (int)critEffect.Severity;
				

				if (effect is BleedEffect)
					return 15;

				if (effect is BleedingWoundEffect bleedingWoundEffect)
					return bleedingWoundEffect.Severity;

				throw new ExceptionFieldOutOfRange<Effect>();
			}
		}
	}
}
