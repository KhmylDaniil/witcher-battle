using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.RunBattleRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.RunBattleRequests
{
	public class MakeTurnHandler : BaseHandler<MakeTurnCommand, MakeTurnResponse>
	{
		public MakeTurnHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<MakeTurnResponse> Handle(MakeTurnCommand request, CancellationToken cancellationToken)
		{
			var battle = await _authorizationService.BattleMasterFilter(_appDbContext.Battles, request.BattleId)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Abilities)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Battle>();

			var currentCreature = battle.Creatures.FirstOrDefault(x => x.Id == request.CreatureId)
				?? throw new ExceptionEntityNotFound<Creature>(request.CreatureId);

			return new MakeTurnResponse
			{ 
				BattleId = battle.Id,
				CreatureId = currentCreature.Id,
				CurrentCreatureName = currentCreature.Name,
				PossibleTargets = battle.Creatures.ToDictionary(x => x.Id, x => x.Name),
				MyAbilities = currentCreature.Abilities.ToDictionary(x => x.Id, x => x.Name)
			};
		}
	}
}
