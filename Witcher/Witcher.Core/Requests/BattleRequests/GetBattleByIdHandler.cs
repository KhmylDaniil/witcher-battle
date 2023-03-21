using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.BattleRequests
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
					?? throw new EntityNotFoundException<Battle>(request.Id);

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
				BattleId = battle.Id,
				Creatures = creatures
			};
		}
	}
}
