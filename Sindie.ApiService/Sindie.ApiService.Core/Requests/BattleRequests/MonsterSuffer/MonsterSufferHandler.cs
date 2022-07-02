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
			var instance = await _authorizationService.InstanceMasterFilter(_appDbContext.Instances, request.InstanceId)
				.Include(i => i.Creatures.Where(c => c.Id == request.MonsterId))
					.ThenInclude(c => c.CreatureParameters)
					.ThenInclude(cp => cp.Parameter)
				.Include(i => i.Creatures.Where(c => c.Id == request.MonsterId))
					.ThenInclude(c => c.CreatureParts)
					.ThenInclude(btp => btp.BodyPartType)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Instance>();

			CheckRequest(request, instance);

			var monster = instance.Creatures.FirstOrDefault(x => x.Id == request.MonsterId);

			var aimedPart = request.CreaturePartId == null
				? null
				: monster.CreatureParts.FirstOrDefault(x => x.Id == request.CreaturePartId);

			var attack = new Attack(_rollService);

			var attackResult = attack.MonsterSuffer(
				monster: ref monster,
				aimedPart: aimedPart,
				damageValue: request.DamageValue,
				successValue: request.SuccessValue,
				isResistant: request.IsResistant,
				isVulnerable: request.IsVulnerable);

			if (monster.HP == 0)
				instance.Creatures.Remove(monster);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return new MonsterSufferResponse()
			{ Message = attackResult };
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="instance">Инстанс</param>
		private void CheckRequest(MonsterSufferCommand request, Instance instance)
		{
			var monster = instance.Creatures.FirstOrDefault(x => x.Id == request.MonsterId)
				?? throw new ExceptionEntityNotFound<Creature>(request.MonsterId);

			if (request.CreaturePartId != null)
				_ = monster.CreatureParts.FirstOrDefault(x => x.Id == request.CreaturePartId)
					?? throw new ExceptionEntityNotFound<BodyTemplatePart>(request.CreaturePartId.Value);

			if (request.IsResistant && request.IsVulnerable)
				throw new ApplicationException("Одновременные уязвимость и сопротивление урону невозможны");
		}
	}
}
