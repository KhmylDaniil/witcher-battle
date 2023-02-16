using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbility;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.AbilityRequests.GetAbility
{
	public class GetAbilityHandler : BaseHandler<GetAbilityQuery, IEnumerable<Ability>>
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

		public override async Task<IEnumerable<Ability>> Handle(GetAbilityQuery request, CancellationToken cancellationToken)
		{
			if (request.CreationMinTime > _dateTimeProvider.TimeProvider)
				throw new ArgumentOutOfRangeException(nameof(GetAbilityQuery.CreationMinTime));
			if (request.ModificationMinTime > _dateTimeProvider.TimeProvider)
				throw new ArgumentOutOfRangeException(nameof(GetAbilityQuery.ModificationMinTime));

			if (request.CreationMaxTime != default && request.CreationMinTime >= request.CreationMaxTime)
				throw new ArgumentOutOfRangeException(nameof(GetAbilityQuery.CreationMaxTime));
			if (request.ModificationMaxTime != default && request.ModificationMinTime >= request.ModificationMaxTime)
				throw new ArgumentOutOfRangeException(nameof(GetAbilityQuery.ModificationMaxTime));

			var filter = _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(g => g.Abilities)
					.ThenInclude(a => a.AppliedConditions)
					.SelectMany(g => g.Abilities
						.Where(x => x.AttackDiceQuantity <= request.MaxAttackDiceQuantity && x.AttackDiceQuantity >= request.MinAttackDiceQuantity)
						.Where(x => request.UserName == null || x.Game.UserGames
							.Any(u => u.User.Name.Contains(request.UserName) && u.UserId == x.CreatedByUserId))
						.Where(x => request.Name == null || x.Name.Contains(request.Name))
						.Where(x => x.CreatedOn >= request.CreationMinTime)
						.Where(x => (request.CreationMaxTime == default && x.CreatedOn <= _dateTimeProvider.TimeProvider)
						|| x.CreatedOn <= request.CreationMaxTime)
						.Where(x => x.ModifiedOn >= request.ModificationMinTime)
						.Where(x => (request.ModificationMaxTime == default && x.ModifiedOn <= _dateTimeProvider.TimeProvider)
						|| x.ModifiedOn <= request.ModificationMaxTime))
						.Where(x => request.AttackSkillName == null || Enum.GetName(x.AttackSkill).Contains(request.AttackSkillName))
						.Where(x => request.DamageType == null || Enum.GetName(x.DamageType).Contains(request.DamageType))
						.Where(x => request.ConditionName == null
						|| x.AppliedConditions.Select(ac => CritNames.GetConditionFullName(ac.Condition)).Contains(request.ConditionName));

			var list = await filter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.ToListAsync(cancellationToken);

			return list;
		}
	}
}
