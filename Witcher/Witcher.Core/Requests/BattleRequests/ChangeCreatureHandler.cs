using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;
using Witcher.Core.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.BattleRequests
{
	public class ChangeCreatureHandler : BaseHandler<ChangeCreatureCommand, Unit>
	{
		public ChangeCreatureHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<Unit> Handle(ChangeCreatureCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Battles.Where(b => b.Id == request.BattleId))
					.ThenInclude(b => b.Creatures.Where(c => c.Id == request.Id))
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionNoAccessToEntity<Game>();

			var battle = game.Battles.FirstOrDefault(a => a.Id == request.BattleId)
				?? throw new ExceptionEntityNotFound<Battle>(request.BattleId);

			if (battle.Creatures.Any(c => c.Name == request.Name && c.Id != request.Id))
				throw new RequestNameNotUniqException<Creature>(request.Name);

			var creature = battle.Creatures.FirstOrDefault(a => a.Id == request.Id)
				?? throw new ExceptionEntityNotFound<Creature>(request.Id);

			creature.ChangeCreature(request.Name, request.Description);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
