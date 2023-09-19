using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.ArmorTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.ArmorTemplateRequests
{
	public sealed class CreateArmorTemplateHandler : BaseHandler<CreateArmorTemplateCommand, Guid>
	{
		public CreateArmorTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<Guid> Handle(CreateArmorTemplateCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.ItemTemplates)
				.Include(g => g.BodyTemplates.Where(bt => bt.Id == request.BodyTemplateId))
				.ThenInclude(bt => bt.BodyTemplateParts)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			if (game.ItemTemplates.Exists(x => x.Name == request.Name))
				throw new RequestNameNotUniqException<ItemTemplate>(request.Name);

			var bodyTemplate = game.BodyTemplates.Find(bt => bt.Id == request.BodyTemplateId)
				?? throw new EntityNotFoundException<BodyTemplate>(request.BodyTemplateId);

			var newTemplate = ArmorTemplate.CreateArmorTemplate(
				game: game,
				bodyTemplate: bodyTemplate,
				name: request.Name,
				description: request.Description,
				weight: request.Weight,
				price: request.Price,
				armor: request.Armor,
				encumbranceValue: request.EncumbranceValue,
				bodyPartTypes: request.BodyPartTypes);

			_appDbContext.ItemTemplates.Add(newTemplate);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return newTemplate.Id;
		}
	}
}
