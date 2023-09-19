using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.WeaponTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.ExtensionMethods;

namespace Witcher.Core.Requests.WeaponTemplateRequests
{
	public sealed class GetWeaponTemplateHandler : BaseHandler<GetWeaponTemplateQuery, IEnumerable<GetWeaponTemplateResponse>>
	{
		public GetWeaponTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<IEnumerable<GetWeaponTemplateResponse>> Handle(GetWeaponTemplateQuery request, CancellationToken cancellationToken)
		{
			var attackSkillMatches = request.AttackSkillName.GetPossibleEnumNumbersFromSearchString<Enums.Skill>();

			var damageTypeMatches = request.DamageType.GetPossibleEnumNumbersFromSearchString<Enums.DamageType>();

			var conditionMatches = request.ConditionName.GetPossibleEnumNumbersFromSearchString<Condition>();

			var filter =  _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.ItemTemplates.Where(it => it.ItemType == Enums.ItemType.Weapon))
				.SelectMany(g => g.ItemTemplates)
				.Select(it => (WeaponTemplate)it)
				.Where(x => x.AttackDiceQuantity <= request.MaxAttackDiceQuantity && x.AttackDiceQuantity >= request.MinAttackDiceQuantity)
				.Where(x => request.UserName == null || x.Game.UserGames
					.Any(u => u.User.Name.Contains(request.UserName) && u.UserId == x.CreatedByUserId))
				.Where(x => request.Name == null || x.Name.Contains(request.Name))
				.Where(x => request.AttackSkillName == null || attackSkillMatches.Any(s => (int)x.AttackSkill == s))
						.Where(x => request.DamageType == null || damageTypeMatches.Any(dt => (int)x.DamageType == dt))
						.Where(x => request.ConditionName == null
						|| x.AppliedConditions.Any(ac => conditionMatches.Any(cm => cm == (int)ac.Condition)));

			return await filter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.Select(x => new GetWeaponTemplateResponse()
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description,
					AttackDiceQuantity = x.AttackDiceQuantity,
					DamageModifier = x.DamageModifier,
					Accuracy = x.Accuracy,
					AttackSkill = x.AttackSkill,
					DamageType = x.DamageType
				}).ToListAsync(cancellationToken);
		}
	}
}
