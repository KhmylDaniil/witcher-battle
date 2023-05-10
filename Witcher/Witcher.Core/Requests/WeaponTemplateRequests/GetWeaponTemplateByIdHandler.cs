using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Contracts.WeaponTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;

namespace Witcher.Core.Requests.WeaponTemplateRequests
{
	public class GetWeaponTemplateByIdHandler : BaseHandler<GetWeaponTemplateByIdQuery, GetWeaponTemplateByIdResponse>
	{
		public GetWeaponTemplateByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<GetWeaponTemplateByIdResponse> Handle(GetWeaponTemplateByIdQuery request, CancellationToken cancellationToken)
		{
			var filter = _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(x => x.ItemTemplates.Where(x => x.Id == request.Id))
				.SelectMany(x => x.ItemTemplates);

			var weaponTemplate = await filter.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) as WeaponTemplate
				?? throw new EntityNotFoundException<Ability>(request.Id);

			return new GetWeaponTemplateByIdResponse()
			{
				Id = weaponTemplate.Id,
				Name = weaponTemplate.Name,
				Price = weaponTemplate.Price,
				Weight = weaponTemplate.Weight,
				Description = weaponTemplate.Description,
				AttackSkill = weaponTemplate.AttackSkill,
				DamageType = weaponTemplate.DamageType,
				AttackDiceQuantity = weaponTemplate.AttackDiceQuantity,
				DamageModifier = weaponTemplate.DamageModifier,
				Durability = weaponTemplate.Durability,
				Range = weaponTemplate.Range,
				Accuracy = weaponTemplate.Accuracy,
				AppliedConditions = weaponTemplate.AppliedConditions.Select(x => new UpdateAttackFormulaCommandItemAppledCondition()
				{
					Id = x.Id,
					ApplyChance = x.ApplyChance,
					Condition = x.Condition
				}).ToList(),
			};
		}
	}
}
