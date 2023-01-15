using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplate;
using Sindie.ApiService.Core.Contracts.GameRequests.GetGame;
using Sindie.ApiService.Core.ExtensionMethods;
using Sindie.ApiService.Core.Services.Authorization;
using Sindie.ApiService.Core.Services.DateTimeProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Sindie.ApiService.Core.Requests.GameRequests.GetGame
{
	public class GetGameHandler : IRequestHandler<GetGameQuery, GetGameResponse>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		public GetGameHandler(IAppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public async Task<GetGameResponse> Handle(GetGameQuery request, CancellationToken cancellationToken)
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

			var response = list.Select(x => new GetGameResponseItem()
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description,
					AvatarId = x.AvatarId,
					Users = x.UserGames.Select(ug => ug.User).DistinctBy(u => u.Id).ToDictionary(x => x.Id, x => x.Name),
					TextFiles = x.TextFiles.Select(tf => tf.Id).ToList(),
					ImgFiles = x.ImgFiles.Select(imf => imf.Id).ToList()
				}).ToList();

			return new GetGameResponse { GamesList = response, TotalCount = response.Count };
		}
	}
}
