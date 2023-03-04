using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.RunBattleRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.RunBattleRequests
{
	/// <summary>
	/// Обработчик попытки снятия эффекта
	/// </summary>
	public class TreatEffectHandler : BaseHandler<TreatEffectCommand, TreatEffectResponse>
	{
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
			: base(appDbContext, authorizationService)
		{
			_rollService = rollService;
		}

		/// <summary>
		/// Обработка попытки снятия эффекта
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public override async Task<TreatEffectResponse> Handle(TreatEffectCommand request, CancellationToken cancellationToken)
		{
			var battle = await _authorizationService.BattleMasterFilter(_appDbContext.Battles, request.BattleId)
				.Include(i => i.Creatures.Where(c => c.Id == request.Id))
					.ThenInclude(c => c.CreatureSkills)
				.Include(i => i.Creatures.Where(c => c.Id == request.Id))
					.ThenInclude(c => c.CreatureParts)
				.Include(i => i.Creatures.Where(c => c.Id == request.Id))
					.ThenInclude(c => c.DamageTypeModifiers)
				.Include(i => i.Creatures.Where(c => c.Id == request.Id))
					.ThenInclude(c => c.Effects)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Battle>();

			var creature = battle.Creatures.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<Creature>();

			var effect = creature.Effects.FirstOrDefault(x => x.Id == request.EffectId)
				?? throw new ExceptionEntityNotFound<Effect>();

			StringBuilder message = new();

			effect.Treat(_rollService, creature, ref message);
			
			battle.NextInitiative++;

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return new TreatEffectResponse() { Message = message.ToString() };
		}
	}
}
