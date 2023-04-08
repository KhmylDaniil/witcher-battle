using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Защитный навык
	/// </summary>
	public class DefensiveSkill : EntityBase
	{
		/// <summary>
		/// Навык
		/// </summary>
		public Skill Skill { get; set; }

		protected DefensiveSkill() { }

		public DefensiveSkill(Skill skill) => Skill = skill;
	}
}
