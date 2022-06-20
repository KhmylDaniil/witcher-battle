using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests.MonsterAttack;
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

namespace Sindie.ApiService.Core.Requests.BattleRequests.MonsterAttack
{
	/// <summary>
	/// Обработчикк атаки монстра
	/// </summary>
	public class MonsterAttackHandler : IRequestHandler<MonsterAttackCommand, MonsterAttackResponse>
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
		/// Бросок параметра
		/// </summary>
		private readonly IRollService _rollService;

		/// <summary>
		/// Конструктор обработчика создания шаблона существа
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		public MonsterAttackHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IRollService rollService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
			_rollService = rollService;
		}

		/// <summary>
		/// Конструктор обработчика атаки монстра
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Результат атаки монстра</returns>
		public async Task<MonsterAttackResponse> Handle(MonsterAttackCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.InstanceMasterFilter(_appDbContext.Games, request.InstanceId)
				.Include(g => g.BodyTemplates.Where(bt => bt.Id == request.TargetBodyTemplateId))
					.ThenInclude(bt => bt.BodyTemplateParts)
					.ThenInclude(btp => btp.BodyPartType)
				.Include(g => g.Instances.Where(i => i.Id == request.InstanceId))
					.ThenInclude(i => i.Creatures.Where(c => c.Id == request.Id))
						.ThenInclude(c => c.CreatureParameters)
						.ThenInclude(cp => cp.Parameter)
				.Include(g => g.Instances.Where(i => i.Id == request.InstanceId))
					.ThenInclude(i => i.Creatures.Where(c => c.Id == request.Id))
						.ThenInclude(c => c.Abilities)
						.ThenInclude(a => a.AppliedConditions)
						.ThenInclude(ac => ac.Condition)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			CheckRequest(request, game);

			var monster = game.Instances.Where(x => x.Id == request.InstanceId)
				.SelectMany(x => x.Creatures)
				.FirstOrDefault(x => x.Id == request.Id);

			var bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.Id == request.TargetBodyTemplateId);

			var bodyTemplatePart = request.BodyTemplatePartId == null
				? bodyTemplate.DefaultBodyTemplatePart()
				: bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Id == request.BodyTemplatePartId);

			var hitPenalty = request.BodyTemplatePartId == null
				? 0
				: bodyTemplatePart.HitPenalty;

			var ability = request.AbilityId == null
				? monster.DefaultAbility()
				: monster.Abilities.FirstOrDefault(x => x.Id == request.AbilityId);

			var successValue = _rollService.RollAttack(
				attackValue: monster.CreatureParameters
				.FirstOrDefault(x => x.ParameterId == ability.AttackParameterId).ParameterValue + ability.Accuracy - hitPenalty,
				defenseValue: request.DefenseValue);

			var attackResult = monster.MonsterAttack(ability, bodyTemplatePart, successValue);

			return new MonsterAttackResponse()
			{ Message = attackResult};
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		private void CheckRequest(MonsterAttackCommand request, Game game)
		{
			var instance = game.Instances.FirstOrDefault(x => x.Id == request.InstanceId)
				?? throw new ExceptionEntityNotFound<Instance>(request.InstanceId);

			var monster = instance.Creatures.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<Creature>(request.Id);

			var bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.Id == request.TargetBodyTemplateId)
				?? throw new ExceptionEntityNotFound<BodyTemplate>(request.TargetBodyTemplateId);

			if (request.BodyTemplatePartId != null)
				_ = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Id == request.BodyTemplatePartId)
					?? throw new ExceptionEntityNotFound<BodyTemplatePart>(request.BodyTemplatePartId.Value);

			if (!monster.Abilities.Any())
				throw new ApplicationException($"У существа с айди {request.Id} отсутствуют способности, атака невозможна.");

			if (request.AbilityId != null)
				_ = monster.Abilities.FirstOrDefault(x => x.Id == request.AbilityId)
					?? throw new ExceptionEntityNotFound<Ability>(request.AbilityId.Value);
		}
	}
}
