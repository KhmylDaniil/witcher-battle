using MediatR;
using System;
using System.Collections.Generic;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	/// <summary>
	/// Service query for for battle participants userId list for SignalR hub work only
	/// </summary>
	public sealed class GetUserIdListForBattleQuery : IRequest<IReadOnlyList<string>>
	{
		public Guid GameId { get; set; }

		public Guid BattleId { get; set; }
	}
}
