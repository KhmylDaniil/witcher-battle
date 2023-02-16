using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Drafts.AbilityDrafts
{
	public static class DefensiveSkillsDrafts
	{
		/// <summary>
		/// Базовые защитные навыки для атаки ближнего боя
		/// </summary>
		public static readonly List<Skill> BaseDefensiveSkills = new() { Skill.Melee, Skill.Dodge, Skill.Athletics };
	}
}
