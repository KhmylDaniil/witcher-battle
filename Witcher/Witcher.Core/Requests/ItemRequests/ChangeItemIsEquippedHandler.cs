using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.ItemRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.ItemRequests
{
	public class ChangeItemIsEquippedHandler : BaseHandler<ChangeItemIsEquippedCommand, Unit>
	{
		public ChangeItemIsEquippedHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(ChangeItemIsEquippedCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.CharacterOwnerFilter(_appDbContext.Games, request.CharacterId)
				.Include(g => g.Characters.Where(c => c.Id == request.CharacterId))
					.ThenInclude(c => c.Items.Where(i => i.Id == request.ItemId))
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var character = game.Characters.FirstOrDefault(c => c.Id == request.CharacterId)
				?? throw new EntityNotFoundException<Character>(request.CharacterId);

			var item = character.Items.FirstOrDefault(x => x.Id == request.ItemId && x.IsEquipped.HasValue)
				?? throw new EntityNotFoundException<ItemTemplate>(request.ItemId);

			character.ChangeItemIsEquipped(item);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
