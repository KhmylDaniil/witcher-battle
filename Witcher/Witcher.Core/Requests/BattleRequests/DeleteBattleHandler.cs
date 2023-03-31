using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Exceptions.RequestExceptions;
using Witcher.Core.Logic;

namespace Witcher.Core.Requests.BattleRequests
{
	public class DeleteBattleHandler : BaseHandler<DeleteBattleCommand, Unit>
	{
		public DeleteBattleHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<Unit> Handle(DeleteBattleCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(x => x.Battles.Where(x => x.Id == request.Id))
				.ThenInclude(b => b.Creatures)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var battle = game.Battles.FirstOrDefault(x => x.Id == request.Id)
				?? throw new EntityNotFoundException<Battle>(request.Id);

			battle.RemoveNonCharacterCreaturesAndUntieCharactersFromBattle();

			game.Battles.Remove(battle);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
