using System;
using System.Collections.Generic;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	/// <summary>
	/// Service query for for battle participants userId list for SignalR hub work only
	/// </summary>
	public class GetUserIdListForBattleQuery : IValidatableCommand<IReadOnlyList<string>>
	{
		public Guid GameId { get; set; }

		public Guid BattleId { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
