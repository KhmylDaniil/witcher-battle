using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Exceptions.RequestExceptions;
using Witcher.Core.ExtensionMethods;

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
			var game = await _authorizationService.CharacterOwnerFilter(_appDbContext.Games, request.CreatureId)
				.GetCreaturesAndCharactersFormBattle(request.BattleId)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var battle = game.Battles.FirstOrDefault()
				?? throw new EntityNotFoundException<Battle>(request.BattleId);

			var healer = battle.Creatures.FirstOrDefault(x => x.Id == request.CreatureId)
				?? throw new EntityNotFoundException<Creature>(request.CreatureId);

			var target = battle.Creatures.FirstOrDefault(x => x.Id == request.TargetId)
				?? throw new EntityNotFoundException<Creature>(request.TargetId);

			var effect = target.Effects.FirstOrDefault(x => x.Id == request.EffectId)
				?? throw new EntityNotFoundException<Effect>(request.EffectId);

			StringBuilder message = new();

			effect.Treat(_rollService, healer, target, ref message);

			battle.BattleLog += message;
			healer.Turn.TurnState = BaseData.Enums.TurnState.BaseActionIsDone;

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
