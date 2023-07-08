using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Exceptions;

namespace Witcher.Core.Requests.RunBattleRequests
{
	/// <summary>
	/// Service handler for battle participants userId list for SignalR hub work only, dont call manually
	/// </summary>
	public class GetUserIdListForBattleHandler : BaseHandler<GetUserIdListForBattleQuery, IReadOnlyList<string>>
	{
		public GetUserIdListForBattleHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<IReadOnlyList<string>> Handle(GetUserIdListForBattleQuery request, CancellationToken cancellationToken)
		{
			var game = await _appDbContext.Games
				.Include(g => g.UserGames.Where(c => c.UserGameCharacters.Any(usg => usg.Character.BattleId == request.BattleId)))
				.FirstOrDefaultAsync(g => g.Id == request.GameId, cancellationToken)
					?? throw new LogicBaseException($"Game with id {request.GameId} is not found");

			return game.UserGames.Select(ug => ug.UserId.ToString()).ToList();
		}
	}
}
