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
			var battle = await _authorizationService.BattleMasterFilter(_appDbContext.Battles, request.Id)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.CreatureSkills)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.CreatureParts)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.DamageTypeModifiers)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Effects)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Abilities)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.CreatureTemplate)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Battle>();

			var currentCreature = battle.Creatures.FirstOrDefault(x => x.Id == request.CreatureId)
				?? throw new ExceptionEntityNotFound<Creature>(request.CreatureId);

			var result = new MakeTurnResponse
			{ 
				Id = battle.Id,
				CreatureId = currentCreature.Id,
				CurrentCreatureName = currentCreature.Name,
				CurrentEffectsOnMe = currentCreature.Effects.ToDictionary(x => x.Id, x => x.Name),
				PossibleTargets = battle.Creatures.Where(x => x.Id != currentCreature.Id).ToDictionary(x => x.Id, x => x.Name),
				MyAbilities = currentCreature.Abilities.ToDictionary(x => x.Id, x => x.Name)
			};
			return result;
		}
	}
}
