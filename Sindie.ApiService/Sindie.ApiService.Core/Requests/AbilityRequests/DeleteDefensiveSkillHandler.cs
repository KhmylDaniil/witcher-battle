using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.AbilityRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.AbilityRequests
{
	public class DeleteDefensiveSkillHandler : BaseHandler<DeleteDefensiveSkillCommand, Unit>
	{
		public DeleteDefensiveSkillHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService) { }

		public override async Task<Unit> Handle(DeleteDefensiveSkillCommand request, CancellationToken cancellationToken)
		{	
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Abilities.Where(a => a.Id == request.AbilityId))
					.ThenInclude(a => a.DefensiveSkills)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionNoAccessToEntity<Game>();

			var ability = game.Abilities.FirstOrDefault(a => a.Id == request.AbilityId)
				?? throw new ExceptionEntityNotFound<Ability>(request.AbilityId);

			if (ability.DefensiveSkills.Count == 1)
				throw new ArgumentException("Can`t delete last defensive skill.");

			var defensiveSkill = ability.DefensiveSkills.FirstOrDefault(ds => ds.Id == request.Id)
				?? throw new ExceptionEntityNotFound<DefensiveSkill>(request.Id);

			ability.DefensiveSkills.Remove(defensiveSkill);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
