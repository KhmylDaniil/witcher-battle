using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.RunBattleRequests
{
	/// <summary>
	/// Обработчик попытки снятия эффекта
	/// </summary>
	public class HealEffectHandler : BaseHandler<HealEffectCommand, Unit>
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
		public HealEffectHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IRollService rollService)
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
		public override async Task<Unit> Handle(HealEffectCommand request, CancellationToken cancellationToken)
		{
			var battle = await _authorizationService.BattleMasterFilter(_appDbContext.Battles, request.BattleId)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.CreatureSkills)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.CreatureParts)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.DamageTypeModifiers)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Effects)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Battle>();

			var healer = battle.Creatures.FirstOrDefault(x => x.Id == request.CreatureId)
				?? throw new EntityNotFoundException<Creature>(request.CreatureId);

			var target = battle.Creatures.FirstOrDefault(x => x.Id == request.TargetCreatureId)
				?? throw new EntityNotFoundException<Creature>(request.TargetCreatureId);

			var effect = target.Effects.FirstOrDefault(x => x.Id == request.EffectId)
				?? throw new EntityNotFoundException<Effect>(request.EffectId);

			StringBuilder message = new();

			effect.Treat(_rollService, healer, target, ref message);
			
			battle.NextInitiative++;

			battle.BattleLog += message;

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
