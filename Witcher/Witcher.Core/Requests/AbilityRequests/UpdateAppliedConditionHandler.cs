using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.AbilityRequests
{
	public class UpdateAppliedConditionHandler : BaseHandler<UpdateAppliedCondionCommand, Unit>
	{
		public UpdateAppliedConditionHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService) { }

		public override async Task<Unit> Handle(UpdateAppliedCondionCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Abilities.Where(a => a.Id == request.AbilityId))
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new NoAccessToEntityException<Game>();

			var ability = game.Abilities.FirstOrDefault(a => a.Id == request.AbilityId)
				?? throw new EntityNotFoundException<Ability>(request.AbilityId);

			var currentAppliedCondition = ability.AppliedConditions.FirstOrDefault(x => x.Id == request.Id);

			if (ability.AppliedConditions.Any(x => x.Condition == request.Condition && x.Id != request.Id))
				throw new RequestNotUniqException<UpdateAppliedCondionCommand>(nameof(request.Condition));

			if (currentAppliedCondition is null)
				ability.AppliedConditions.Add(new AppliedCondition(request.Condition, request.ApplyChance));
			else
				currentAppliedCondition.ChangeAppliedCondition(request.Condition, request.ApplyChance);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
