using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BagRequests.GiveItems;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BagRequests
{
	/// <summary>
	/// Тест для <see cref="GetGivenItemsHandler"/>
	/// </summary>
	[TestClass]
	public class GetGivenItemsHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Instance _instance;
		private readonly Item _item;
		private readonly NotificationTradeRequest _tradeRequest;

		/// <summary>
		/// Конструктор для теста<see cref="GetGivenItemsHandler"/>
		/// </summary>
		public GetGivenItemsHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_instance = Instance.CreateForTest(game: _game);
			_item = Item.CreateForTest(
				itemTemplate: ItemTemplate.CreateForTest(game: _game),
				slot: Slot.CreateForTest(game: _game),
				name: "шлем",
				weight: 5,
				maxQuantity: 1);
			_tradeRequest = NotificationTradeRequest.CreateForTest(
				sourceBag: Bag.CreateForTest(instance: _instance),
				receiveCharacter: Character.CreateForTest(
					instance: _instance,
					bag: Bag.CreateForTest(instance: _instance)));

			_tradeRequest.BagItems.Add(new NotificationTradeRequestItem(
				item: _item,
				quantity: 1,
				stack: 0));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_instance,
				_item,
				_tradeRequest));
		}

		// <summary>
		/// Тест метода Handle получение списка предлагаемых к передаче предметов
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_GetGivenItems_ShouldReturnUnit()
		{
			var request = new GetGivenItemsCommand(notificationId: _tradeRequest.Id);

			var newHandler = new GetGivenItemsHandler(_dbContext);
			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.BagItemsList);
			Assert.AreEqual(1, result.TotalCount);
			var resultItem = result.BagItemsList.First();
			Assert.IsNotNull(resultItem);
			Assert.AreEqual(resultItem.ItemId, _item.Id);
			Assert.AreEqual(resultItem.ItemName, _item.Name);
			Assert.AreEqual(resultItem.Quantity, 1);
			Assert.AreEqual(resultItem.MaxQuantity, 1);
			Assert.AreEqual(resultItem.TotalWeight, 5);
		}
	}
}
