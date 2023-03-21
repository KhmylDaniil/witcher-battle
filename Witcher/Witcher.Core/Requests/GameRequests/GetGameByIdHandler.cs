using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.GameRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.GameRequests
{
	public class GetGameByIdHandler : BaseHandler<GetGameByIdCommand, GetGameByIdResponse>
	{
		public GetGameByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService)
		{
		}

		public override async Task<GetGameByIdResponse> Handle(GetGameByIdCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.UserGameFilter(_appDbContext.Games)
				.Include(g => g.TextFiles)
				.Include(g => g.ImgFiles)
				.Include(g => g.UserGames)
					.ThenInclude(ug => ug.User)
				.Include(g => g.UserGames)
					.ThenInclude(ug => ug.GameRole)
				.FirstOrDefaultAsync(cancellationToken: cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			return new GetGameByIdResponse
			{
				Id = game.Id,
				GameMasterName = game.UserGames.Select(ug => ug.User).First(u => u.Id == game.CreatedByUserId).Name,
				Name = game.Name,
				Description = game.Description,
				AvatarId = game.AvatarId,
				Users = game.UserGames.DistinctBy(u => u.Id).ToDictionary(x => x.UserId, x => (x.User.Name, x.GameRole.Name)),
				TextFiles = game.TextFiles.Select(tf => tf.Id).ToList(),
				ImgFiles = game.ImgFiles.Select(imf => imf.Id).ToList()
			};
		}
	}
}
