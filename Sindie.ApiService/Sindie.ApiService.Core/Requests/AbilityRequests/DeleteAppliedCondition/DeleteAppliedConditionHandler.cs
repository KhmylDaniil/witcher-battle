using MediatR;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sindie.ApiService.Core.Contracts.AbilityRequests.DeleteAppliedCondition;
using Microsoft.EntityFrameworkCore;

namespace Sindie.ApiService.Core.Requests.AbilityRequests.DeleteAppliedCondition
{
	public class DeleteAppliedConditionHandler : BaseHandler, IRequestHandler<DeleteAppliedCondionCommand>
	{
		public DeleteAppliedConditionHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService) { }

		public async Task<Unit> Handle(DeleteAppliedCondionCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(g => g.Abilities.Where(a => a.Id == request.AbilityId))
					.ThenInclude(a => a.AppliedConditions)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionNoAccessToEntity<Game>();

			var ability = game.Abilities.FirstOrDefault(a => a.Id == request.AbilityId)
				?? throw new ExceptionEntityNotFound<Ability>(request.AbilityId);

			var appliedCondition = ability.AppliedConditions.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<AppliedCondition>(request.Id);

			ability.AppliedConditions.Remove(appliedCondition);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
