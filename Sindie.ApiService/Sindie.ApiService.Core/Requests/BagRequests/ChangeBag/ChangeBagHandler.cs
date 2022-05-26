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

namespace Sindie.ApiService.Core.Requests.BagRequests.ChangeBag
{
	/// <summary>
	/// Обработчик команды изменения сумки
	/// </summary>
	public class ChangeBagHandler : IRequestHandler<ChangeBagCommand, Unit>
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
		/// Конструктор обработчика команды изменения сумки
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public ChangeBagHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Изменение сумки
		/// </summary>
		/// <param name="request">Команда изменения сумки</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Юнит</returns>
		public async Task<Unit> Handle(ChangeBagCommand request, CancellationToken cancellationToken)
		{
			//проверка наличия игры
			var game = await _authorizationService.BagOwnerOrMasterFilter(
				_appDbContext.Games, request.GameId, request.Id)
				.Include(x => x.Instances.Where(i => i.Id == request.InstanceId))
					.ThenInclude(y => y.Bags.Where(b => b.Id == request.Id))
						.ThenInclude(b => b.NotificationDeletedItems)
				.Include(x => x.Instances.Where(i => i.Id == request.InstanceId))
					.ThenInclude(y => y.Bags.Where(b => b.Id == request.Id))
						.ThenInclude(b => b.BagItems)
				.Include(x => x.ItemTemplates)
					.ThenInclude(i => i.Items)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionNoAccessToEntity<Game>();

			CheckRequest(request, game);

			var bag = game.Instances.SelectMany(x => x.Bags).FirstOrDefault(x => x.Id == request.Id);

			var data = BagItemData.CreateBagItemData(request, game);

			var capacityMessage = Bag.CheckBagCapacity(data, bag.MaxBagSize, bag.MaxWeight);
			if (!string.IsNullOrEmpty(capacityMessage))
				throw new Exception(capacityMessage);
			
			bag.NotifyDeleteItems(data);

			bag.UpdateBagItems(data);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		/// <summary>
		/// Проверить правильность сущностей
		/// </summary>
		/// <param name="request">Данные</param>
		/// <param name="game">Игра</param>
		private static void CheckRequest(ChangeBagCommand request, Game game)
		{
			if (request.BagItems is null)
				throw new ExceptionRequestFieldIncorrectData<ChangeBagCommand>(nameof(request.BagItems));
			var bag = game.Instances.SelectMany(x => x.Bags).FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<Bag>(request.Id);

			if (request.BagItems.Any())
				foreach (var bagItem in request.BagItems)
				{
					var item = game.ItemTemplates.SelectMany(x => x.Items).FirstOrDefault(x => x.Id == bagItem.ItemId)
						?? throw new ExceptionEntityNotFound<Item>(bagItem.ItemId);

					if (bagItem.QuantityItem < 1)
						throw new ExceptionRequestFieldIncorrectData<ChangeBagCommand>(nameof(bagItem.QuantityItem), nameof(item.MaxQuantity));

					if (bagItem.Blocked < 0 || bagItem.Blocked > bagItem.QuantityItem)
						throw new ExceptionRequestFieldIncorrectData<ChangeBagCommand>(nameof(bagItem.Blocked), nameof(bagItem.QuantityItem));

					if (bagItem.Stack != null && (bagItem.Stack < 0
						|| (bag.MaxBagSize != null && bagItem.Stack >= bag.MaxBagSize)))
						throw new ExceptionRequestFieldIncorrectData<ChangeBagCommand>(nameof(bagItem.Stack), nameof(bag.MaxBagSize));
					if (bagItem.Stack != null && request.BagItems.Where(x => x.Stack == bagItem.Stack).Count() != 1)
						throw new ApplicationException("Значения в поле стак повторяются");
				}
		}
	}
}