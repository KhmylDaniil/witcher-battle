using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;

namespace Sindie.ApiService.Core.Requests.BattleRequests
{
	public class CreateCreatureHandler : BaseHandler<CreateCreatureCommand, Unit>
	{
		public CreateCreatureHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<Unit> Handle(CreateCreatureCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Battles.Where(b => b.Id == request.BattleId))
					.ThenInclude(b => b.Creatures)
				.Include(g => g.CreatureTemplates.Where(ct => ct.Id == request.CreatureTemplateId))
					.ThenInclude(ct => ct.CreatureTemplateParts)
				.Include(g => g.CreatureTemplates.Where(ct => ct.Id == request.CreatureTemplateId))
					.ThenInclude(ct => ct.CreatureTemplateSkills)
				.Include(g => g.CreatureTemplates.Where(ct => ct.Id == request.CreatureTemplateId))
					.ThenInclude(ct => ct.DamageTypeModifiers)
				.Include(g => g.CreatureTemplates.Where(ct => ct.Id == request.CreatureTemplateId))
					.ThenInclude(ct => ct.Abilities)
						.ThenInclude(a => a.AppliedConditions)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionNoAccessToEntity<Game>();

			var battle = game.Battles.FirstOrDefault(a => a.Id == request.BattleId)
				?? throw new ExceptionEntityNotFound<Battle>(request.BattleId);

			if (battle.Creatures.Any(c => c.Name == request.Name))
				throw new RequestNameNotUniqException<Creature>(request.Name);

			var creatureTemplate = game.CreatureTemplates.FirstOrDefault(a => a.Id == request.CreatureTemplateId)
				?? throw new ExceptionEntityNotFound<CreatureTemplate>(request.CreatureTemplateId);

			battle.Creatures.Add(new Creature(creatureTemplate, battle, request.Name, request.Description));

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
