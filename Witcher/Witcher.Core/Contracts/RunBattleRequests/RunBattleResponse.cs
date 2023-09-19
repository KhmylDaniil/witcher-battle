using Witcher.Core.Contracts.BattleRequests;
using System;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	public sealed class RunBattleResponse : GetBattleByIdResponse
	{
		public string BattleLog { get; set; }

		public Guid CreatureId { get; set; }

		public string CurrentCreatureName { get; set; }
	}
}