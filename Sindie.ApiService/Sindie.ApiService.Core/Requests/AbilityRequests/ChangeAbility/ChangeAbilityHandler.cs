using MediatR;
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

namespace Sindie.ApiService.Core.Requests.AbilityRequests.ChangeAbility
{
	/// <summary>
	/// Обработчик изменения способности
	/// </summary>
	public class ChangeAbilityHandler : IRequestHandler<ChangeAbilityCommand>
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
		public ChangeAbilityHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

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
				.Include(x => x.Skills)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			var conditions = await _appDbContext.Conditions.ToListAsync(cancellationToken);
			var damageTypes = await _appDbContext.DamageTypes.ToListAsync(cancellationToken);

			CheckRequest(request, game, conditions, damageTypes);

			var ability = game.Abilities.FirstOrDefault(x => x.Id == request.Id);

			ability.ChangeAbility(
				name: request.Name,
				description: request.Description,
				attackDiceQuantity: request.AttackDiceQuantity,
				damageModifier: request.DamageModifier,
				attackSpeed: request.AttackSpeed,
				accuracy: request.Accuracy,
				attackSkill: game.Skills.First(x => x.Id == request.AttackSkillId),
				defensiveSkills: CreateDefensiveSkills(request, game),
				damageTypes: CreateDamageTypes(request, damageTypes),
				appliedConditions: AppliedConditionData.CreateAbilityData(request, conditions));

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
		private void CheckRequest(ChangeAbilityCommand request, Game game, List<Condition> conditions, List<DamageType> damageTypes)
		{
			var ability = game.Abilities.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<Ability>(request.Id);

			if (game.Abilities.Any(x => x.Name == request.Name && x.Id != ability.Id))
				throw new ExceptionRequestNameNotUniq<ChangeAbilityCommand>(nameof(request.Name));

			_ = game.Skills.FirstOrDefault(x => x.Id == request.AttackSkillId)
				?? throw new ExceptionEntityNotFound<Skill>(request.AttackSkillId);

			foreach (var id in request.DefensiveSkills)
				_ = game.Skills.FirstOrDefault(x => x.Id == id)
					?? throw new ExceptionEntityNotFound<Skill>(id);

			foreach (var id in request.DamageTypes)
				_ = damageTypes.FirstOrDefault(x => x.Id == id)
					?? throw new ExceptionEntityNotFound<DamageType>(id);

			foreach (var appliedCondition in request.AppliedConditions)
			{
				if (appliedCondition.Id != default)
					_ = ability.AppliedConditions.FirstOrDefault(x => x.Id == appliedCondition.Id)
							?? throw new ExceptionEntityNotFound<AppliedCondition>(appliedCondition.Id.Value);

				_ = conditions.FirstOrDefault(x => x.Id == appliedCondition.ConditionId)
					?? throw new ExceptionEntityNotFound<Condition>(appliedCondition.ConditionId);

				if (appliedCondition.ApplyChance < 0 || appliedCondition.ApplyChance > 100)
					throw new ExceptionRequestFieldIncorrectData<ChangeAbilityCommand>(nameof(appliedCondition.ApplyChance));
			}
		}

		/// <summary>
		/// Создание списка защитных навыков
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		/// <returns>Список защитных навыков</returns>
		private List<Skill> CreateDefensiveSkills(ChangeAbilityCommand request, Game game)
		{
			var defensiveSkills = new List<Skill>();

			foreach (var id in request.DefensiveSkills)
				defensiveSkills.Add(game.Skills.FirstOrDefault(x => x.Id == id));

			return defensiveSkills;
		}

		/// <summary>
		/// Создание списка типов урона
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="damageTypes">Список типов урона из БД</param>
		/// <returns>Список типов урона</returns>
		private List<DamageType> CreateDamageTypes(ChangeAbilityCommand request, List<DamageType> damageTypes)
		{
			var result = new List<DamageType>();

			foreach (var id in request.DamageTypes)
				result.Add(damageTypes.FirstOrDefault(x => x.Id == id));

			return result;
		}
	}
}
