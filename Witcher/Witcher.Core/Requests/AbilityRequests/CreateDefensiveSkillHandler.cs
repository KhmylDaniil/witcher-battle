using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.AbilityRequests
{
	public class CreateDefensiveSkillHandler : BaseHandler<CreateDefensiveSkillCommand, Unit>
	{
		public CreateDefensiveSkillHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService) { }

		public override async Task<Unit> Handle(CreateDefensiveSkillCommand request, CancellationToken cancellationToken)
		{
			if (!Enum.IsDefined(request.Skill))
				throw new FieldOutOfRangeException<CreateDefensiveSkillCommand>(nameof(request.Skill));

			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Abilities.Where(a => a.Id == request.AbilityId))
					.ThenInclude(a => a.DefensiveSkills)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new NoAccessToEntityException<Game>();

			var ability = game.Abilities.FirstOrDefault(a => a.Id == request.AbilityId)
				?? throw new EntityNotFoundException<Ability>(request.AbilityId);

			if (ability.DefensiveSkills.Any(x => x.Skill == request.Skill))
				throw new RequestFieldIncorrectDataException<CreateDefensiveSkillCommand>(nameof(request.Skill), "Указанный навык уже включен в список");

			ability.DefensiveSkills.Add(new DefensiveSkill(ability.Id, request.Skill));

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
