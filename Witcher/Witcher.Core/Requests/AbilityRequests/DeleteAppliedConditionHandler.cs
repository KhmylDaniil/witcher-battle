﻿using MediatR;
using Witcher.Core.Abstractions;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Contracts.AbilityRequests;

namespace Witcher.Core.Requests.AbilityRequests
{
	public class DeleteAppliedConditionHandler : BaseHandler<DeleteAppliedCondionCommand, Unit>
	{
		public DeleteAppliedConditionHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService) { }

		public override async Task<Unit> Handle(DeleteAppliedCondionCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
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
