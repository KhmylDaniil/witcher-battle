using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.ArmorTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.ExtensionMethods;

namespace Witcher.Core.Requests.ArmorTemplateRequests
{
	public class GetArmorTemplateHandler : BaseHandler<GetArmorTemplateQuery, IEnumerable<GetArmorTemplateResponse>>
	{
		public GetArmorTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<IEnumerable<GetArmorTemplateResponse>> Handle(GetArmorTemplateQuery request, CancellationToken cancellationToken)
		{
			var filter = _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.ItemTemplates.Where(it => it.ItemType == Enums.ItemType.Armor))
				.Include(x => x.BodyTemplates)
					.ThenInclude(bt => bt.BodyTemplateParts)
				.SelectMany(g => g.ItemTemplates)
				.Select(it => (ArmorTemplate)it)
				.Where(x => x.Armor <= request.MaxArmor && x.Armor >= request.MinArmor)
				.Where(x => request.UserName == null || x.Game.UserGames
					.Any(u => u.User.Name.Contains(request.UserName) && u.UserId == x.CreatedByUserId))
				.Where(x => request.Name == null || x.Name.Contains(request.Name))
				.Where(x => request.BodyPartType == null || x.BodyTemplateParts.Any(x => Enum.GetName(x.BodyPartType).Contains(request.BodyPartType)))
				.Where(x => request.DamageTypeModifier == null || x.DamageTypeModifiers.Any(x => Enum.GetName(x.DamageType).Contains(request.DamageTypeModifier)));

			return await filter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.Select(x => new GetArmorTemplateResponse()
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description,
					Armor = x.Armor,
					EncumbranceValue = x.EncumbranceValue,
					BodyTemplateName = x.BodyTemplate.Name
				}).ToListAsync(cancellationToken);
		}
	}
}
