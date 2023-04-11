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
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.BagRequests
{
	public class AddItemToBagHandler : BaseHandler<AddItemToBagCommand, Unit>
	{
		public AddItemToBagHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(AddItemToBagCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Characters.Where(c => c.Id == request.CharacterId))
					.ThenInclude(c => c.Bag)
				.Include(g => g.ItemTemplates.Where(it => it.Id == request.ItemTemplateId))
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var character = game.Characters.FirstOrDefault(c => c.Id == request.CharacterId)
				?? throw new EntityNotFoundException<Character>(request.CharacterId);

			var bag = character.Bag ?? throw new EntityNotIncludedException<Character>(nameof(Bag));

			var itemTemplate = game.ItemTemplates.FirstOrDefault(x => x.Id == request.ItemTemplateId)
				?? throw new EntityNotFoundException<ItemTemplate>(request.ItemTemplateId);

			var item = Item.CreateItem(bag, itemTemplate, request.Quantity);

			bag.Items.Add(item);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
