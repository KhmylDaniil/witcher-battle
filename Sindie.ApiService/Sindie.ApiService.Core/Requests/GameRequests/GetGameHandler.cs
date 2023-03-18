using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.GameRequests;
using Witcher.Core.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.GameRequests
{
	public class GetGameHandler : BaseHandler<GetGameQuery, IEnumerable<GetGameResponseItem>>
	{
		public GetGameHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<IEnumerable<GetGameResponseItem>> Handle(GetGameQuery request, CancellationToken cancellationToken)
		{
			var filter = _appDbContext.Games
				.Include(g => g.TextFiles)
				.Include(g => g.ImgFiles)
				.Include(g => g.UserGames)
					.ThenInclude(ug => ug.User)
					.Where(g => request.Name == null || g.Name.Contains(request.Name))
					.Where(g => request.Description == null || g.Description.Contains(request.Description))
					.Where(g => request.AuthorName == null || g.UserGames
							.Any(ug => ug.User.Name.Contains(request.AuthorName) && ug.UserId == g.CreatedByUserId));

			var list = await filter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.ToListAsync(cancellationToken);

			return list.Select(x => new GetGameResponseItem()
			{
				Id = x.Id,
				Name = x.Name,
				Description = x.Description,
				AvatarId = x.AvatarId,
				Users = x.UserGames.Select(ug => ug.User).DistinctBy(u => u.Id).ToDictionary(x => x.Id, x => x.Name),
				TextFiles = x.TextFiles.Select(tf => tf.Id).ToList(),
				ImgFiles = x.ImgFiles.Select(imf => imf.Id).ToList()
			}).ToList();
		}
	}
}
