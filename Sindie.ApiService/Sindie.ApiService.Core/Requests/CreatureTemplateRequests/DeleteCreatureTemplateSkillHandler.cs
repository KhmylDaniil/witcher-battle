using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests
{
	public class DeleteCreatureTemplateSkillHandler : BaseHandler<DeleteCreatureTemplateSkillCommand, Unit>
	{
		public DeleteCreatureTemplateSkillHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(DeleteCreatureTemplateSkillCommand request, CancellationToken cancellationToken)
		{
			var creatureTemplate = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.SelectMany(g => g.CreatureTemplates.Where(g => g.Id == request.CreatureTemplateId))
					.Include(ct => ct.CreatureTemplateSkills)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			var creatureTemplateSkill = creatureTemplate.CreatureTemplateSkills.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<CreatureTemplateSkill>(request.Id);

			creatureTemplate.CreatureTemplateSkills.Remove(creatureTemplateSkill);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
