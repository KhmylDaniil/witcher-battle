using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests
{
	public class UpdateCreatureTemplateSkillHandler : BaseHandler<UpdateCreatureTemplateSkillCommand, Unit>
	{
		public UpdateCreatureTemplateSkillHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(UpdateCreatureTemplateSkillCommand request, CancellationToken cancellationToken)
		{
			var creatureTemplate = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.SelectMany(g => g.CreatureTemplates.Where(g => g.Id == request.CreatureTemplateId))
					.Include(ct => ct.CreatureTemplateSkills)
				.FirstOrDefaultAsync(cancellationToken)
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
