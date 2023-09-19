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
	public sealed class ChangeItemIsEquippedHandler : BaseHandler<ChangeItemIsEquippedCommand, Unit>
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

			var character = game.Characters.Find(c => c.Id == request.CharacterId)
				?? throw new EntityNotFoundException<Character>(request.CharacterId);

			var item = character.Items.Find(x => x.Id == request.ItemId && x.IsEquipped.HasValue)
				?? throw new EntityNotFoundException<ItemTemplate>(request.ItemId);

			if (item.ItemType == BaseData.Enums.ItemType.Armor)
			{
				await _appDbContext.Entry(character).Collection(c => c.CreatureParts).LoadAsync(cancellationToken);

				Armor armor = item as Armor;
				await _appDbContext.Entry(armor).Collection(a => a.ArmorParts).LoadAsync(cancellationToken);
			}

			character.ChangeItemEquippedStatus(item);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
