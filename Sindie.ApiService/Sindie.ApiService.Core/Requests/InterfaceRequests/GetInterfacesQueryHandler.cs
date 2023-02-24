using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.InterfaceRequests.GetInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.InterfaceRequests
{
	/// <summary>
	/// Обработчик <see cref="GetInterfacesQuery"/>
	/// </summary>
	public class GetInterfacesQueryHandler : BaseHandler<GetInterfacesQuery, IEnumerable<GetInterfacesQueryResponseItem>>
	{
		public GetInterfacesQueryHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService)
		{
		}

		/// <summary>
		/// Обработать запрос, получить список интерфейсов
		/// </summary>
		/// <param name="request">Тип интерфейса</param>
		/// <param name="cancellationToken">Отмена запроса</param>
		/// <returns>Список интерфейсов</returns>
		public override async Task<IEnumerable<GetInterfacesQueryResponseItem>> Handle(GetInterfacesQuery request, CancellationToken cancellationToken)
			=> await _appDbContext.Interfaces
				.Where(x => request.SearchText == null || x.Type.Contains(request.SearchText))
				.Select(x => new GetInterfacesQueryResponseItem
				{
					Id = x.Id,
					Name = x.Name,
				})
				.ToListAsync(cancellationToken);
	}
}
