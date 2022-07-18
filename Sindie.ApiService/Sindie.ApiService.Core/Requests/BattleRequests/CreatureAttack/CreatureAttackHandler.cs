using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests.CreatureAttack;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.Logic;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BattleRequests.CreatureAttack
{
	/// <summary>
	/// Обрпботчик атаки существа
	/// </summary>
	public class CreatureAttackHandler : IRequestHandler<CreatureAttackCommand, CreatureAttackResponse>
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
		/// Конструктор обработчика атаки существа
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		public CreatureAttackHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IRollService rollService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
			_rollService = rollService;
		}

		/// <summary>
		/// Обработчик атаки существа
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Результат атаки существа</returns>
		public async Task<CreatureAttackResponse> Handle(CreatureAttackCommand request, CancellationToken cancellationToken)
		{
			var instance = await _authorizationService.InstanceMasterFilter(_appDbContext.Instances, request.InstanceId)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.CreatureParameters)
					.ThenInclude(cp => cp.Parameter)
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
					.ThenInclude(a => a.DefensiveParameters)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Immunities)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Vulnerables)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Resistances)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Conditions)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Instance>();

			var attackData = CheckAndFormData(request, instance);

			var attack = new Attack(_rollService);

			var attackResult = attack.CreatureAttack(ref attackData);

			Attack.DisposeCorpses(ref instance);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return new CreatureAttackResponse { Message = attackResult };
		}

		/// <summary>
		/// Проверка запроса и формирование данных
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="instance">Инстанс</param>
		/// <returns>Данные для расчета атаки</returns>
		private AttackData CheckAndFormData(CreatureAttackCommand request, Instance instance)
		{
			var attacker = instance.Creatures.FirstOrDefault(x => x.Id == request.AttackerId)
				?? throw new ExceptionEntityNotFound<Creature>(request.AttackerId);

			var target = instance.Creatures.FirstOrDefault(x => x.Id == request.TargetCreatureId)
				?? throw new ExceptionEntityNotFound<Creature>(request.TargetCreatureId);

			var aimedPart = request.CreaturePartId == null
				? null
				: target.CreatureParts.FirstOrDefault(x => x.Id == request.CreaturePartId)
					?? throw new ExceptionEntityNotFound<BodyTemplatePart>(request.CreaturePartId.Value);

			if (!attacker.Abilities.Any())
				throw new ApplicationException($"У существа с айди {request.AttackerId} отсутствуют способности, атака невозможна.");

			var ability = request.AbilityId == null
				? null
				: attacker.Abilities.FirstOrDefault(x => x.Id == request.AbilityId)
					?? throw new ExceptionEntityNotFound<Ability>(request.AbilityId.Value);

			if (ability == null && request.DefensiveParameter != null)
				throw new ExceptionRequestFieldIncorrectData<CreatureAttackCommand>(nameof(request.DefensiveParameter), null);

			var defensiveParameter = request.DefensiveParameter == null
				? null
				: target.CreatureParameters.FirstOrDefault(x => x.ParameterId == ability.DefensiveParameters.First(a => a.Id == request.DefensiveParameter).Id)
					?? throw new ExceptionRequestFieldIncorrectData<CreatureAttackCommand>(nameof(request.DefensiveParameter));

			return AttackData.CreateData(attacker, target, aimedPart, ability, defensiveParameter, request.SpecialToHit, request.SpecialToDamage);
		}
	}
}
