using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.AbilityRequests
{
	public sealed class DeleteDefensiveSkillHandler : BaseHandler<DeleteDefensiveSkillCommand, Unit>
	{
		public DeleteDefensiveSkillHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService) { }

		public override async Task<Unit> Handle(DeleteDefensiveSkillCommand request, CancellationToken cancellationToken)
		{	
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Abilities.Where(a => a.Id == request.AbilityId))
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new NoAccessToEntityException<Game>();

			var ability = game.Abilities.Find(a => a.Id == request.AbilityId)
				?? throw new EntityNotFoundException<Ability>(request.AbilityId);

			if (ability.DefensiveSkills.Count == 1)
				throw new RequestValidationException("Can`t delete last defensive skill.");

			var defensiveSkill = ability.DefensiveSkills.Find(ds => ds.Id == request.Id)
				?? throw new EntityNotFoundException<DefensiveSkill>(request.Id);

			ability.DefensiveSkills.Remove(defensiveSkill);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
