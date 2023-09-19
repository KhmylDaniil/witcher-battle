using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Contracts.BaseRequests;

namespace Witcher.Core.Requests.AbilityRequests
{
	public sealed class GetAbilityByIdHandler : BaseHandler<GetAbilityByIdQuery, GetAbilityByIdResponse>
	{
		public GetAbilityByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService) { }

		public override async Task<GetAbilityByIdResponse> Handle(GetAbilityByIdQuery request, CancellationToken cancellationToken)
		{
			var filter = _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(x => x.Abilities.Where(x => x.Id == request.Id))
				.SelectMany(x => x.Abilities);

			var ability = await filter.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
				?? throw new EntityNotFoundException<Ability>(request.Id);

			return new GetAbilityByIdResponse()
			{
				Id = ability.Id,
				Name = ability.Name,
				Description = ability.Description,
				AttackSkill = ability.AttackSkill,
				DamageType = ability.DamageType,
				AttackDiceQuantity = ability.AttackDiceQuantity,
				DamageModifier = ability.DamageModifier,
				AttackSpeed = ability.AttackSpeed,
				Accuracy = ability.Accuracy,
				CreatedOn = ability.CreatedOn,
				ModifiedOn = ability.ModifiedOn,
				AppliedConditions = ability.AppliedConditions.Select(x => new UpdateAttackFormulaCommandItemAppledCondition()
				{
					Id = x.Id,
					ApplyChance = x.ApplyChance,
					Condition = x.Condition
				}).ToList(),
				DefensiveSkills = ability.DefensiveSkills.Select(x => new GetAbilityByIdResponseItemDefensiveSkill()
				{
					Id = x.Id,
					AbilityId = ability.Id,
					Skill = x.Skill
				}).ToList()
			};
		}
	}
}
