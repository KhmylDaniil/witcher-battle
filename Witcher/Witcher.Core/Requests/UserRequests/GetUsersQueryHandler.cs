using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.ExtensionMethods;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Contracts.UserRequests;
using System.Collections.Generic;

namespace Witcher.Core.Requests.UserRequests
{
	/// <summary>
	/// Обработчик <see cref="GetUsersQuery"/>
	/// </summary>
	public class GetUsersQueryHandler : BaseHandler<GetUsersQuery, IEnumerable<GetUsersQueryResponse>>
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
		public override async Task<IEnumerable<GetUsersQueryResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
		{
			var searchFilter = _appDbContext.Users
				.Where(x => request.SearchText == null || x.Name.Contains(request.SearchText) || x.Email.Contains(request.SearchText));

			return await searchFilter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.Select(x => new GetUsersQueryResponse()
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
		}
	}
}
