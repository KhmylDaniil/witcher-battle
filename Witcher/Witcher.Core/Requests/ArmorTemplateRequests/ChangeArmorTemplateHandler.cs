using MediatR;
using Microsoft.EntityFrameworkCore;
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
	public sealed class ChangeArmorTemplateHandler : BaseHandler<ChangeArmorTemplateCommand, Unit>
	{
		public ChangeArmorTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<Unit> Handle(ChangeArmorTemplateCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.BodyTemplates.Where(bt => bt.ArmorsTemplates.Any(at => at.Id == request.Id)))
					.ThenInclude(at => at.BodyTemplateParts)
				.Include(g => g.BodyTemplates.Where(bt => bt.ArmorsTemplates.Any(at => at.Id == request.Id)))
					.ThenInclude(bt => bt.ArmorsTemplates)
						.ThenInclude(at => at.BodyTemplateParts)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			if (game.ItemTemplates.Exists(x => x.Name == request.Name && x.Id != request.Id))
				throw new RequestNameNotUniqException<ItemTemplate>(request.Name);

			var bodyTemplate = game.BodyTemplates.Find(x => x.ArmorsTemplates.Exists(x => x.Id == request.Id))
				?? throw new EntityNotFoundException<ArmorTemplate>(request.Id);

			var armorTemplate = bodyTemplate.ArmorsTemplates[0];

			armorTemplate.ChangeArmorTemplate(
				name: request.Name,
				description: request.Description,
				weight: request.Weight,
				price: request.Price,
				armor: request.Armor,
				encumbranceValue: request.EncumbranceValue,
				bodyTemplate: bodyTemplate,
				bodyPartTypes: request.BodyPartTypes);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
