﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
				.Include(x => x.Parameters)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			var conditions =  await _appDbContext.Conditions.ToListAsync(cancellationToken);
			var damageTypes = await _appDbContext.DamageTypes.ToListAsync(cancellationToken);

			CheckRequest(request, game, conditions, damageTypes);

			var newAbility = Ability.CreateAbility(
				game: game,
				name: request.Name,
				description: request.Description,
				attackDiceQuantity: request.AttackDiceQuantity,
				damageModifier: request.DamageModifier,
				attackSpeed: request.AttackSpeed,
				accuracy: request.Accuracy,
				attackParameter: game.Parameters.First(x => x.Id == request.AttackParameterId),
				defensiveParameters: CreateDefensiveParameters(request, game),
				damageTypes: CreateDamageTypes(request, damageTypes),
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
		private void CheckRequest(CreateAbilityCommand request, Game game, List<Condition> conditions, List<DamageType> damageTypes)
		{
			if (game.Abilities.Any(x => x.Name == request.Name))
				throw new ExceptionRequestNameNotUniq<CreateAbilityCommand>(nameof(request.Name));

			_ = game.Parameters.FirstOrDefault(x => x.Id == request.AttackParameterId)
				?? throw new ExceptionEntityNotFound<Parameter>(request.AttackParameterId);

			foreach (var id in request.DefensiveParameters)
				_ = game.Parameters.FirstOrDefault(x => x.Id == id)
					?? throw new ExceptionEntityNotFound<Parameter>(id);

			foreach (var id in request.DamageTypes)
				_ = damageTypes.FirstOrDefault(x => x.Id == id)
					?? throw new ExceptionEntityNotFound<DamageType>(id);

			foreach (var appliedCondition in request.AppliedConditions)
			{
				_ = conditions.FirstOrDefault(x => x.Id == appliedCondition.ConditionId)
					?? throw new ExceptionEntityNotFound<Condition>(appliedCondition.ConditionId);

				if (appliedCondition.ApplyChance < 0 || appliedCondition.ApplyChance > 100)
					throw new ExceptionRequestFieldIncorrectData<CreateAbilityCommand>(nameof(appliedCondition.ApplyChance));
			}
		}

		/// <summary>
		/// Создание списка защитных параметров
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		/// <returns>Список защитных параметров</returns>
		private List<Parameter> CreateDefensiveParameters(CreateAbilityCommand request, Game game)
		{
			var defensiveParameters = new List<Parameter>();

			foreach (var id in request.DefensiveParameters)
				defensiveParameters.Add(game.Parameters.FirstOrDefault(x => x.Id == id));

			return defensiveParameters;
		}

		/// <summary>
		/// Создание списка типов урона
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="damageTypes">Список типов урона из БД</param>
		/// <returns>Список типов урона</returns>
		private List<DamageType> CreateDamageTypes(CreateAbilityCommand request, List<DamageType> damageTypes)
		{
			var result = new List<DamageType>();

			foreach (var id in request.DamageTypes)
				result.Add(damageTypes.FirstOrDefault(x => x.Id == id));

			return result;
		}
	}
}
