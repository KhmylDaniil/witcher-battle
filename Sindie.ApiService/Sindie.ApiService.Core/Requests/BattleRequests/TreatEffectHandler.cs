using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests.TreatEffect;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BattleRequests
{
	/// <summary>
	/// Обработчик попытки снятия эффекта
	/// </summary>
	public class TreatEffectHandler : IRequestHandler<TreatEffectCommand, TreatEffectResponse>
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
		/// Конструктор обработчика попытки снятия эффекта
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		public TreatEffectHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IRollService rollService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
			_rollService = rollService;
		}

		/// <summary>
		/// Обработка попытки снятия эффекта
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public async Task<TreatEffectResponse> Handle(TreatEffectCommand request, CancellationToken cancellationToken)
		{
			var battle = await _authorizationService.BattleMasterFilter(_appDbContext.Battles, request.BattleId)
				.Include(i => i.Creatures.Where(c => c.Id == request.CreatureId))
					.ThenInclude(c => c.CreatureSkills)
					.ThenInclude(cp => cp.Skill)
				.Include(i => i.Creatures.Where(c => c.Id == request.CreatureId))
					.ThenInclude(c => c.CreatureParts)
				.Include(i => i.Creatures.Where(c => c.Id == request.CreatureId))
					.ThenInclude(c => c.Vulnerables)
				.Include(i => i.Creatures.Where(c => c.Id == request.CreatureId))
					.ThenInclude(c => c.Resistances)
				.Include(i => i.Creatures.Where(c => c.Id == request.CreatureId))
					.ThenInclude(c => c.Effects)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Battle>();

			var creature = battle.Creatures.FirstOrDefault(x => x.Id == request.CreatureId)
				?? throw new ExceptionEntityNotFound<Creature>();

			var effect = creature.Effects.FirstOrDefault(x => x.Id == request.EffectId)
				?? throw new ExceptionEntityNotFound<Effect>();

			StringBuilder message = new();

			effect.Treat(_rollService, creature, ref message);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return new TreatEffectResponse() { Message = message.ToString() };
		}
	}
}
