using System;
using System.Collections.Generic;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Contracts.RunBattleRequests
{
	public class FormAttackResponse
	{
		public Dictionary<string, Guid?> CreatureParts { get; set; }

		public Dictionary<string, Skill?> DefensiveSkills { get; set; }
	}
}