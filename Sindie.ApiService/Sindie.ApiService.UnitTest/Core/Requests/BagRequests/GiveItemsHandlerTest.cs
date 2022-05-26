using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BagRequests.GiveItems;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BagRequests.GiveItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BagRequests
{
	/// <summary>
	/// Тест для <see cref="GiveItemsHandler"/>
	/// </summary>
	[TestClass]
    public class GiveItemsHandlerTest: UnitTestBase
    {
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Instance _instance;
		private readonly ItemTemplate _itemTemplate;
		private readonly Slot _slot;
		private readonly Bag _sourceBag;
		private readonly Character _receiveCharacter;
		private readonly Item _item;

		/// <summary>
		/// Конструктор для теста <see cref="GiveItemsHandler"/>
		/// </summary>
		public GiveItemsHandlerTest() : base()
		{
			_game = Game.CreateForTest();

			_instance = Instance.CreateForTest(game: _game);

			_itemTemplate = ItemTemplate.CreateForTest(game: _game);
			_slot = Slot.CreateForTest(game: _game);

			_item = Item.CreateForTest(
				itemTemplate: _itemTemplate,
				slot: _slot,
				name: "бутылка",
				weight: 1,
				maxQuantity: 10);
			_sourceBag = Bag.CreateForTest(
				instance: _instance);

			_sourceBag.BagItems.Add(new BagItem(
				bag: _sourceBag,
				item: _item,
				quantityItem: 10,
				maxQuantityItem: 10,
				stack: 1));

			_receiveCharacter = Character.CreateForTest(instance: _instance);
			_receiveCharacter.Bag = Bag.CreateForTest(instance: _instance);

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_instance,
				_itemTemplate,
				_slot,
				_sourceBag,
				_receiveCharacter,
				_item));
		}

		// <summary>
		/// Тест метода Handle, должен пометить предметы как заблокированные и создать уведомление
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_GiveItems_ShouldReturnUnit()
		{
			var request = new GiveItemsCommand(
				gameId: _game.Id,
				instanceId: _instance.Id,
				sourceBagId: _sourceBag.Id,
				receiveCharacterId: _receiveCharacter.Id,
				bagItems: new List<GiveItemsRequestItem>
				{
					new GiveItemsRequestItem()
					{
						ItemId = _item.Id,
						ToBlockQuantity = 4,
						Stack = 1,
					}
				});

			var newHandler = new GiveItemsHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var sourceBag = _dbContext.Bags.FirstOrDefault(x => x.Id == _sourceBag.Id);
			Assert.IsNotNull(sourceBag);
			Assert.IsNotNull(sourceBag.BagItems);
			Assert.AreEqual(1, sourceBag.BagItems.Count);

			var sourceBagItem = sourceBag.BagItems.FirstOrDefault(x => x.Stack == 1);
			Assert.IsNotNull(sourceBagItem);
			Assert.AreEqual(sourceBagItem.ItemId, _item.Id);
			Assert.IsTrue(sourceBagItem.MaxQuantityItem == 10);
			Assert.IsTrue(sourceBagItem.QuantityItem == 10);
			Assert.IsTrue(sourceBagItem.Blocked == 4);

			var notification = sourceBag.NotificationsTradeRequestSource.FirstOrDefault();
			Assert.IsNotNull(notification);
			Assert.IsTrue(notification.Message.Contains("бутылка"));
			Assert.AreEqual(notification.SourceBagId, _sourceBag.Id);
			Assert.AreEqual(notification.ReceiveBagId, _receiveCharacter.Bag.Id);
			Assert.AreEqual(notification.ReceiveCharacterId, _receiveCharacter.Id);
			Assert.IsNotNull(notification.BagItems);
			Assert.IsTrue(notification.BagItems.Count() == 1);
			
			var notifItem = notification.BagItems.First();
			Assert.AreEqual(notifItem.ItemName, _item.Name);
			Assert.AreEqual(notifItem.ItemId, _item.Id);
			Assert.AreEqual(notifItem.Quantity, 4);
			Assert.AreEqual(notifItem.MaxQuantity, _item.MaxQuantity);
			Assert.AreEqual(notifItem.Stack, 1);
			Assert.AreEqual(notifItem.TotalWeight, 4);
		}
	}
}
