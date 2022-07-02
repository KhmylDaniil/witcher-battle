using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests.CreatureAttack;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Instance>();

			CheckRequest(request, instance);

			var hero = instance.Creatures.FirstOrDefault(x => x.Id == request.AttackerId);

			var target = instance.Creatures.FirstOrDefault(x => x.Id == request.TargetCreatureId);

			var aimedPart = request.CreaturePartId == null
				? null
				: target.CreatureParts.FirstOrDefault(x => x.Id == request.CreaturePartId);

			var ability = request.AbilityId == null
				? null
				: hero.Abilities.FirstOrDefault(x => x.Id == request.AbilityId);

			var attack = new Attack(_rollService);

			//var attackResult = attack.HeroAttack(
			//	hero: hero,
			//	target: target,
			//	attackValue: request.AttackValue,
			//	damageValue: request.DamageValue.Value,
			//	aimedPart: aimedPart,
			//	ability: ability,
			//	specialToHit: request.SpecialToHit.Value,
			//	specialToDamage: request.SpecialToDamage.Value);



			throw new NotImplementedException();
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="instance">Инстанс</param>
		private void CheckRequest(CreatureAttackCommand request, Instance instance)
		{
			var hero = instance.Creatures.FirstOrDefault(x => x.Id == request.AttackerId)
				?? throw new ExceptionEntityNotFound<Creature>(request.AttackerId);

			var target = instance.Creatures.FirstOrDefault(x => x.Id == request.TargetCreatureId)
				?? throw new ExceptionEntityNotFound<Creature>(request.TargetCreatureId);

			if (request.CreaturePartId != null)
				_ = target.CreatureParts.FirstOrDefault(x => x.Id == request.CreaturePartId)
					?? throw new ExceptionEntityNotFound<BodyTemplatePart>(request.CreaturePartId.Value);

			if (!hero.Abilities.Any())
				throw new ApplicationException($"У существа с айди {request.AttackerId} отсутствуют способности, атака невозможна.");

			if (request.AbilityId != null)
				_ = hero.Abilities.FirstOrDefault(x => x.Id == request.AbilityId)
					?? throw new ExceptionEntityNotFound<Ability>(request.AbilityId.Value);
		}
	}
}
