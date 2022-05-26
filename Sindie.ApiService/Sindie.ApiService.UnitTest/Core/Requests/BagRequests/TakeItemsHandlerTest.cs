using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BagRequests.TakeItems;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BagRequests.TakeItems;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BagRequests
{
	/// <summary>
	/// Тест для <see cref="TakeItemsHandler"/>
	/// </summary>
	[TestClass]
	public class TakeItemsHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Instance _instance;
		private readonly ItemTemplate _itemTemplate;
		private readonly Slot _slot;
		private readonly Bag _sourceBag;
		private readonly Bag _receiveBag;
		private readonly Item _item1;
		private readonly Item _item2;

		/// <summary>
		/// Конструктор для теста <see cref="TakeItemsHandler"/>
		/// </summary>
		public TakeItemsHandlerTest() : base()
		{
			_game = Game.CreateForTest();

			_instance = Instance.CreateForTest(game: _game);

			_itemTemplate = ItemTemplate.CreateForTest(game: _game);
			_slot = Slot.CreateForTest(game: _game);;

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
				quantityItem: 10,
				maxQuantityItem: 10,
				stack: 1));

			_receiveBag = Bag.CreateForTest(
				instance: _instance,
				character: Character.CreateForTest(instance: _instance),
				maxBagSize: 3,
				maxWeight: 30);

			_receiveBag.BagItems.Add(new BagItem(
				bag: _sourceBag,
				item: _item1,
				quantityItem: 1,
				maxQuantityItem: 1,
				stack: 1));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_instance,
				_itemTemplate,
				_slot,
				_sourceBag,
				_receiveBag,
				_item1,
				_item2));
		}

		// <summary>
		/// Тест метода Handle, должен изменить предметы в сумке
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_TakeItems_ShouldReturnUnit()
		{
			var request = new TakeItemsCommand(
				gameId: _game.Id,
				instanceId: _instance.Id,
				sourceBagId: _sourceBag.Id,
				receiveBagId: _receiveBag.Id,
				bagItems: new List<TakeItemsRequestItem>
				{
					new TakeItemsRequestItem()
					{
						ItemId = _item2.Id,
						QuantityItem = 4,
					}
				});

			var newHandler = new TakeItemsHandler(_dbContext, AuthorizationService.Object);

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
			Assert.IsTrue(sourceBagItem.QuantityItem == 6);

			var receiveBag = _dbContext.Bags.FirstOrDefault(x => x.Id == _receiveBag.Id);
			Assert.IsNotNull(receiveBag);
			Assert.IsNotNull(receiveBag.BagItems);
			Assert.AreEqual(2, receiveBag.BagItems.Count);

			var receiveBagItem1 = receiveBag.BagItems.FirstOrDefault(x => x.Stack == 0);
			Assert.IsNotNull(receiveBagItem1);
			Assert.AreEqual(receiveBagItem1.ItemId, _item2.Id);
			Assert.IsTrue(receiveBagItem1.MaxQuantityItem == 10);
			Assert.IsTrue(receiveBagItem1.QuantityItem == 4);

			var receiveBagItem2 = receiveBag.BagItems.FirstOrDefault(x => x.Stack == 1);
			Assert.IsNotNull(receiveBagItem2);
			Assert.AreEqual(receiveBagItem2.ItemId, _item1.Id);
			Assert.IsTrue(receiveBagItem2.MaxQuantityItem == 1);
			Assert.IsTrue(receiveBagItem2.QuantityItem == 1);

			var notification = sourceBag.NotificationDeletedItems.FirstOrDefault();
			Assert.IsNotNull(notification);
			Assert.IsTrue(notification.Message.Contains("бутылка"));
		}
	}
}
