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
			var battle = await _authorizationService.BattleMasterFilter(_appDbContext.Battles, request.BattleId)
				.Include(i => i.Creatures)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Battle>();

			var currentCreature = battle.Creatures.FirstOrDefault(x => x.Id == request.CreatureId)
				?? throw new EntityNotFoundException<Creature>(request.CreatureId);

			battle.BattleLog += $"Ход {currentCreature.Name} завершен.";
			battle.NextInitiative++;

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
