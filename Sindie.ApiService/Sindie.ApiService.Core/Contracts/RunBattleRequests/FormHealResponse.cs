using System.Collections.Generic;
using System;

namespace Sindie.ApiService.Core.Contracts.RunBattleRequests
{
	public class FormHealResponse
	{
		public Dictionary<Guid, string> EffectsOnTarget { get; set; }
	}
}