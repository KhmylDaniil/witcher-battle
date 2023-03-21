using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Exceptions.RequestExceptions;
using Witcher.Core.Logic;

namespace Witcher.Core.Requests.BattleRequests
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
				?? throw new NoAccessToEntityException<Game>();

			var battle = game.Battles.FirstOrDefault(a => a.Id == request.BattleId)
				?? throw new EntityNotFoundException<Battle>(request.BattleId);

			if (battle.Creatures.Any(c => c.Name == request.Name))
				throw new RequestNameNotUniqException<Creature>(request.Name);

			var creatureTemplate = game.CreatureTemplates.FirstOrDefault(a => a.Id == request.CreatureTemplateId)
				?? throw new EntityNotFoundException<CreatureTemplate>(request.CreatureTemplateId);

			battle.Creatures.Add(new Creature(creatureTemplate, battle, request.Name, request.Description));

			RunBattle.FormInitiativeOrder(battle);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
