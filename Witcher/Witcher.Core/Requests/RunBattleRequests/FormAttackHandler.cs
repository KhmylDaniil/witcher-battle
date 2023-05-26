using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Requests.RunBattleRequests
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
					?? throw new EntityNotFoundException<Creature>(request.TargetId);

			var attacker = await _appDbContext.Creatures
					.Include(c => c.Abilities)
					.FirstOrDefaultAsync(x => x.Id == request.AttackerId, cancellationToken)
						?? throw new EntityNotFoundException<Creature>(request.AttackerId);

			var baseDefensiveSkills = Drafts.AbilityDrafts.DefensiveSkillsDrafts.BaseDefensiveSkills;

			if (request.AttackType == AttackType.Ability)
			{
				var ability = attacker.Abilities.FirstOrDefault(a => a.Id == request.AttackFormulaId)
					?? throw new EntityNotFoundException<Ability>(request.AttackFormulaId);

				baseDefensiveSkills = ability.DefensiveSkills.Select(x => x.Skill).ToList();
			}

			var defensiveSkills = targetCreature.CreatureSkills.Select(x => x.Skill)
				.Intersect(baseDefensiveSkills);

			var result =  new FormAttackResponse
			{
				CreatureParts = targetCreature.CreatureParts.ToDictionary(x => x.Name, x => (Guid?)x.Id),
				DefensiveSkills = defensiveSkills.ToDictionary(x => Enum.GetName(x), x => (Skill?)x)
			};

			//added default choice
			result.DefensiveSkills.Add(string.Empty, null);
			result.CreatureParts.Add(string.Empty, null);

			return result;
		}
	}
}
