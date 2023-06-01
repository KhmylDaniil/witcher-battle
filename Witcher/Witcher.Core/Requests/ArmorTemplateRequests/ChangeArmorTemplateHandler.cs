using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.ArmorTemplateRequests;
using Witcher.Core.Contracts.WeaponTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.ArmorTemplateRequests
{
	public class ChangeArmorTemplateHandler : BaseHandler<ChangeArmorTemplateCommand, Unit>
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

			if (game.ItemTemplates.Any(x => x.Name == request.Name && x.Id != request.Id))
				throw new RequestNameNotUniqException<ChangeArmorTemplateCommand>(nameof(request.Name));

			var bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.ArmorsTemplates.Any(x => x.Id == request.Id))
				?? throw new EntityNotFoundException<ArmorTemplate>(request.Id);

			var armorTemplate = bodyTemplate.ArmorsTemplates.First();

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
