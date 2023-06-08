using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.ItemTemplateRequests;
using Witcher.Core.ExtensionMethods;

namespace Witcher.Core.Requests.ItemTemplateRequests
{
	public class GetItemTemplateHandler : BaseHandler<GetItemTemplateQuery, IEnumerable<GetItemTemplateResponse>>
	{
		public GetItemTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<IEnumerable<GetItemTemplateResponse>> Handle(GetItemTemplateQuery request, CancellationToken cancellationToken)
		{
			var filter = _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.ItemTemplates)
				.SelectMany(g => g.ItemTemplates);

			return await filter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.Select(x => new GetItemTemplateResponse()
				{
					Id = x.Id,
					Name = x.Name,
					ItemType = x.ItemType
				}).ToListAsync(cancellationToken);
		}
	}
}
