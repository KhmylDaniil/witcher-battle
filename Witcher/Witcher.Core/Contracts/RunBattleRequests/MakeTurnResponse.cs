using System.Collections.Generic;
using System;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	/// <summary>
	/// Отвевт на запрос данных для совершения хода
	/// </summary>
	public class MakeTurnResponse 
	{
		public Guid BattleId { get; set; }

		public Guid CreatureId { get; set; }
		
		public string CurrentCreatureName { get; set; }

		public Dictionary<Guid, string> PossibleTargets { get; set; } = new();

		public Dictionary<Guid, string> MyAbilities { get; set; } = new();
	}
}