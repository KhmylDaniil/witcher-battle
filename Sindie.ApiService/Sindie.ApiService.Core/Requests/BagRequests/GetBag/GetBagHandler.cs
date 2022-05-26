using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BagRequests.GetBag;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.ExtensionMethods;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BagRequests.GetBag
{
	/// <summary>
	/// Обработчик команды получения сумки и предметов в сумке
	/// </summary>
	public class GetBagHandler : IRequestHandler<GetBagCommand, GetBagResponse>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Сервис авторизации
		/// </summary>
		private readonly IAuthorizationService _authorizationService;

		/// <summary>
		/// Конструктор обработчика команды получения сумки и предметов в сумке
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public GetBagHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Получение сумки и предметов в сумке
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Отфильтрованный запрос</returns>
		public async Task<GetBagResponse> Handle(GetBagCommand request, CancellationToken cancellationToken)
		{
			if (request.PageSize < 1)
				throw new ArgumentOutOfRangeException(nameof(GetBagCommand.PageSize));
			if (request.PageNumber < 1)
				throw new ArgumentOutOfRangeException(nameof(GetBagCommand.PageNumber));

			_= _authorizationService.BagOwnerOrMasterFilter(_appDbContext.Games, request.GameId, request.BagId)
				?? throw new ExceptionNoAccessToEntity<Game>();

			var filter = _authorizationService.BagOwnerOrMasterFilter(_appDbContext.Games, request.GameId, request.BagId)
				.Include(g => g.Instances.Where(i => i.Id == request.InstanceId))
					.ThenInclude(i => i.Bags.Where(b => b.Id == request.BagId))
				.Include(g => g.Slots.Where(x => request.SlotName == null || x.Name.Contains(request.SlotName)))
				.Include(g => g.ItemTemplates.Where(x => request.ItemTemplateName == null || x.Name.Contains(request.ItemTemplateName)))
					.ThenInclude(it => it.Items.Where(x => request.ItemName == null || x.Name.Contains(request.ItemName)))
				.SelectMany(x => x.Instances.SelectMany(i => i.Bags.Where(b => b.Id == request.BagId)
					.SelectMany(b => b.BagItems)))
					.Where(x => request.SlotName == null || x.Item.Slot.Name.Contains(request.SlotName))
					.Where(x => request.ItemTemplateName == null || x.Item.ItemTemplate.Name.Contains(request.ItemTemplateName))
					.Where(x => request.ItemName == null || x.Item.Name.Contains(request.ItemName));

			var list = await filter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.Select(x => new GetBagResponseItem()
				{
					ItemId = x.ItemId,
					ItemName = x.Item.Name,
					QuantityItem = x.QuantityItem,
					MaxQuantityItem = x.MaxQuantityItem,
					Stack = x.Stack,
					Blocked = x.Blocked,
				}).ToListAsync(cancellationToken);
			
			return new GetBagResponse { BagItemsList = list, TotalCount = list.Count };
		}
	}
}
