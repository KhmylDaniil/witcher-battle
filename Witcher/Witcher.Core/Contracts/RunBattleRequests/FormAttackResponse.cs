using System;
using System.Collections.Generic;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	public class FormAttackResponse
	{
		public Dictionary<string, Guid?> CreatureParts { get; set; }

		public Dictionary<string, Skill?> DefensiveSkills { get; set; }
	}
}