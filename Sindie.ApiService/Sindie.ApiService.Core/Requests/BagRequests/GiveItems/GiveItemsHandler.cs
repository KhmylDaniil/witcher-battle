using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BagRequests.GiveItems
{
	/// <summary>
	/// Обработчик команды передачи предметов из сумки
	/// </summary>
	public class GiveItemsHandler : IRequestHandler<GiveItemsCommand, Unit>
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
		/// Конструктор обработчика команды передачи предметов из сумки
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public GiveItemsHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Передача предметов из сумки
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public async Task<Unit> Handle(GiveItemsCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.BagOwnerOrMasterFilter(
				_appDbContext.Games, request.GameId, request.SourceBagId)
				.Include(x => x.Instances.Where(i => i.Id == request.InstanceId))
					.ThenInclude(y => y.Bags.Where(b => b.Id == request.SourceBagId))
						.ThenInclude(b => b.BagItems)
							.ThenInclude(bi => bi.Item)
				.Include(x => x.Instances.Where(i => i.Id == request.InstanceId))
					.ThenInclude(y => y.Bags.Where(b => b.Id == request.SourceBagId))
						.ThenInclude(b => b.NotificationsTradeRequestSource)
				.Include(x => x.Instances.Where(i => i.Id == request.InstanceId))
					.ThenInclude(y => y.Characters.Where(c => c.Id == request.ReceiveCharacterId))
						.ThenInclude(c => c.Bag)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionNoAccessToEntity<Game>();

			var sourceBag = game.Instances.SelectMany(x => x.Bags).FirstOrDefault(x => x.Id == request.SourceBagId)
				?? throw new ExceptionEntityNotIncluded<Bag>(nameof(Game), request.GameId);
			var receiveCharacter = game.Instances.SelectMany(x => x.Characters).FirstOrDefault(x => x.Id == request.ReceiveCharacterId)
				?? throw new ExceptionEntityNotIncluded<Character>(nameof(Game), request.GameId);

			CheckRequest(request, sourceBag, receiveCharacter);

			var requestData = BagItemData.CreateBagItemsData(request, sourceBag);
			var sourceData = BagItemData.CreateBagItemsData(sourceBag);

			sourceBag.UpdateBagItems(BagItemData.BlockData(sourceData, requestData));

			sourceBag.NotificationsTradeRequestSource
				.Add(new NotificationTradeRequest(sourceBag, requestData, receiveCharacter));

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		/// <summary>
		/// Проверить правильность сущностей
		/// </summary>
		/// <param name="request">Данные</param>
		/// <param name="sourceBag">Сумка-источник</param>
		/// <param name="receiveBag">Сумка-получатель</param>
		private static void CheckRequest(GiveItemsCommand request, Bag sourceBag, Character receiveCharacter)
		{
			if (request.BagItems is null)
				throw new ExceptionRequestFieldIncorrectData<GiveItemsCommand>(nameof(request.BagItems));
			if (sourceBag.BagItems is null)
				throw new ExceptionEntityNotIncluded<BagItem>(nameof(sourceBag));
			if (receiveCharacter.Bag is null)
				throw new ExceptionEntityNotIncluded<Bag>(nameof(receiveCharacter));

			foreach (var bagItem in request.BagItems)
			{
				if (bagItem.ToBlockQuantity < 1)
					throw new ExceptionRequestFieldIncorrectData<GiveItemsCommand>(nameof(bagItem.ToBlockQuantity), "положительному числу");
				_ = sourceBag.BagItems.Select(x => x.Item).FirstOrDefault(x => x.Id == bagItem.ItemId)
					?? throw new ExceptionEntityNotFound<Item>(bagItem.ItemId);
			}

			foreach (var bagItem in request.BagItems)
			{
				var item = sourceBag.BagItems.Select(x => x.Item).FirstOrDefault(x => x.Id == bagItem.ItemId)
					?? throw new ExceptionEntityNotFound<Item>(bagItem.ItemId);

				var sourceBagItem = sourceBag.BagItems.FirstOrDefault(x => x.Stack == bagItem.Stack && x.ItemId == item.Id)
					?? throw new ExceptionEntityNotFound<BagItem>(nameof(bagItem.Stack) + $"{bagItem.Stack}");

				if (bagItem.ToBlockQuantity > sourceBagItem.QuantityItem - sourceBagItem.Blocked)
					throw new ApplicationException($"Невозможно переместить {bagItem.ToBlockQuantity} единиц предмета {sourceBagItem.Item.Name}. Доступно {sourceBagItem.QuantityItem - sourceBagItem.Blocked} единиц");

				if (bagItem.Stack < 0
					|| (sourceBag.MaxBagSize != null && bagItem.Stack >= sourceBag.MaxBagSize))
					throw new ExceptionRequestFieldIncorrectData<GiveItemsCommand>(nameof(bagItem.Stack), nameof(sourceBag.MaxBagSize));
				if (request.BagItems.Where(x => x.Stack == bagItem.Stack).Count() != 1)
					throw new ApplicationException("Значения в поле стак повторяются");
			}
		}
	}
}
