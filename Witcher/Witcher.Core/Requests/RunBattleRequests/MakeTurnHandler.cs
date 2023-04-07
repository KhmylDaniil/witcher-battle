using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.RunBattleRequests
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
					?? throw new NoAccessToEntityException<Battle>();

			var currentCreature = battle.Creatures.FirstOrDefault(x => x.Id == request.CreatureId)
				?? throw new EntityNotFoundException<Creature>(request.CreatureId);

			return new MakeTurnResponse
			{
				BattleId = battle.Id,
				CreatureId = currentCreature.Id,
				CurrentCreatureName = currentCreature.Name,
				MultiAttackAbilityId = currentCreature.Turn.MuitiattackAbilityId,
				PossibleTargets = battle.Creatures.ToDictionary(x => x.Id, x => x.Name),
				MyAbilities = currentCreature.Abilities.ToDictionary(x => x.Id, x => x.Name),
				TurnState = currentCreature.Turn.TurnState
			};
		}
	}
}
