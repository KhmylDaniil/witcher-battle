using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Requests.RunBattleRequests
{
	public class PassInMultiAttackHandler : BaseHandler<PassInMultiattackCommand, Unit>
	{
		public PassInMultiAttackHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(PassInMultiattackCommand request, CancellationToken cancellationToken)
		{
			var battle = await _authorizationService.BattleMasterFilter(_appDbContext.Battles, request.BattleId)
				.Include(i => i.Creatures)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Battle>();

			var currentCreature = battle.Creatures.FirstOrDefault(x => x.Id == request.CreatureId)
				?? throw new EntityNotFoundException<Creature>(request.CreatureId);

			currentCreature.Turn.TurnState = currentCreature.Turn.TurnState switch
			{
				TurnState.InProcessOfAdditionAction => TurnState.TurnIsDone,
				TurnState.InProcessOfBaseAction => TurnState.BaseActionIsDone,
				_ => throw new LogicBaseException("Цель не находится в процессе мультиатаки")
			};

			battle.BattleLog += $"{currentCreature.Name} пасует в процессе мультиатаки.";

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
