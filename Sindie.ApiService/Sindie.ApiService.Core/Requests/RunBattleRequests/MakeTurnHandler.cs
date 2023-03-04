using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.RunBattleRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Logic;
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

			var message = RunBattle.TurnBeginning(battle, out Creature currentCreature);

			var result = new MakeTurnResponse() { BattleId = battle.Id, Message = message };

			if (currentCreature != null)
			{
				result.Id = currentCreature.Id;
				result.CurrentCreatureName = currentCreature.Name;
				result.CurrentEffectsOnMe = currentCreature.Effects.ToDictionary(x => x.Id, x => x.Name);
				result.PossibleTargets = battle.Creatures.Where(x => x.Id != currentCreature.Id).ToDictionary(x => x.Id, x => x.Name);
				result.MyAbilities = currentCreature.Abilities.ToDictionary(x => x.Id, x => x.Name);
			}

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return result;
		}
	}
}
