using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.ItemRequests;
using Witcher.Core.Contracts.Notifications;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.ItemRequests
{
	public class GiveItemToAnotherCharacterHandler : BaseHandler<GiveItemToAnotherCharacterCommand, GiveItemNotification>
	{
		public GiveItemToAnotherCharacterHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService)
		{
		}

		public async override Task<GiveItemNotification> Handle(GiveItemToAnotherCharacterCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.CharacterOwnerFilter(_appDbContext.Games, request.CharacterId)
				.Include(g => g.Characters.Where(c => c.Id == request.CharacterId || c.Id == request.TargetCharacterId))
					.ThenInclude(c => c.Items)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var character = game.Characters.FirstOrDefault(c => c.Id == request.CharacterId)
				?? throw new EntityNotFoundException<Character>(request.CharacterId);

			var targetCharacter = game.Characters.FirstOrDefault(c => c.Id == request.TargetCharacterId)
				?? throw new EntityNotFoundException<Character>(request.TargetCharacterId);

			var item = character.Items.FirstOrDefault(x => x.Id == request.ItemId)
				?? throw new EntityNotFoundException<ItemTemplate>(request.ItemId);

			return new GiveItemNotification(item, character, targetCharacter);
		}
	}
}
