using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests;
using Sindie.ApiService.Core.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BattleRequests
{
	public class GetBattleHandler : BaseHandler<GetBattleQuery, IEnumerable<GetBattleResponseItem>>
	{
		public GetBattleHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<IEnumerable<GetBattleResponseItem>> Handle(GetBattleQuery request, CancellationToken cancellationToken)
		{
			var filter = _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Battles)
					.SelectMany(g => g.Battles
						.Where(x => request.Name == null || x.Name.Contains(request.Name))
						.Where(x => request.Description == null || x.Description.Contains(request.Description)));

			var list = await filter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.Select(x => new GetBattleResponseItem()
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description
				})
				.ToListAsync(cancellationToken);

			return list;
		}
	}
}
