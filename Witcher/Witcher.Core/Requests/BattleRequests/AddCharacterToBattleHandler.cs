using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;
using Witcher.Core.Logic;

namespace Witcher.Core.Requests.BattleRequests
{
	public class AddCharacterToBattleHandler : BaseHandler<AddCharacterToBattleCommand, Unit>
	{
		public AddCharacterToBattleHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(AddCharacterToBattleCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Characters)
				.Include(g => g.Battles.Where(b => b.Id == request.BattleId))
					.ThenInclude(b => b.Creatures)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new NoAccessToEntityException<Game>();

			var battle = game.Battles.FirstOrDefault(a => a.Id == request.BattleId)
				?? throw new EntityNotFoundException<Battle>(request.BattleId);

			var character = game.Characters.FirstOrDefault(a => a.Id == request.CharacterId)
				?? throw new EntityNotFoundException<Character>(request.CharacterId);

			character.Turn = new();

			battle.Creatures.Add(character);

			RunBattle.FormInitiativeOrder(battle);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
