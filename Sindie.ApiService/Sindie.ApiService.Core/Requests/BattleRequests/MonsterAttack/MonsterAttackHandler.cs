using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests.MonsterAttack;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Logic;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BattleRequests.MonsterAttack
{
	/// <summary>
	/// Обработчик атаки монстра
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
		/// Конструктор обработчика атаки монcтра
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
		/// Обработчик атаки монстра
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Результат атаки монстра</returns>
		public async Task<MonsterAttackResponse> Handle(MonsterAttackCommand request, CancellationToken cancellationToken)
		{
			var battle = await _authorizationService.BattleMasterFilter(_appDbContext.Instances, request.BattleId)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.CreatureSkills)
					.ThenInclude(cp => cp.Skill)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.CreatureParts)
					.ThenInclude(cp => cp.BodyPartType)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Abilities)
					.ThenInclude(a => a.AppliedConditions)
					.ThenInclude(ac => ac.Condition)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Abilities)
					.ThenInclude(a => a.DamageTypes)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Abilities)
					.ThenInclude(a => a.DefensiveSkills)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Vulnerables)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Resistances)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Effects)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Battle>();

			var data = CheckAndFormData(request, battle);

			var attack = new Attack(_rollService);
			
			var attackResult = attack.MonsterAttack(
				data: data,
				defenseValue: request.DefenseValue);

			Attack.DisposeCorpses(ref battle);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return new MonsterAttackResponse()
			{ Message = attackResult};
		}

		/// <summary>
		/// Проверка запроса и формирование данных для расчета атаки
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="battle">Бой</param>
		private AttackData CheckAndFormData(MonsterAttackCommand request, Battle battle)
		{
			var monster = battle.Creatures.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<Creature>(request.Id);

			var target = battle.Creatures.FirstOrDefault(x => x.Id == request.TargetCreatureId)
				?? throw new ExceptionEntityNotFound<Creature>(request.TargetCreatureId);

			var aimedPart = request.CreaturePartId == null
				? null
				: target.CreatureParts.FirstOrDefault(x => x.Id == request.CreaturePartId)
					?? throw new ExceptionEntityNotFound<CreaturePart>(request.CreaturePartId.Value);

			if (!monster.Abilities.Any())
				throw new ApplicationException($"У существа с айди {request.Id} отсутствуют способности, атака невозможна.");

			var ability = request.AbilityId == null
				? null
				: monster.Abilities.FirstOrDefault(x => x.Id == request.AbilityId)
					?? throw new ExceptionEntityNotFound<Ability>(request.AbilityId.Value);

			return AttackData.CreateData(monster, target, aimedPart, ability, null, request.SpecialToHit, request.SpecialToDamage);
		}
	}
}
