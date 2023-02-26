using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.GameRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.GameRequests
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
					?? throw new ExceptionNoAccessToEntity<Game>();

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
