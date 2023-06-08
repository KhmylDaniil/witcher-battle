using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.RunBattleRequests
{
	public class EndTurnHandler : BaseHandler<EndTurnCommand, Unit>
	{
		public EndTurnHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(EndTurnCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.CharacterOwnerFilter(_appDbContext.Games, request.CreatureId)
				.Include(g => g.Battles.Where(b => b.Id == request.BattleId))
				.ThenInclude(b => b.Creatures)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var battle = game.Battles.FirstOrDefault()
				?? throw new EntityNotFoundException<Battle>(request.BattleId);

			var currentCreature = battle.Creatures.FirstOrDefault(x => x.Id == request.CreatureId)
				?? throw new EntityNotFoundException<Creature>(request.CreatureId);

			battle.BattleLog += $"Ход {currentCreature.Name} завершен.";
			battle.NextInitiative++;

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
