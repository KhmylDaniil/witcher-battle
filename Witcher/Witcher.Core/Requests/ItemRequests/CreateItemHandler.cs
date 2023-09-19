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
	public sealed class CreateItemHandler : BaseHandler<CreateItemCommand, Unit>
	{
		public CreateItemHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(CreateItemCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Characters.Where(c => c.Id == request.CharacterId))
					.ThenInclude(c => c.Items)
				.Include(g => g.ItemTemplates.Where(it => it.Id == request.ItemTemplateId))
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var character = game.Characters.Find(c => c.Id == request.CharacterId)
				?? throw new EntityNotFoundException<Character>(request.CharacterId);

			var itemTemplate = game.ItemTemplates.Find(x => x.Id == request.ItemTemplateId)
				?? throw new EntityNotFoundException<ItemTemplate>(request.ItemTemplateId);

			if (itemTemplate.ItemType == BaseData.Enums.ItemType.Armor)
			{
				await _appDbContext.Entry(character).Collection(c => c.CreatureParts).LoadAsync(cancellationToken);

				ArmorTemplate armorTemplate = itemTemplate as ArmorTemplate;
				await _appDbContext.Entry(armorTemplate).Collection(at => at.BodyTemplateParts).LoadAsync(cancellationToken);
			}

			character.AddItems(itemTemplate, request.Quantity);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
