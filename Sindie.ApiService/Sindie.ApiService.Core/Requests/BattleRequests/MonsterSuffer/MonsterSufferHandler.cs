using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests.MonsterSuffer;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Logic;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BattleRequests.MonsterSuffer
{
	/// <summary>
	/// Обработчик получения монстром урона
	/// </summary>
	public class MonsterSufferHandler : IRequestHandler<MonsterSufferCommand, MonsterSufferResponse>
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
		/// Конструктор обработчика получения монстром урона
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		public MonsterSufferHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IRollService rollService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
			_rollService = rollService;
		}

		/// <summary>
		/// Обработчик получения монстром уррона
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public async Task<MonsterSufferResponse> Handle(MonsterSufferCommand request, CancellationToken cancellationToken)
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
					.ThenInclude(c => c.Conditions)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Battle>();

			var data = CheckAndFormData(request, battle);

			var attack = new Attack(_rollService);

			var attackResult = attack.MonsterSuffer(
				data: ref data,
				damage: request.DamageValue,
				successValue: request.SuccessValue);

			Attack.DisposeCorpses(ref battle);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return new MonsterSufferResponse()
			{ Message = attackResult };
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="battle">Бой</param>
		private AttackData CheckAndFormData(MonsterSufferCommand request, Battle battle)
		{
			var attacker = battle.Creatures.FirstOrDefault(x => x.Id == request.AttackerId)
				?? throw new ExceptionEntityNotFound<Creature>(request.AttackerId);

			var target = battle.Creatures.FirstOrDefault(x => x.Id == request.TargetId)
				?? throw new ExceptionEntityNotFound<Creature>(request.TargetId);

			var aimedPart = request.CreaturePartId == null
				? null
				: target.CreatureParts.FirstOrDefault(x => x.Id == request.CreaturePartId)
					?? throw new ExceptionEntityNotFound<CreaturePart>(request.CreaturePartId.Value);

			if (!attacker.Abilities.Any())
				throw new ApplicationException($"У существа с айди {request.AttackerId} отсутствуют способности, атака невозможна.");

			var ability = attacker.Abilities.FirstOrDefault(x => x.Id == request.AbilityId)
					?? throw new ExceptionEntityNotFound<Ability>(request.AbilityId);

			return AttackData.CreateData(attacker, target, aimedPart, ability, null, 0, 0);
		}
	}
}
