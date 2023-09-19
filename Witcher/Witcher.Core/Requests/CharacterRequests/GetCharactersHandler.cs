using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CharacterRequests;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.ExtensionMethods;

namespace Witcher.Core.Requests.CharacterRequests
{
	public sealed class GetCharactersHandler : BaseHandler<GetCharactersCommand, IEnumerable<GetCharactersResponseItem>>
	{
		public GetCharactersHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<IEnumerable<GetCharactersResponseItem>> Handle(GetCharactersCommand request, CancellationToken cancellationToken)
		{
			var filter = _authorizationService.UserGameFilter(_appDbContext.Games)
				.Include(g => g.Characters)
				.ThenInclude(c => c.UserGameCharacters)
				.ThenInclude(ugc => ugc.UserGame)
				.ThenInclude(ug => ug.User)
				.SelectMany(x => x.Characters
					.Where(x => request.UserName == null || x.UserGameCharacters
						.Any(ugc => ugc.UserGame.User.Name.Contains(request.UserName)))
					.Where(x => request.Name == null || x.Name.Contains(request.Name)));

			var list = await filter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.Select(x => new GetCharactersResponseItem()
				{
					Name = x.Name,
					Id = x.Id,
					OwnerName = x.UserGameCharacters.Select(ugc => ugc.UserGame)
					.OrderBy(ug => ug.GameRoleId != BaseData.GameRoles.MainMasterRoleId)
					.FirstOrDefault().User.Name
				}).ToListAsync(cancellationToken);

			return list;
		}
	}
}
