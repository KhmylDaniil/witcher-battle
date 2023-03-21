using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Exceptions;

namespace Witcher.Core.Requests.CreatureTemplateRequests
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
					?? throw new NoAccessToEntityException<Game>();

			var creatureTemplateSkill = creatureTemplate.CreatureTemplateSkills.FirstOrDefault(x => x.Id == request.Id)
				?? throw new EntityNotFoundException<CreatureTemplateSkill>(request.Id);

			creatureTemplate.CreatureTemplateSkills.Remove(creatureTemplateSkill);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
