using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Logic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.RunBattleRequests
{
	public class RunBattleHandler : BaseHandler<RunBattleCommand, RunBattleResponse>
	{
		public RunBattleHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<RunBattleResponse> Handle(RunBattleCommand request, CancellationToken cancellationToken)
		{
			var battle = await _authorizationService.BattleMasterFilter(_appDbContext.Battles, request.BattleId)
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
					?? throw new NoAccessToEntityException<Battle>();

			var creatures = battle.Creatures.Select(x => new GetBattleByIdResponseItem()
			{
				Id = x.Id,
				Name = x.Name,
				CreatureTemplateName = x.CreatureTemplate.Name,
				Description = x.Description,
				HP = (x.HP, x.MaxHP),
				Effects = string.Join(", ", x.Effects.Select(x => x.Name))
			}).ToList();

			RunBattle.TurnBeginning(battle, out Creature currentCreature);

			var result = new RunBattleResponse
			{
				Name = battle.Name,
				Description = battle.Description,
				BattleId = battle.Id,
				Creatures = creatures,
				BattleLog = battle.BattleLog,
				CreatureId = currentCreature.Id,
				CurrentCreatureName = currentCreature.Name
			};

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return result;
		}
	}
}
