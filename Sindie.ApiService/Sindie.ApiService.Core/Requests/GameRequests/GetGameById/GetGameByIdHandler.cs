using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.GameRequests.GetGameById;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.GameRequests.GetGameById
{
	public class GetGameByIdHandler  : BaseHandler<GetGameByIdCommand, GetGameByIdResponse>
	{
		public GetGameByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<GetGameByIdResponse> Handle(GetGameByIdCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.UserGameFilter(_appDbContext.Games, request.Id)
				.Include(g => g.TextFiles)
				.Include(g => g.ImgFiles)
				.Include(g => g.UserGames)
					.ThenInclude(ug => ug.User)
				.FirstAsync(cancellationToken: cancellationToken);

			return new GetGameByIdResponse
			{
				Id = game.Id,
				GameMasterName = game.UserGames.Select(ug => ug.User).First(u => u.Id == game.CreatedByUserId).Name,
				Name = game.Name,
				Description = game.Description,
				AvatarId = game.AvatarId,
				Users = game.UserGames.Select(ug => ug.User).DistinctBy(u => u.Id).ToDictionary(x => x.Id, x => x.Name),
				TextFiles = game.TextFiles.Select(tf => tf.Id).ToList(),
				ImgFiles = game.ImgFiles.Select(imf => imf.Id).ToList()
			};
		}
	}
}
