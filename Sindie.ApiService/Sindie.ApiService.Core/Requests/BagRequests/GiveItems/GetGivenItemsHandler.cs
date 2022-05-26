using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BagRequests.GiveItems;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BagRequests.GiveItems
{
	/// <summary>
	/// Обработчик команды на получение списка предлагаемых к передаче предметов
	/// </summary>
	public class GetGivenItemsHandler : IRequestHandler<GetGivenItemsCommand, GetGivenItemsResponse>
    {
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Конструктор обработчика команды на получение списка предлагаемых к передаче предметов
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		public GetGivenItemsHandler(
			IAppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		/// <summary>
		/// Получение списка предлагаемых к передаче предметов
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public async Task<GetGivenItemsResponse> Handle(GetGivenItemsCommand request, CancellationToken cancellationToken)
        {

			var filter = _appDbContext.NotificationsTradeRequest.
				Where(n => n.Id == request.NotificationId)
					.SelectMany(n => n.BagItems);

			var list = await filter
				.Select(x => new GetGivenItemsResponseItem()
				{
					ItemId = x.ItemId,
					ItemName = x.ItemName,
					Quantity = x.Quantity,
					MaxQuantity = x.MaxQuantity,
					TotalWeight = x.TotalWeight,
				}).ToListAsync(cancellationToken);

			return new GetGivenItemsResponse { BagItemsList = list, TotalCount = list.Count };
        }
    }
}
