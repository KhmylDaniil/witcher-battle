using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.AbilityRequests.UpdateAppliedCondition;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.AbilityRequests.UpdateAppliedCondition
{
	public class UpdateAppliedConditionHandler : BaseHandler<UpdateAppliedCondionCommand, Unit>
	{
		public UpdateAppliedConditionHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService) { }

		public override async Task<Unit> Handle(UpdateAppliedCondionCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(g => g.Abilities.Where(a => a.Id == request.AbilityId))
					.ThenInclude(a => a.AppliedConditions)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionNoAccessToEntity<Game>();

			var ability = game.Abilities.FirstOrDefault(a => a.Id == request.AbilityId)
				?? throw new ExceptionEntityNotFound<Ability>(request.AbilityId);

			var currentAppliedCondition = ability.AppliedConditions.FirstOrDefault(x => x.Id == request.Id);

			if (ability.AppliedConditions.Any(x => x.Condition == request.Condition && x.Id != request.Id))
				throw new RequestNotUniqException<UpdateAppliedCondionCommand>(nameof(request.Condition));

			if (currentAppliedCondition is null)
				ability.AppliedConditions.Add(AppliedCondition.CreateAppliedCondition(ability, request.Condition, request.ApplyChance));
			else
				currentAppliedCondition.ChangeAppliedCondition(request.Condition, request.ApplyChance);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
