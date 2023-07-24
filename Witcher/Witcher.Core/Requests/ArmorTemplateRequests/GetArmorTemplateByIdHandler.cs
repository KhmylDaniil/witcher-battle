using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.ArmorTemplateRequests;
using Witcher.Core.Contracts.BaseRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;

namespace Witcher.Core.Requests.ArmorTemplateRequests
{
	public sealed class GetArmorTemplateByIdHandler : BaseHandler<GetArmorTemplateByIdQuery, GetArmorTemplateByIdResponse>
	{
		public GetArmorTemplateByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<GetArmorTemplateByIdResponse> Handle(GetArmorTemplateByIdQuery request, CancellationToken cancellationToken)
		{
			var filter = _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.BodyTemplates.Where(bt => bt.ArmorsTemplates.Any(at => at.Id == request.Id)))
					.ThenInclude(bt => bt.ArmorsTemplates)
						.ThenInclude(at => at.BodyTemplateParts)
				.SelectMany(g => g.BodyTemplates);

			var bodyTemplate = await filter.FirstOrDefaultAsync(x => x.ArmorsTemplates.Any(at => at.Id == request.Id), cancellationToken)
				?? throw new EntityNotFoundException<BodyTemplate>(request.Id);

			var armorTemplate = bodyTemplate.ArmorsTemplates[0];

			return new GetArmorTemplateByIdResponse()
			{
				Id = armorTemplate.Id,
				Name = armorTemplate.Name,
				Price = armorTemplate.Price,
				Weight = armorTemplate.Weight,
				Description = armorTemplate.Description,
				Armor = armorTemplate.Armor,
				EncumbranceValue = armorTemplate.EncumbranceValue,
				BodyTemplateName = armorTemplate.BodyTemplate.Name,
				BodyTemplatePartsNames = armorTemplate.BodyTemplateParts.Select(x => x.Name).ToList(),
				DamageTypeModifiers = armorTemplate.DamageTypeModifiers.Select(x => new GetResponsePartDamageTypeModifier
				{
					Id = x.Id,
					DamageType = x.DamageType,
					DamageTypeModifier = x.DamageTypeModifier
				}).ToList()
			};
		}
	}
}
