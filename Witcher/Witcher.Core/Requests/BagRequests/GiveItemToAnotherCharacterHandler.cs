using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BagRequests;
using Witcher.Core.Contracts.Notifications;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.BagRequests
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
				.Include(g => g.Characters)
					.ThenInclude(c => c.Bag)
				.Include(g => g.Characters.Where(c => c.Id == request.CharacterId))
					.ThenInclude(c => c.EquippedWeapons)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var character = game.Characters.FirstOrDefault(c => c.Id == request.CharacterId)
				?? throw new EntityNotFoundException<Character>(request.CharacterId);

			var targetCharacter = game.Characters.FirstOrDefault(c => c.Id == request.TargetCharacterId)
				?? throw new EntityNotFoundException<Character>(request.TargetCharacterId);

			var bag = character.Bag ?? throw new EntityNotIncludedException<Character>(nameof(Bag));

			var item = bag.Items.FirstOrDefault(x => x.Id == request.ItemId)
				?? throw new EntityNotFoundException<ItemTemplate>(request.ItemId);

			return new GiveItemNotification(item, character, targetCharacter);
		}
	}
}
