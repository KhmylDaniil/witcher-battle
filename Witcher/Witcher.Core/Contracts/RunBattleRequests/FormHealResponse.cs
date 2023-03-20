using System.Collections.Generic;
using System;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	public class FormHealResponse
	{
		public Dictionary<Guid, string> EffectsOnTarget { get; set; }
	}
}