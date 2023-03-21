using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.CreatureTemplateRequests
{
	public class ChangeCreatureTemplatePartHandler : BaseHandler<ChangeCreatureTemplatePartCommand, Unit>
	{
		public ChangeCreatureTemplatePartHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService)
		{
		}

		public override async Task<Unit> Handle(ChangeCreatureTemplatePartCommand request, CancellationToken cancellationToken)
		{
			var creatureTemplate = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.SelectMany(g => g.CreatureTemplates.Where(g => g.Id == request.CreatureTemplateId))
					.Include(ct => ct.CreatureTemplateParts)
				.FirstOrDefaultAsync(ct => ct.Id == request.CreatureTemplateId, cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var partsToUpdate = request.Id == null
				? creatureTemplate.CreatureTemplateParts
				: creatureTemplate.CreatureTemplateParts.Where(ct => ct.Id == request.Id).ToList();

			foreach (var part in partsToUpdate)
				part.Armor = request.ArmorValue;

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
