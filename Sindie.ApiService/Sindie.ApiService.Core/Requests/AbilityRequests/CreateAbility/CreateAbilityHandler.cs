using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.AbilityRequests.CreateAbility;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.AbilityRequests.CreateAbility
{
	/// <summary>
	/// Обработчик создания способности
	/// </summary>
	public class CreateAbilityHandler : BaseHandler, IRequestHandler<CreateAbilityCommand, Ability>
	{
		public CreateAbilityHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService) { }

		/// <summary>
		/// Обработчик создания способности
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Способность</returns>
		public async Task<Ability> Handle(CreateAbilityCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, GameRoles.MasterRoleId)
				.Include(g => g.Abilities)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			CheckRequest(request, game);

			var newAbility = Ability.CreateAbility(
				game: game,
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

			_appDbContext.Abilities.Add(newAbility);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return newAbility;
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		private static void CheckRequest(CreateAbilityCommand request, Game game)
		{
			if (game.Abilities.Any(x => x.Name == request.Name))
				throw new ExceptionRequestNameNotUniq<CreateAbilityCommand>(nameof(request.Name));

			if (!Enum.IsDefined(request.AttackSkill))
				throw new ExceptionRequestFieldIncorrectData<CreateAbilityCommand>(nameof(request.AttackSkill));

			if (!Enum.IsDefined(request.DamageType))
				throw new ExceptionRequestFieldIncorrectData<CreateAbilityCommand>(nameof(request.DamageType));

			if (request.DefensiveSkills is not null)
				foreach (var item in request.DefensiveSkills)
					if (!Enum.IsDefined(item))
						throw new ExceptionRequestFieldIncorrectData<CreateAbilityCommand>(nameof(request.DefensiveSkills));

			if (request.AppliedConditions is not null)
				foreach (var appliedCondition in request.AppliedConditions.Select(x => x.Condition))
					if (!Enum.IsDefined(appliedCondition))
						throw new ExceptionRequestFieldIncorrectData<CreateAbilityCommand>(nameof(appliedCondition));
		}
	}
}
