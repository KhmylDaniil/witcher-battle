using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.InterfaceRequests.GetInterfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.InterfaceRequests.GetInterfaces
{
	/// <summary>
	/// Обработчик <see cref="GetInterfacesQuery"/>
	/// </summary>
	public class GetInterfacesQueryHandler : IRequestHandler<GetInterfacesQuery, GetInterfacesQueryResponse>
	{
		private readonly IAppDbContext _appDbContext;
		private readonly IMapper _mapper;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="mapper">АвтоМаппер</param>
		public GetInterfacesQueryHandler(IAppDbContext appDbContext, IMapper mapper)
		{
			_appDbContext = appDbContext;
			_mapper = mapper;
		}

		/// <summary>
		/// Обработать запрос, получить список интерфейсов
		/// </summary>
		/// <param name="request">Тип интерфейса</param>
		/// <param name="cancellationToken">Отмена запроса</param>
		/// <returns>Список интерфейсов</returns>
		public async Task<GetInterfacesQueryResponse> Handle(GetInterfacesQuery request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			var list = await _appDbContext.Interfaces
				.Where(x => request.SearchText == null || x.Type.Contains(request.SearchText))
				.Select(x => new GetInterfacesQueryResponseItem
				{
					Id = x.Id,
					Name = x.Name,
				})
				.ToListAsync(cancellationToken);

			return new GetInterfacesQueryResponse
			{
				InterfacesList = list,
				TotalCount = list.Count(),
			};
		}
	}
}
