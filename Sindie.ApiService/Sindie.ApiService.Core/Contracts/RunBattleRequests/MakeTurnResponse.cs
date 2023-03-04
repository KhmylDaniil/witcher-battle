using System.Collections.Generic;
using System;

namespace Sindie.ApiService.Core.Contracts.RunBattleRequests
{
	/// <summary>
	/// Отвевт на запрос запуска битвы
	/// </summary>
	public class MakeTurnResponse : TurnResult
	{
		public Guid BattleId { get; set; }

		public Guid Id { get; set; }
		
		public string CurrentCreatureName { get; set; }

		public Dictionary<Guid, string> CurrentEffectsOnMe { get; set; } = new();

		public Dictionary<Guid, string> PossibleTargets { get; set; } = new();

		public Dictionary<Guid, string> MyAbilities { get; set; } = new();
	}
}