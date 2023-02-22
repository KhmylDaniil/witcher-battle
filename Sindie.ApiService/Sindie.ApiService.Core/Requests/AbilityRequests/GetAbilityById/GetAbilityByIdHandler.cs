using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.AbilityRequests;
using Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbilityById;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplateById;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.AbilityRequests.GetAbilityById
{
	public class GetAbilityByIdHandler : BaseHandler<GetAbilityByIdQuery, GetAbilityByIdResponse>
	{
		public GetAbilityByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService) { }

		public override async Task<GetAbilityByIdResponse> Handle(GetAbilityByIdQuery request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new RequestNullException<GetBodyTemplateByIdQuery>();

			var filter = _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, GameRoles.MasterRoleId)
				.Include(x => x.Abilities.Where(x => x.Id == request.Id))
					.ThenInclude(x => x.AppliedConditions)
				.SelectMany(x => x.Abilities);

			var ability = await filter.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
				?? throw new ExceptionEntityNotFound<Ability>(request.Id);

			return new GetAbilityByIdResponse()
			{
				Id = ability.Id,
				GameId = ability.GameId,
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
				AppliedConditions = ability.AppliedConditions.Select(x => new UpdateAbilityCommandItemAppledCondition()
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
