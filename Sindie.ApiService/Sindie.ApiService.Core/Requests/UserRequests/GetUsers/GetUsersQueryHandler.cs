using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.UserRequests.GetUsers;
using Sindie.ApiService.Core.ExtensionMethods;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.UserRequests.GetUsers
{
	/// <summary>
	/// Обработчик <see cref="GetUsersQuery"/>
	/// </summary>
	public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetUsersQueryResponse>
	{
		private readonly IAppDbContext _appDbContext;
		private readonly IMapper _mapper;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="mapper">АфтоМаппер</param>
		public GetUsersQueryHandler(IAppDbContext appDbContext, IMapper mapper)
		{
			_appDbContext = appDbContext;
			_mapper = mapper;
		}

		/// <summary>
		/// Обработать запрос, получить список пользователей
		/// </summary>
		/// <param name="request">Искомое Имя пользователя или Email</param>
		/// <param name="cancellationToken">Отмена запроса</param>
		/// <returns>Список пользователей</returns>
		public async Task<GetUsersQueryResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));
			if (request.PageSize < 1)
				throw new Exception("Количество записей должно быть не менее 1");
			if (request.PageNumber < 1)
				throw new Exception("Номер страницы, с которой вы хотите вывести данные долежн быть не менее 1");

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
