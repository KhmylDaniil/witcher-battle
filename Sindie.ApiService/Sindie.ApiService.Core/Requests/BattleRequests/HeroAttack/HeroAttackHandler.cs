using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests.HeroAttack;
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

namespace Sindie.ApiService.Core.Requests.BattleRequests.HeroAttack
{
	/// <summary>
	/// Обрпботчик атаки героя
	/// </summary>
	public class HeroAttackHandler : IRequestHandler<HeroAttackCommand, HeroAttackResponse>
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
		/// Конструктор обработчика атаки героя
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		public HeroAttackHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IRollService rollService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
			_rollService = rollService;
		}

		/// <summary>
		/// Обработчик атаки героя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Результат атаки героя</returns>
		public async Task<HeroAttackResponse> Handle(HeroAttackCommand request, CancellationToken cancellationToken)
		{
			var instance = await _authorizationService.InstanceMasterFilter(_appDbContext.Instances, request.InstanceId)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.CreatureParameters)
					.ThenInclude(cp => cp.Parameter)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.BodyTemplate)
					.ThenInclude(bp => bp.BodyTemplateParts)
					.ThenInclude(btp => btp.BodyPartType)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Abilities)
					.ThenInclude(a => a.AppliedConditions)
					.ThenInclude(ac => ac.Condition)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Instance>();

			CheckRequest(request, instance);

			var hero = instance.Creatures.FirstOrDefault(x => x.Id == request.Id);

			var target = instance.Creatures.FirstOrDefault(x => x.Id == request.TargetCreatureId);

			var aimedPart = request.BodyTemplatePartId == null
				? null
				: target.BodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Id == request.BodyTemplatePartId);

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
		private void CheckRequest(HeroAttackCommand request, Instance instance)
		{
			var hero = instance.Creatures.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<Creature>(request.Id);

			var target = instance.Creatures.FirstOrDefault(x => x.Id == request.TargetCreatureId)
				?? throw new ExceptionEntityNotFound<Creature>(request.TargetCreatureId);

			if (request.BodyTemplatePartId != null)
				_ = target.BodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Id == request.BodyTemplatePartId)
					?? throw new ExceptionEntityNotFound<BodyTemplatePart>(request.BodyTemplatePartId.Value);

			if (!hero.Abilities.Any())
				throw new ApplicationException($"У существа с айди {request.Id} отсутствуют способности, атака невозможна.");

			if (request.AbilityId != null)
				_ = hero.Abilities.FirstOrDefault(x => x.Id == request.AbilityId)
					?? throw new ExceptionEntityNotFound<Ability>(request.AbilityId.Value);
		}
	}
}
