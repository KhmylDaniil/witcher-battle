using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BagRequests.TakeItems
{
	/// <summary>
	/// Обработчик команды помещения предметов в сумку
	/// </summary>
	public class TakeItemsHandler : IRequestHandler<TakeItemsCommand, Unit>
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
		/// Конструктор обработчика команды помещения предметов в сумку
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public TakeItemsHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Помещение предметов в сумку
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public async Task<Unit> Handle(TakeItemsCommand request, CancellationToken cancellationToken)
		{
			var list = new List<Guid>() { request.SourceBagId, request.ReceiveBagId };
			var game = await _authorizationService.BagOwnerOrMasterFilter(
				_appDbContext.Games, request.GameId, request.ReceiveBagId)
				.Include(x => x.Instances.Where(i => i.Id == request.InstanceId))
					.ThenInclude(y => y.Bags.Where(b =>  list.Contains(b.Id)))
						.ThenInclude(b => b.NotificationDeletedItems)
				.Include(x => x.Instances.Where(i => i.Id == request.InstanceId))
					.ThenInclude(y => y.Bags.Where(b => list.Contains(b.Id)))
						.ThenInclude(b => b.BagItems)
							.ThenInclude(bi => bi.Item)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionNoAccessToEntity<Game>();

			var sourceBag = game.Instances.SelectMany(x => x.Bags).FirstOrDefault(x => x.Id == request.SourceBagId)
				?? throw new ExceptionEntityNotIncluded<Bag>(nameof(Game), request.GameId);
			var receiveBag = game.Instances.SelectMany(x => x.Bags).FirstOrDefault(x => x.Id == request.ReceiveBagId)
				?? throw new ExceptionEntityNotIncluded<Bag>(nameof(Game), request.GameId);

			CheckRequest(request, sourceBag, receiveBag);

			var requestData = BagItemData.CreateBagItemsData(request, sourceBag);

			var mergedData = BagItemData.MergeData(
				requestData,
				BagItemData.CreateBagItemsData(receiveBag));
			var substractedData = BagItemData.SubstractData(
				BagItemData.CreateBagItemsData(sourceBag),
				requestData);

			var capacityMessage = Bag.CheckBagCapacity(mergedData, receiveBag.MaxBagSize, receiveBag.MaxWeight);
			if (!string.IsNullOrEmpty(capacityMessage))
				throw new Exception(capacityMessage);

			receiveBag.UpdateBagItems(mergedData);

			sourceBag.NotifyDeleteItems(substractedData);
			sourceBag.UpdateBagItems(substractedData);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		/// <summary>
		/// Проверить правильность сущностей
		/// </summary>
		/// <param name="request">Данные</param>
		/// <param name="sourceBag">Сумка-источник</param>
		/// <param name="receiveBag">Сумка-получатель</param>
		private static void CheckRequest(TakeItemsCommand request, Bag sourceBag, Bag receiveBag)
		{
			if (request.BagItems is null)
				throw new ExceptionRequestFieldIncorrectData<TakeItemsCommand>(nameof(request.BagItems));
			if (sourceBag.BagItems is null)
				throw new ExceptionEntityNotIncluded<BagItem>(nameof(sourceBag));
			if (receiveBag.BagItems is null)
				throw new ExceptionEntityNotIncluded<BagItem>(nameof(receiveBag));

			foreach (var bagItem in request.BagItems)
			{
				if (bagItem.QuantityItem < 1)
					throw new ExceptionRequestFieldIncorrectData<TakeItemsCommand>(nameof(bagItem.QuantityItem), "положительному числу");
				_ = sourceBag.BagItems.Select(x => x.Item).FirstOrDefault(x => x.Id == bagItem.ItemId)
					?? throw new ExceptionEntityNotFound<Item>(bagItem.ItemId);
			}	

			foreach (var item in sourceBag.BagItems.Select(x => x.Item).Distinct())
			{
				var totalInSource = 0;
				var totalBlocked = 0;		
				foreach (var bagItem in sourceBag.BagItems.Where(x => x.ItemId == item.Id))
				{
					totalInSource += bagItem.QuantityItem;
					totalBlocked += bagItem.Blocked;
				}
				var totalToTake = 0;
				foreach (var bagItem in request.BagItems.Where(x => x.ItemId == item.Id))
					totalToTake += bagItem.QuantityItem;

				if (totalInSource - totalBlocked < totalToTake)
					throw new ApplicationException($"Невозможно переместить {totalToTake} единиц предмета {item.Name}. Доступно {totalInSource - totalBlocked} единиц");
			}
		}
	}
}
