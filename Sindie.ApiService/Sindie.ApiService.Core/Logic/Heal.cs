using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Entities.Effects;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
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
				message.AppendLine($"Не удалось стабилизировать эффект {effect.Name}.");

			static int CalculateHealingBase(Creature healer)
			{
				var healingBase = healer.Cra;

				var healingSkills = healer.CreatureSkills.Where(x => x.SkillId == Skills.FirstAidId || x.SkillId == Skills.HealingHandsId).ToList();

				if (healingSkills.Any())
				{
					var sortedList = from x in healingSkills
									 orderby healer.SkillBase(x.SkillId)
									 select x;
					healingBase = healer.SkillBase(sortedList.First().SkillId);
				}

				return healingBase;
			}

			static int CalculateStabilizationDifficulty(Effect effect)
			{
				if (effect.Name.StartsWith("Simple"))
					return 12;

				if (effect.Name.StartsWith("Complex"))
					return 14;

				if (effect.Name.StartsWith("Difficult"))
					return 16;

				if (effect.Name.StartsWith("Deadly"))
					return 18;

				if (effect.Name.Equals(Conditions.BleedName))
					return 15;

				if (effect.Name.Equals(Conditions.BleedingWoundName))
					return (effect as BleedingWoundEffect).Severity;

				throw new ExceptionFieldOutOfRange<Effect>(nameof(effect.Name));
			}
		}

		//public static void ApplyPenalty<TEntity>(Creature creature, TEntity wingCrit) where TEntity : IWingCrit, ILegCrit
		//{
		//	var type = typeof(TEntity);
			
			
		//	var existingCrit = creature.Effects.FirstOrDefault(x => x is TEntity crit && crit.PenaltyApplied) as TEntity;

		//	if (existingCrit is null || wingCrit.Severity < existingCrit.Severity)
		//		return;

		//	existingCrit.RevertStatChanges(creature);
		//	wingCrit.ApplyStatChanges(creature);
		//}

		//public static void UpdatePenalty(Creature creature, IWingCrit wingCrit)
		//{
		//	var severiestCrit = creature.Effects.Where(x => x.Id != wingCrit.Id
		//	&& x is IWingCrit crit && crit.Severity > wingCrit.Severity).Cast<IWingCrit>().OrderBy(x => x.Severity).FirstOrDefault();

		//	if (severiestCrit != null)
		//		severiestCrit.ApplyStatChanges(creature);
		//}




		public static void ApplyPenalty(Creature creature, IWingCrit wingCrit)
		{
			var existingCrit = creature.Effects.FirstOrDefault(x => x is IWingCrit crit && crit.PenaltyApplied) as IWingCrit;

			if (existingCrit is null || wingCrit.Severity < existingCrit.Severity)
				return;

			existingCrit.RevertStatChanges(creature);
			wingCrit.ApplyStatChanges(creature);
		}

		public static void UpdatePenalty(Creature creature, IWingCrit wingCrit)
		{
			var severiestCrit = creature.Effects.Where(x => x.Id != wingCrit.Id
			&& x is IWingCrit crit && crit.Severity > wingCrit.Severity).Cast<IWingCrit>().OrderBy(x => x.Severity).FirstOrDefault();

			if (severiestCrit != null)
				severiestCrit.ApplyStatChanges(creature);
		}

		public static void ApplyPenalty(Creature creature, ILegCrit wingCrit)
		{
			var existingCrit = creature.Effects.FirstOrDefault(x => x is IWingCrit crit && crit.PenaltyApplied) as IWingCrit;

			if (existingCrit is null || wingCrit.Severity < existingCrit.Severity)
				return;

			existingCrit.RevertStatChanges(creature);
			wingCrit.ApplyStatChanges(creature);
		}

		public static void UpdatePenalty(Creature creature, ILegCrit wingCrit)
		{
			var severiestCrit = creature.Effects.Where(x => x.Id != wingCrit.Id
			&& x is IWingCrit crit && crit.Severity > wingCrit.Severity).Cast<IWingCrit>().OrderBy(x => x.Severity).FirstOrDefault();

			if (severiestCrit != null)
				severiestCrit.ApplyStatChanges(creature);
		}
	}
}
