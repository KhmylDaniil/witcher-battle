using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbility;
using Sindie.ApiService.Core.ExtensionMethods;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.AbilityRequests.GetAbility
{
	public class GetAbilityHandler : IRequestHandler<GetAbilityCommand, GetAbilityResponse>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Сервис авторизации
		/// </summary>
		private readonly IAuthorizationService _authorizationService;

		/// <summary>
		/// Провайдер времени
		/// </summary>
		private readonly IDateTimeProvider _dateTimeProvider;

		/// <summary>
		/// Конструктор обработчика команды получения списка шаблонов тела
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public GetAbilityHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService,
			IDateTimeProvider dateTimeProvider)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
			_dateTimeProvider = dateTimeProvider;
		}

		public async Task<GetAbilityResponse> Handle(GetAbilityCommand request, CancellationToken cancellationToken)
		{
			if(request.PageSize < 1)
				throw new ArgumentOutOfRangeException(nameof(GetAbilityCommand.PageSize));
			if (request.PageNumber < 1)
				throw new ArgumentOutOfRangeException(nameof(GetAbilityCommand.PageNumber));

			if (request.CreationMinTime > _dateTimeProvider.TimeProvider)
				throw new ArgumentOutOfRangeException(nameof(GetAbilityCommand.CreationMinTime));
			if (request.ModificationMinTime > _dateTimeProvider.TimeProvider)
				throw new ArgumentOutOfRangeException(nameof(GetAbilityCommand.ModificationMinTime));

			if (request.CreationMaxTime != default && request.CreationMinTime >= request.CreationMaxTime)
				throw new ArgumentOutOfRangeException(nameof(GetAbilityCommand.CreationMaxTime));
			if (request.ModificationMaxTime != default && request.ModificationMinTime >= request.ModificationMaxTime)
				throw new ArgumentOutOfRangeException(nameof(GetAbilityCommand.ModificationMaxTime));

			var filter = _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(g => g.Abilities)
					.ThenInclude(a => a.AttackParameter)
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
						.Where(x => request.AttackParameterId == null || x.AttackParameterId == request.AttackParameterId)
						.Where(x => request.DamageTypeId == null || x.DamageTypes.Select(dt => dt.Id).Contains(request.DamageTypeId.Value))
						.Where(x => request.ConditionId == null || x.AppliedConditions.Select(ac => ac.ConditionId).Contains(request.ConditionId.Value));

			var list = await filter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.Select(x => new GetAbilityResponseItem()
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description,
					AttackParameterId = x.AttackParameterId,
					AttackParameterName = x.AttackParameter.Name,
					AttackDiceQuantity = x.AttackDiceQuantity,
					DamageModifier = x.DamageModifier,
					CreatedByUserId = x.CreatedByUserId,
					ModifiedByUserId = x.ModifiedByUserId,
					CreatedOn = x.CreatedOn,
					ModifiedOn = x.ModifiedOn
				}).ToListAsync(cancellationToken);

			return new GetAbilityResponse { AbilitiesList = list, TotalCount = list.Count };
		}
	}
}
