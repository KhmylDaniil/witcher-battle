using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.AbilityRequests
{
	public sealed class GetAbilityHandler : BaseHandler<GetAbilityQuery, IEnumerable<GetAbilityResponseItem>>
	{
		/// <summary>
		/// Провайдер времени
		/// </summary>
		private readonly IDateTimeProvider _dateTimeProvider;

		public GetAbilityHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IDateTimeProvider dateTimeProvider)
			: base(appDbContext, authorizationService)
		{
			_dateTimeProvider = dateTimeProvider;
		}

		public override async Task<IEnumerable<GetAbilityResponseItem>> Handle(GetAbilityQuery request, CancellationToken cancellationToken)
		{
			var attackSkillMatches = request.AttackSkillName.GetPossibleEnumNumbersFromSearchString<Enums.Skill>();

			var damageTypeMatches = request.DamageType.GetPossibleEnumNumbersFromSearchString<Enums.DamageType>();

			var conditionMatches = request.ConditionName.GetPossibleEnumNumbersFromSearchString<Condition>();

			var filter = _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Abilities)
					.SelectMany(g => g.Abilities
						.Where(x => x.AttackDiceQuantity <= request.MaxAttackDiceQuantity && x.AttackDiceQuantity >= request.MinAttackDiceQuantity)
						.Where(x => request.UserName == null || x.Game.UserGames
							.Any(u => u.User.Name.Contains(request.UserName) && u.UserId == x.CreatedByUserId))
						.Where(x => request.Name == null || x.Name.Contains(request.Name))
						.Where(x => x.CreatedOn >= request.CreationMinTime)
						.Where(x => request.CreationMaxTime == default && x.CreatedOn <= _dateTimeProvider.TimeProvider
						|| x.CreatedOn <= request.CreationMaxTime)
						.Where(x => x.ModifiedOn >= request.ModificationMinTime)
						.Where(x => request.ModificationMaxTime == default && x.ModifiedOn <= _dateTimeProvider.TimeProvider
						|| x.ModifiedOn <= request.ModificationMaxTime))
						.Where(x => request.AttackSkillName == null || attackSkillMatches.Any(s => (int)x.AttackSkill == s))
						.Where(x => request.DamageType == null || damageTypeMatches.Any(dt => (int)x.DamageType == dt))
						.Where(x => request.ConditionName == null
						|| x.AppliedConditions.Any(ac => conditionMatches.Any(cm => cm == (int)ac.Condition)));

			var list = await filter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.Select(x => new GetAbilityResponseItem()
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description,
					AttackDiceQuantity = x.AttackDiceQuantity,
					DamageModifier = x.DamageModifier,
					AttackSpeed = x.AttackSpeed,
					Accuracy = x.Accuracy,
					AttackSkill = x.AttackSkill,
					DamageType = x.DamageType
				})
				.ToListAsync(cancellationToken);

			return list;
		}
	}
}
