using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BagRequests.GiveItems
{
	/// <summary>
	/// Обработчик команды получения предметов
	/// </summary>
	public class ReceiveItemsHandler : IRequestHandler<ReceiveItemsCommand, Unit>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Конструктор обработчика команды получения предметов
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		public ReceiveItemsHandler(IAppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		/// <summary>
		/// Обработка команды получения предметов
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public async Task<Unit> Handle(ReceiveItemsCommand request, CancellationToken cancellationToken)
		{
			var notification = await _appDbContext.NotificationsTradeRequest.Where(n => n.Id == request.NotificationId)
				.Include(n => n.SourceBag)
					.ThenInclude(b => b.BagItems)
						.ThenInclude(bi => bi.Item)
				.Include(n => n.ReceiveCharacter)
					.ThenInclude(c => c.Bag)
						.ThenInclude(b => b.BagItems)
							.ThenInclude(bi => bi.Item)
				.FirstOrDefaultAsync();

			CheckRequest(notification);

			var requestData = BagItemData.CreateBagItemsData(notification);

			var sourceData = BagItemData.UnblockData(
				BagItemData.CreateBagItemsData(notification.SourceBag), requestData);

			var receiveData = BagItemData.MergeData(
				requestData, BagItemData.CreateBagItemsData(notification.ReceiveCharacter.Bag));
				
			var capacityMessage = Bag.CheckBagCapacity(
				receiveData,
				notification.ReceiveCharacter.Bag.MaxBagSize,
				notification.ReceiveCharacter.Bag.MaxWeight);
			if (!string.IsNullOrEmpty(capacityMessage))
				request.Consent = false;

			if (request.Consent is false)
				notification.SourceBag.UpdateBagItems(sourceData);
	        else
            {
				notification.SourceBag.UpdateBagItems(
					BagItemData.SubstractData(sourceData, requestData));
				notification.ReceiveCharacter.Bag.UpdateBagItems(receiveData);
            }

			_appDbContext.NotificationsTradeRequest.Remove(notification);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		/// <summary>
		/// Проверить правильность сущностей
		/// </summary>
		/// <param name="notification">Уведомление</param>
		private static void CheckRequest(NotificationTradeRequest notification)
        {
            if (notification.BagItems is null)
                throw new ExceptionEntityNotIncluded<BagItemData>(nameof(notification.BagItems));
            var sourceBag = notification.SourceBag
                ?? throw new ExceptionEntityNotIncluded<Bag>(nameof(Bag), notification.SourceBagId);
			if (sourceBag.BagItems is null)
				throw new ExceptionEntityNotIncluded<BagItem>(nameof(sourceBag));
			var receiveBag = notification.ReceiveCharacter.Bag
				?? throw new ExceptionEntityNotIncluded<Bag>(nameof(Bag), notification.ReceiveBagId);
			if (receiveBag.BagItems is null)
				throw new ExceptionEntityNotIncluded<BagItem>(nameof(receiveBag));

			foreach (var bagItem in notification.BagItems)
            {
                var item = sourceBag.BagItems.Select(x => x.Item).FirstOrDefault(x => x.Id == bagItem.ItemId)
                    ?? throw new ExceptionEntityNotFound<Item>(bagItem.ItemId);

                var sourceBagItem = sourceBag.BagItems.FirstOrDefault(x => x.Stack == bagItem.Stack && x.ItemId == item.Id)
                    ?? throw new ExceptionEntityNotFound<BagItem>(nameof(bagItem.Stack) + $"{bagItem.Stack}");

                if (bagItem.Quantity < 1 || bagItem.Quantity > sourceBagItem.QuantityItem)
                    throw new ExceptionRequestFieldIncorrectData<ReceiveItemsCommand>(nameof(bagItem.Quantity), nameof(sourceBagItem.QuantityItem));

                if (bagItem.Stack < 0
                    || (sourceBag.MaxBagSize != null && bagItem.Stack >= sourceBag.MaxBagSize))
                    throw new ExceptionRequestFieldIncorrectData<ReceiveItemsCommand>(nameof(bagItem.Stack), nameof(sourceBag.MaxBagSize));
                if (notification.BagItems.Where(x => x.Stack == bagItem.Stack).Count() != 1)
                    throw new ApplicationException("Значения в поле стак повторяются");
            }

			foreach (var bagItem in receiveBag.BagItems)
            {
				var item = receiveBag.BagItems.Select(x => x.Item).FirstOrDefault(x => x.Id == bagItem.ItemId)
					?? throw new ExceptionEntityNotFound<Item>(bagItem.ItemId);

				if (bagItem.QuantityItem < 1 || bagItem.QuantityItem > bagItem.MaxQuantityItem)
					throw new ExceptionFieldOutOfRange<Bag>(nameof(bagItem.QuantityItem), nameof(bagItem.MaxQuantityItem));

				if (bagItem.Stack < 0
					|| (receiveBag.MaxBagSize != null && bagItem.Stack >= receiveBag.MaxBagSize))
					throw new ExceptionFieldOutOfRange<Bag>(nameof(bagItem.Stack), $"максимуму в {bagItem.Stack}.");
				if (receiveBag.BagItems.Where(x => x.Stack == bagItem.Stack).Count() != 1)
					throw new ApplicationException("Значения в поле стак повторяются");
			}
        }
    }
}
