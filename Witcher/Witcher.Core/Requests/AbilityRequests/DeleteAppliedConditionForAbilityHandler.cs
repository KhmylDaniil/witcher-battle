using MediatR;
using Witcher.Core.Abstractions;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.AbilityRequests
{
	public class DeleteAppliedConditionForAbilityHandler : BaseHandler<DeleteAppliedConditionForAbilityCommand, Unit>
	{
		public DeleteAppliedConditionForAbilityHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService) { }

		public override async Task<Unit> Handle(DeleteAppliedConditionForAbilityCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Abilities.Where(a => a.Id == request.AbilityId))
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new NoAccessToEntityException<Game>();

			var ability = game.Abilities.FirstOrDefault(a => a.Id == request.AbilityId)
				?? throw new EntityNotFoundException<Ability>(request.AbilityId);

			var appliedCondition = ability.AppliedConditions.FirstOrDefault(x => x.Id == request.Id)
				?? throw new EntityNotFoundException<AppliedCondition>(request.Id);

			ability.AppliedConditions.Remove(appliedCondition);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
