using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Logic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BattleRequests
{
	public class GetBattleByIdHandler : BaseHandler<GetBattleByIdQuery, GetBattleByIdResponse>
	{
		public GetBattleByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<GetBattleByIdResponse> Handle(GetBattleByIdQuery request, CancellationToken cancellationToken)
		{
			var battle = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Battles.Where(b => b.Id == request.Id))
					.ThenInclude(b => b.Creatures)
						.ThenInclude(c => c.CreatureTemplate)
				.Include(g => g.Battles.Where(b => b.Id == request.Id))
					.ThenInclude(b => b.Creatures)
						.ThenInclude(c => c.Effects)
				.SelectMany(g => g.Battles)
				.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken)
					?? throw new ExceptionEntityNotFound<Battle>(request.Id);

			var creatures = battle.Creatures.Select(x => new GetBattleByIdResponseItem()
			{
				Id = x.Id,
				Name = x.Name,
				CreatureTemplateName = x.CreatureTemplate.Name,
				Description = x.Description,	
				HP = (x.HP, x.MaxHP),
				Effects = string.Join(", ", x.Effects.Select(x => x.Name))
			}).ToList();

			return new GetBattleByIdResponse()
			{
				Name = battle.Name,
				Description = battle.Description,
				Id = battle.Id,
				Creatures = creatures
			};
		}
	}
}
