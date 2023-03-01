using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BattleRequests
{
	public class DeleteCreatureHandler : BaseHandler<DeleteCreatureCommand, Unit>
	{
		public DeleteCreatureHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<Unit> Handle(DeleteCreatureCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Battles.Where(b => b.Id == request.BattleId))
					.ThenInclude(b => b.Creatures.Where(c => c.Id == request.Id))
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionNoAccessToEntity<Game>();

			var battle = game.Battles.FirstOrDefault(a => a.Id == request.BattleId)
				?? throw new ExceptionEntityNotFound<Battle>(request.BattleId);

			var creature = battle.Creatures.FirstOrDefault(a => a.Id == request.Id)
				?? throw new ExceptionEntityNotFound<Creature>(request.Id);

			battle.Creatures.Remove(creature);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
