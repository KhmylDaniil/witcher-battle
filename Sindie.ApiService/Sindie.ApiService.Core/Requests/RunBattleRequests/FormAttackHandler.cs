using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.RunBattleRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Requests.RunBattleRequests
{
	public class FormAttackHandler : BaseHandler<FormAttackCommand, FormAttackResponse>
	{
		public FormAttackHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<FormAttackResponse> Handle(FormAttackCommand request, CancellationToken cancellationToken)
		{
			var targetCreature = await _appDbContext.Creatures
				.Include(c => c.CreatureParts)
				.Include(c => c.CreatureSkills)
				.FirstOrDefaultAsync(x => x.Id == request.TargetId, cancellationToken)
					?? throw new ExceptionEntityNotFound<Creature>(request.TargetId);

			var attacker = await _appDbContext.Creatures
					.Include(c => c.Abilities)
					.ThenInclude(a => a.DefensiveSkills)
					.FirstOrDefaultAsync(x => x.Id == request.AttackerId, cancellationToken)
						?? throw new ExceptionEntityNotFound<Creature>(request.AttackerId);

			var ability = request.AbilityId is null
				? attacker.DefaultAbility()
				: attacker.Abilities.FirstOrDefault(a => a.Id == request.AbilityId)
					?? throw new ExceptionEntityNotFound<Ability>(request.AbilityId.Value);

			var defensiveSkills = targetCreature.CreatureSkills.Select(x => x.Skill)
				.Intersect(ability.DefensiveSkills.Select(x => x.Skill));

			var result =  new FormAttackResponse
			{
				CreatureParts = targetCreature.CreatureParts.ToDictionary(x => x.Name, x => (Guid?)x.Id),
				DefensiveSkills = defensiveSkills.ToDictionary(x => Enum.GetName(x), x => (Skill?)x)
			};

			result.DefensiveSkills.Add(string.Empty, null);
			result.CreatureParts.Add(string.Empty, null);

			return result;
		}
	}
}
