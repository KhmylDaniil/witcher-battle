using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.RequestExceptions;
using Witcher.Core.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.CreatureTemplateRequests
{
	public class UpdateCreatureTemplateSkillHandler : BaseHandler<UpdateCreatureTemplateSkillCommand, Unit>
	{
		public UpdateCreatureTemplateSkillHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(UpdateCreatureTemplateSkillCommand request, CancellationToken cancellationToken)
		{
			var creatureTemplate = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.SelectMany(g => g.CreatureTemplates.Where(ct => ct.Id == request.CreatureTemplateId))
					.Include(ct => ct.CreatureTemplateSkills)
				.FirstOrDefaultAsync(ct => ct.Id == request.CreatureTemplateId, cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			var currentSkill = creatureTemplate.CreatureTemplateSkills.FirstOrDefault(x => x.Id == request.Id);

			if (creatureTemplate.CreatureTemplateSkills.Any(x => x.Skill == request.Skill && x.Id != request.Id))
				throw new RequestNotUniqException<UpdateCreatureTemplateSkillCommand>(nameof(request.Skill));

			if (currentSkill is null)
				creatureTemplate.CreatureTemplateSkills.Add(new CreatureTemplateSkill(
					request.Value, creatureTemplate, request.Skill));
			else
				currentSkill.ChangeValue(request.Value);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
