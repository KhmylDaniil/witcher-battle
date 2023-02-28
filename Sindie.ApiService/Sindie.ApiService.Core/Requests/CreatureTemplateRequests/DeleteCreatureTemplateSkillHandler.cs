using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests;
using Sindie.ApiService.Core.Exceptions;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests
{
	public class DeleteCreatureTemplateSkillHandler : BaseHandler<DeleteCreatureTemplateSkillCommand, Unit>
	{
		public DeleteCreatureTemplateSkillHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(DeleteCreatureTemplateSkillCommand request, CancellationToken cancellationToken)
		{
			var creatureTemplate = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.SelectMany(g => g.CreatureTemplates.Where(ct => ct.Id == request.CreatureTemplateId))
					.Include(ct => ct.CreatureTemplateSkills)
				.FirstOrDefaultAsync(ct => ct.Id == request.CreatureTemplateId, cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			var creatureTemplateSkill = creatureTemplate.CreatureTemplateSkills.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<CreatureTemplateSkill>(request.Id);

			creatureTemplate.CreatureTemplateSkills.Remove(creatureTemplateSkill);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
