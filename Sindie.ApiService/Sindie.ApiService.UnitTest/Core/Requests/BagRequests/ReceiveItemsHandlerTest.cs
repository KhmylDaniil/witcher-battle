using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BagRequests.GiveItems;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BagRequests
{
	/// <summary>
	/// Тест для <see cref="ReceiveItemsHandler"/>
	/// </summary>
	[TestClass]
	public class ReceiveItemsHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Instance _instance;
		private readonly ItemTemplate _itemTemplate;
		private readonly Slot _slot;
		private readonly Bag _sourceBag;
		private readonly Bag _receiveBag;
		private readonly Character _receiveCharacter;
		private readonly Item _item1;
		private readonly Item _item2;
		private readonly NotificationTradeRequest _tradeRequest;

		/// <summary>
		/// Конструктор для теста <see cref="ReceiveItemsHandler"/>
		/// </summary>
		public ReceiveItemsHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_instance = Instance.CreateForTest(game: _game);
			_itemTemplate = ItemTemplate.CreateForTest(game: _game);
			_slot = Slot.CreateForTest(game: _game);

			_item1 = Item.CreateForTest(
				itemTemplate: _itemTemplate,
				slot: _slot,
				name: "меч",
				weight: 5,
				maxQuantity: 1);
			_item2 = Item.CreateForTest(
				itemTemplate: _itemTemplate,
				slot: _slot,
				name: "бутылка",
				weight: 1,
				maxQuantity: 10);
			_sourceBag = Bag.CreateForTest(
				instance: _instance,
				maxBagSize: 3,
				maxWeight: 30);

			_sourceBag.BagItems.Add(new BagItem(
				bag: _sourceBag,
				item: _item2,
				quantityItem: 9,
				maxQuantityItem: 10,
				stack: 1,
				blocked: 5));

			_receiveBag = Bag.CreateForTest(
				instance: _instance,
				maxBagSize: 3,
				maxWeight: 30);
			_receiveBag.BagItems.Add(new BagItem(
				bag: _sourceBag,
				item: _item1,
				quantityItem: 1,
				maxQuantityItem: 1,
				stack: 0));
			_receiveBag.BagItems.Add(new BagItem(
				bag: _sourceBag,
				item: _item2,
				quantityItem: 7,
				maxQuantityItem: 10,
				stack: 1));

			_receiveCharacter = Character.CreateForTest(
				instance: _instance,
				bag: _receiveBag);

			_tradeRequest = NotificationTradeRequest.CreateForTest(
				sourceBag: _sourceBag,
				receiveCharacter: _receiveCharacter);

			_tradeRequest.BagItems.Add(new NotificationTradeRequestItem(
				item: _item2,
				quantity: 4,
				stack: 1));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_instance,
				_itemTemplate,
				_slot,
				_sourceBag,
				_receiveBag,
				_item1,
				_item2,
				_tradeRequest));
		}

		// <summary>
		/// Тест метода Handle, должен передать предметы
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ReceiveItems_ShouldReturnUnit()
		{
			var request = new ReceiveItemsCommand(
				notificationId: _tradeRequest.Id,
				consent: true);

			var newHandler = new ReceiveItemsHandler(_dbContext);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var sourceBag = _dbContext.Bags.FirstOrDefault(x => x.Id == _sourceBag.Id);
			Assert.IsNotNull(sourceBag);
			Assert.IsNotNull(sourceBag.BagItems);
			Assert.AreEqual(1, sourceBag.BagItems.Count);

			var sourceBagItem = sourceBag.BagItems.FirstOrDefault(x => x.Stack == 1);
			Assert.IsNotNull(sourceBagItem);
			Assert.AreEqual(sourceBagItem.ItemId, _item2.Id);
			Assert.IsTrue(sourceBagItem.MaxQuantityItem == 10);
			Assert.IsTrue(sourceBagItem.QuantityItem == 5);
			Assert.IsTrue(sourceBagItem.Blocked == 1);

			var receiveBag = _dbContext.Bags.FirstOrDefault(x => x.Id == _receiveBag.Id);
			Assert.IsNotNull(receiveBag);
			Assert.IsNotNull(receiveBag.BagItems);
			Assert.AreEqual(3, receiveBag.BagItems.Count);

			var receiveBagItem1 = receiveBag.BagItems.FirstOrDefault(x => x.Stack == 0);
			Assert.IsNotNull(receiveBagItem1);
			Assert.AreEqual(receiveBagItem1.ItemId, _item1.Id);
			Assert.IsTrue(receiveBagItem1.MaxQuantityItem == 1);
			Assert.IsTrue(receiveBagItem1.QuantityItem == 1);

			var receiveBagItem2 = receiveBag.BagItems.FirstOrDefault(x => x.Stack == 1);
			Assert.IsNotNull(receiveBagItem2);
			Assert.AreEqual(receiveBagItem2.ItemId, _item2.Id);
			Assert.IsTrue(receiveBagItem2.MaxQuantityItem == 10);
			Assert.IsTrue(receiveBagItem2.QuantityItem == 10);

			var receiveBagItem3 = receiveBag.BagItems.FirstOrDefault(x => x.Stack == 2);
			Assert.IsNotNull(receiveBagItem3);
			Assert.AreEqual(receiveBagItem3.ItemId, _item2.Id);
			Assert.IsTrue(receiveBagItem3.MaxQuantityItem == 10);
			Assert.IsTrue(receiveBagItem3.QuantityItem == 1);

			Assert.IsTrue(!_dbContext.NotificationsTradeRequest.Any(x => x.Id == _tradeRequest.Id));
		}
	}
}
