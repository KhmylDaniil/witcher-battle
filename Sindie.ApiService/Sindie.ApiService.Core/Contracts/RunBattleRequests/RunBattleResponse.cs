using Sindie.ApiService.Core.Contracts.BattleRequests;
using System.Collections.Generic;
using System;

namespace Sindie.ApiService.Core.Contracts.RunBattleRequests
{
	public class RunBattleResponse : GetBattleByIdResponse
	{
		public string Message { get; set; }

		public Guid CreatureId { get; set; }

		public string CurrentCreatureName { get; set; }

		//public Dictionary<Guid, string> CurrentEffectsOnMe { get; set; } = new();

		//public Dictionary<Guid, string> PossibleTargets { get; set; } = new();

		//public Dictionary<Guid, string> MyAbilities { get; set; } = new();
	}
}