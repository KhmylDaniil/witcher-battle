using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Requests.BagRequests.ChangeBag;
using Sindie.ApiService.Core.Requests.BagRequests.GiveItems;
using Sindie.ApiService.Core.Requests.BagRequests.TakeItems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sindie.ApiService.Core.Requests.BagRequests
{
	/// <summary>
	/// Данные для обновления BagItem
	/// </summary>
	public class BagItemData
	{	
		/// <summary>
		/// Предмет
		/// </summary>
		public Item Item { get; set; }

		/// <summary>
		/// Количество в стаке
		/// </summary>
		public int QuantityItem { get; set; }

		/// <summary>
		/// Стак
		/// </summary>
		public int? Stack { get; set; }

		/// <summary>
		/// Количество заблокированных предметов
		/// </summary>
		public int Blocked { get; set; }

		/// <summary>
		/// Получение данных о списке BagItem
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		/// <returns>Список данных для обновления BagItem</returns>
		internal static List<BagItemData> CreateBagItemData(ChangeBagCommand request, Game game)
		{
			if (request?.BagItems == null)
				throw new ExceptionEntityNotIncluded<ChangeBagCommand>(nameof(request));
			if (game?.ItemTemplates.SelectMany(x => x.Items) == null)
				throw new ExceptionEntityNotIncluded<Game>(nameof(Item));

			var result = new List<BagItemData>();

			foreach (var requestItem in request.BagItems)
				result.Add(new BagItemData()
				{
					Item = game.ItemTemplates.SelectMany(x => x.Items).FirstOrDefault(x => x.Id == requestItem.ItemId),
					QuantityItem = requestItem.QuantityItem,
					Stack = requestItem.Stack,
					Blocked = requestItem.Blocked
				});
		return result;
		}

		/// <summary>
		/// Получение данных о списке BagItem
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="bag">Сумка</param>
		/// <returns>Список данных для обновления BagItem</returns>
		internal static List<BagItemData> CreateBagItemsData(TakeItemsCommand request, Bag bag)
		{
			if (request?.BagItems == null)
				throw new ExceptionEntityNotIncluded<TakeItemsCommand>(nameof(request));
			if (bag?.BagItems == null)
				throw new ExceptionEntityNotIncluded<BagItem>(nameof(bag));

			var result = new List<BagItemData>();

			foreach (var requestItem in request.BagItems)
				result.Add(new BagItemData()
				{
					Item = bag.BagItems.Select(x => x.Item).FirstOrDefault(x => x.Id == requestItem.ItemId),
					QuantityItem = requestItem.QuantityItem,
					Stack = null,
					Blocked = 0
				});
			return result;
		}

		/// <summary>
		/// Получение данных о списке BagItem
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="bag">Сумка</param>
		/// <returns>Список данных для обновления BagItem</returns>
		internal static List<BagItemData> CreateBagItemsData(GiveItemsCommand request, Bag bag)
		{
			if (request?.BagItems == null)
				throw new ExceptionEntityNotIncluded<GiveItemsCommand>(nameof(request));
			if (bag?.BagItems == null)
				throw new ExceptionEntityNotIncluded<BagItem>(nameof(bag));

			var result = new List<BagItemData>();

			foreach (var requestItem in request.BagItems)
				result.Add(new BagItemData()
				{
					Item = bag.BagItems.Select(x => x.Item).FirstOrDefault(x => x.Id == requestItem.ItemId),
					QuantityItem = 0,
					Stack = requestItem.Stack,
					Blocked = requestItem.ToBlockQuantity
				});
			return result;
		}

		/// <summary>
		/// Получение данных о списке BagItem
		/// </summary>
		/// <param name="bag">Сумка-источник</param>
		/// <returns>Список данных для обновления BagItem</returns>
		internal static List<BagItemData> CreateBagItemsData(Bag bag)
		{
			if (bag?.BagItems == null)
				throw new ExceptionEntityNotIncluded<BagItem>(nameof(bag));

			var result = new List<BagItemData>();

			foreach (var bagItem in bag.BagItems)
				result.Add(new BagItemData()
				{
					Item = bagItem.Item,
					QuantityItem = bagItem.QuantityItem,
					Stack = bagItem.Stack,
					Blocked = bagItem.Blocked
				});
			return result;
		}

		/// <summary>
		/// Получение данных о списке BagItem
		/// </summary>
		/// <param name="request">Уведомление о предложении получить предметы</param>
		/// <returns>Список данных для обновления BagItem</returns>
		internal static List<BagItemData> CreateBagItemsData(NotificationTradeRequest request)
		{
			if (request?.BagItems == null)
				throw new ExceptionEntityNotIncluded<NotificationTradeRequestItem>(nameof(request));
			if (request.SourceBag?.BagItems == null)
				throw new ExceptionEntityNotIncluded<BagItem>(nameof(request.SourceBag));

			var result = new List<BagItemData>();

			foreach (var bagItem in request.BagItems)
				result.Add(new BagItemData()
				{
					Item = request.SourceBag.BagItems.Select(x => x.Item).FirstOrDefault(x => x.Id == bagItem.ItemId),
					QuantityItem = bagItem.Quantity,
					Stack = bagItem.Stack,
					Blocked = 0
				});
			return result;
		}

		/// <summary>
		/// Слияние данных
		/// </summary>
		/// <param name="sourceData">Данные источника</param>
		/// <param name="receiveData">Данные получателя</param>
		/// <returns>Результат</returns>
		internal static List<BagItemData> MergeData(IEnumerable<BagItemData> sourceData, IEnumerable<BagItemData> receiveData)
		{
			if (sourceData == null || receiveData == null)
				throw new ArgumentNullException("Не поступили данные для слияния");

			var result = receiveData.ToList();
			var sourceItems = sourceData.Select(x => x.Item).Distinct().ToList();

			foreach (var item in sourceItems)
			{
				var totalInSource = 0;
				foreach (var sourceItem in sourceData.Where(x => x.Item == item))
					totalInSource += sourceItem.QuantityItem;

				while (totalInSource > 0)
				{
					var uncompleteDataItem = receiveData
						.FirstOrDefault(x => x.Item == item && x.QuantityItem != x.Item.MaxQuantity);
					if (uncompleteDataItem == null)
					{
						result.Add(new BagItemData()
						{
							Item = item,
							QuantityItem = totalInSource,
							Stack = null,
							Blocked = 0
						});
						break;
					}

					if (uncompleteDataItem.Item.MaxQuantity - uncompleteDataItem.QuantityItem > totalInSource)
					{
						uncompleteDataItem.QuantityItem += totalInSource;
						break;
					}
					else
					{
						totalInSource -= uncompleteDataItem.Item.MaxQuantity - uncompleteDataItem.QuantityItem;
						uncompleteDataItem.QuantityItem = uncompleteDataItem.Item.MaxQuantity;
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Вычитание данных
		/// </summary>
		/// <param name="sourceData">Данные источника</param>
		/// <param name="dataToDelete">Данные для вычитания</param>
		/// <returns>Результат</returns>
		internal static List<BagItemData> SubstractData(IEnumerable<BagItemData> sourceData, IEnumerable<BagItemData> dataToDelete)
		{
			if (sourceData == null || dataToDelete == null)
				throw new ArgumentNullException("Не поступили данные для вычитания");
			
			var result = sourceData.ToList();
			if (!dataToDelete.Any())
				return result;

			foreach (var item in dataToDelete.Select(x => x.Item).Distinct())
			{
				var quantityToDelete = 0;
				foreach (var dataItem in dataToDelete.Where(x => x.Item.Id == item.Id))
					quantityToDelete += dataItem.QuantityItem;

				while (quantityToDelete > 0)
				{
					var bagItem = sourceData.LastOrDefault(x => x.Item == item && x.Blocked < x.QuantityItem)
						?? throw new ApplicationException($"Невозможно удалить предмет {item.Name} в количестве {quantityToDelete}");
					if (quantityToDelete >= bagItem.QuantityItem - bagItem.Blocked)
					{
						quantityToDelete -= bagItem.QuantityItem - bagItem.Blocked;
						bagItem.QuantityItem = bagItem.Blocked;

						if (bagItem.QuantityItem == 0)
							result.Remove(bagItem);
					}
					else
					{
						bagItem.QuantityItem -= quantityToDelete;
						break;
					}
				}
			}
			return result;
		}
		/// <summary>
		/// Блокирование данных
		/// </summary>
		/// <param name="sourceData">Данные источника</param>
		/// <param name="dataToBlock">Данные для блокирования</param>
		/// <returns>Результат</returns>

		internal static List<BagItemData> BlockData(IEnumerable<BagItemData> sourceData, IEnumerable<BagItemData> dataToBlock)
		{
			if (sourceData == null || dataToBlock == null)
				throw new ArgumentNullException("Не поступили данные для блокирования");

			foreach (var sourceDataItem in sourceData)
				if (dataToBlock.Any(x => x.Stack == sourceDataItem.Stack))
					sourceDataItem.Blocked += dataToBlock.FirstOrDefault(x => x.Stack == sourceDataItem.Stack).Blocked;

			return sourceData.ToList();
		}

		/// <summary>
		/// Деллокирование данных
		/// </summary>
		/// <param name="sourceData">Данные источника</param>
		/// <param name="dataToUnblock">Данные для блокирования</param>
		/// <returns>Результат</returns>

		internal static List<BagItemData> UnblockData(IEnumerable<BagItemData> sourceData, IEnumerable<BagItemData> dataToUnblock)
		{
			if (sourceData == null || dataToUnblock == null)
				throw new ArgumentNullException("Не поступили данные для блокирования");

			foreach (var sourceDataItem in sourceData)
				if (dataToUnblock.Any(x => x.Stack == sourceDataItem.Stack))
					sourceDataItem.Blocked -= dataToUnblock.FirstOrDefault(x => x.Stack == sourceDataItem.Stack).QuantityItem;

			return sourceData.ToList();
		}
	}
}
