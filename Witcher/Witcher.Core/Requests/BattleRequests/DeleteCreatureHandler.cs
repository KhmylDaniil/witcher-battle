using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.BattleRequests
{
	public sealed class DeleteCreatureHandler : BaseHandler<DeleteCreatureCommand, Unit>
	{
		public DeleteCreatureHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<Unit> Handle(DeleteCreatureCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Battles.Where(b => b.Id == request.BattleId))
					.ThenInclude(b => b.Creatures.Where(c => c.Id == request.Id))
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new NoAccessToEntityException<Game>();

			var battle = game.Battles.Find(a => a.Id == request.BattleId)
				?? throw new EntityNotFoundException<Battle>(request.BattleId);

			var creature = battle.Creatures.Find(a => a.Id == request.Id)
				?? throw new EntityNotFoundException<Creature>(request.Id);

			if (creature is Character character)
				character.Battle = null;
			else
				battle.Creatures.Remove(creature);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
