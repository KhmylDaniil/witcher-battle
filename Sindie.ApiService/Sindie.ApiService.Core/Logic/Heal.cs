using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public bool Stabilize(Creature healer, ref Creature patient, ref StringBuilder message, Effect effect)
		{
			int healingBase = CalculateHealingBase(healer);

			int stabilizationDifficulty = CalculateStabilizationDifficulty(effect);



			if (_rollService.BeatDifficulty(healingBase, 15))
			{
				message.AppendFormat($"Эффект {Name} снят.");
				creature.Effects.Remove(this);
			}
			else
				message.AppendLine($"Не удалось снять эффект {Name}.");




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
		}

		private int CalculateStabilizationDifficulty(string name)
		{
			int severity = switch
			{
				name.StartsWith("Simple") => 12,

			}
		}

	}
}
