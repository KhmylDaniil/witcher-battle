using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.Requests.AbilityRequests.ChangeAbility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Requests.AbilityRequests.CreateAbility
{
	/// <summary>
	/// Обработчик создания способности
	/// </summary>
	public class CreateAbilityHandler : IRequestHandler<CreateAbilityCommand>
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
		/// Конструктор обработчика создания способности
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		public CreateAbilityHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Обработчик создания способности
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Способность</returns>
		public async Task<Unit> Handle(CreateAbilityCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, GameRoles.MasterRoleId)
				.Include(g => g.Abilities)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			var conditions =  await _appDbContext.Conditions.ToListAsync(cancellationToken);

			CheckRequest(request, game, conditions);

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
				appliedConditions: AppliedConditionData.CreateAbilityData(request, conditions));

			_appDbContext.Abilities.Add(newAbility);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		/// <param name="conditions">Состояния</param>
		/// <param name="damageTypes">Типы урона</param>
		private void CheckRequest(CreateAbilityCommand request, Game game, List<Condition> conditions)
		{
			if (game.Abilities.Any(x => x.Name == request.Name))
				throw new ExceptionRequestNameNotUniq<CreateAbilityCommand>(nameof(request.Name));

			if (!Enum.IsDefined(request.AttackSkill))
				throw new ExceptionRequestFieldIncorrectData<CreateAbilityCommand>(nameof(request.AttackSkill));

			if (!Enum.IsDefined(request.DamageType))
				throw new ExceptionRequestFieldIncorrectData<CreateAbilityCommand>(nameof(request.DamageType));

			foreach (var item in request.DefensiveSkills)
				if (!Enum.IsDefined(item))
					throw new ExceptionRequestFieldIncorrectData<CreateAbilityCommand>(nameof(request.DefensiveSkills));

			foreach (var appliedCondition in request.AppliedConditions)
			{
				_ = conditions.FirstOrDefault(x => x.Id == appliedCondition.ConditionId)
					?? throw new ExceptionEntityNotFound<Condition>(appliedCondition.ConditionId);

				if (appliedCondition.ApplyChance < 0 || appliedCondition.ApplyChance > 100)
					throw new ExceptionRequestFieldIncorrectData<CreateAbilityCommand>(nameof(appliedCondition.ApplyChance));
			}
		}
	}
}
