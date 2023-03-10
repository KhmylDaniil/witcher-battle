using Sindie.ApiService.Core.Contracts.BattleRequests;
using System;

namespace Sindie.ApiService.Core.Contracts.RunBattleRequests
{
	public class RunBattleResponse : GetBattleByIdResponse
	{
		public string Message { get; set; }

		public Guid CreatureId { get; set; }

		public string CurrentCreatureName { get; set; }
	}
}