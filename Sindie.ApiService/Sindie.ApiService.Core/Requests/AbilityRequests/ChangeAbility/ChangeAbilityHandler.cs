﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.AbilityRequests.ChangeAbility;
using Sindie.ApiService.Core.Contracts.AbilityRequests.CreateAbility;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.AbilityRequests.ChangeAbility
{
	/// <summary>
	/// Обработчик изменения способности
	/// </summary>
	public class ChangeAbilityHandler : BaseHandler, IRequestHandler<ChangeAbilityCommand>
	{
		public ChangeAbilityHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService) { }

		/// <summary>
		/// Обработчик изменения способности
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public async Task<Unit> Handle(ChangeAbilityCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, GameRoles.MasterRoleId)
				.Include(g => g.Abilities)
					.ThenInclude(a => a.AppliedConditions)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			CheckRequest(request, game);

			var ability = game.Abilities.FirstOrDefault(x => x.Id == request.Id);

			ability.ChangeAbility(
				name: request.Name,
				description: request.Description,
				attackDiceQuantity: request.AttackDiceQuantity,
				damageModifier: request.DamageModifier,
				attackSpeed: request.AttackSpeed,
				accuracy: request.Accuracy,
				attackSkill: request.AttackSkill,
				defensiveSkills: request.DefensiveSkills,
				damageType: request.DamageType,
				appliedConditions: request.AppliedConditions);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		private void CheckRequest(ChangeAbilityCommand request, Game game)
		{
			var ability = game.Abilities.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<Ability>(request.Id);

			if (game.Abilities.Any(x => x.Name == request.Name && x.Id != ability.Id))
				throw new ExceptionRequestNameNotUniq<ChangeAbilityCommand>(nameof(request.Name));

			if (!Enum.IsDefined(request.AttackSkill))
				throw new ExceptionRequestFieldIncorrectData<ChangeAbilityCommand>(nameof(request.AttackSkill));

			if (!Enum.IsDefined(request.DamageType))
				throw new ExceptionRequestFieldIncorrectData<ChangeAbilityCommand>(nameof(request.DamageType));

			if (request.DefensiveSkills is not null)
				foreach (var item in request.DefensiveSkills)
					if (!Enum.IsDefined(item))
						throw new ExceptionRequestFieldIncorrectData<CreateAbilityCommand>(nameof(request.DefensiveSkills));

			if (request.AppliedConditions is not null)
				foreach (var appliedCondition in request.AppliedConditions)
				{
					if (appliedCondition.Id != default)
						_ = ability.AppliedConditions.FirstOrDefault(x => x.Id == appliedCondition.Id)
								?? throw new ExceptionEntityNotFound<AppliedCondition>(appliedCondition.Id.Value);

					if (!Enum.IsDefined(appliedCondition.Condition))
						throw new ExceptionRequestFieldIncorrectData<ChangeAbilityCommand>(nameof(appliedCondition.Condition));
				}
		}
	}
}
