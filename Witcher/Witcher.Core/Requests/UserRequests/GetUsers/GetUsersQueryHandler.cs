using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.UserRequests.GetUsers;
using Witcher.Core.ExtensionMethods;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.UserRequests.GetUsers
{
	/// <summary>
	/// Обработчик <see cref="GetUsersQuery"/>
	/// </summary>
	public class GetUsersQueryHandler : BaseHandler<GetUsersQuery, GetUsersQueryResponse>
	{
		public GetUsersQueryHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		/// <summary>
		/// Обработать запрос, получить список пользователей
		/// </summary>
		/// <param name="request">Искомое Имя пользователя или Email</param>
		/// <param name="cancellationToken">Отмена запроса</param>
		/// <returns>Список пользователей</returns>
		public override async Task<GetUsersQueryResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
		{
			var searchFilter = _appDbContext.Users
				.Where(x => request.SearchText == null || x.Name.Contains(request.SearchText) || x.Email.Contains(request.SearchText));

			var list = await searchFilter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.Select(x => new GetUsersQueryResponseItem()
				{
					Id = x.Id,
					Name = x.Name,
					Email = x.Email,
					Phone = x.Phone,
					CreatedByUserId = x.CreatedByUserId,
					ModifiedByUserId = x.ModifiedByUserId,
					CreatedOn = x.CreatedOn,
					ModifiedOn = x.ModifiedOn,
				}).ToListAsync(cancellationToken);

			return new GetUsersQueryResponse
			{
				UsersList = list,
				TotalCount = list.Count
			};
		}
	}
}
