using System.Collections.Generic;
using System;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	public sealed class FormHealResponse
	{
		public Dictionary<Guid, string> EffectsOnTarget { get; set; }
	}
}