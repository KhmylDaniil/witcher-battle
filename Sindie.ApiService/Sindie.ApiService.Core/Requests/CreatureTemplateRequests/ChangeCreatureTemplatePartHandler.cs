using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests
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
					?? throw new ExceptionNoAccessToEntity<Game>();

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
