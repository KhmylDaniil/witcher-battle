using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BagRequests.ChangeBag;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BagRequests.ChangeBag;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BagRequests
{
	/// <summary>
	/// Тест для <see cref="ChangeBagHandler"/>
	/// </summary>
	[TestClass]
	public class ChangeBagHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Instance _instance;
		private readonly ItemTemplate _itemTemplate;
		private readonly Slot _slot;
		private readonly Script _script;
		private readonly Bag _bag;
		private readonly Item _item1;
		private readonly Item _item2;
		private readonly Item _item3;

		/// <summary>
		/// Конструктор для теста<see cref="ChangeBagHandler"/>
		/// </summary>
		public ChangeBagHandlerTest(): base()
		{
			_game = Game.CreateForTest();
			_instance = Instance.CreateForTest(game: _game);
			_itemTemplate = ItemTemplate.CreateForTest(game: _game);
			_slot = Slot.CreateForTest(game: _game);
			_script = Script.CreateForTest(game: _game);
			
			_item1 = Item.CreateForTest(
				itemTemplate: _itemTemplate,
				script: _script,
				slot: _slot,
				name: "меч",
				weight: 5,
				maxQuantity: 1);
			_item2 = Item.CreateForTest(
				itemTemplate: _itemTemplate,
				script: _script,
				slot: _slot,
				name: "бутылка",
				weight: 1,
				maxQuantity: 10);
			_item3 = Item.CreateForTest(
				itemTemplate: _itemTemplate,
				script: _script,
				slot: _slot,
				name: "броня",
				weight: 10,
				maxQuantity: 1);
			_bag = Bag.CreateForTest(
				instance: _instance,
				maxBagSize: 3,
				maxWeight: 30);
			_bag.BagItems.Add(new BagItem(
				bag: _bag,
				item: _item3,
				quantityItem: 1,
				maxQuantityItem: 1,
				stack: 0));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_instance,
				_itemTemplate,
				_slot,
				_script,
				_bag,
				_item1,
				_item2,
				_item3));
		}

		// <summary>
		/// Тест метода Handle, должен изменить предметы в сумке
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ChangeBag_ShouldReturnUnit()
		{
			var request = new ChangeBagCommand(
				gameId: _game.Id,
				instanceId: _instance.Id,
				id: _bag.Id,
				bagItems: new List<ChangeBagRequestItem>
				{
					new ChangeBagRequestItem()
					{
						ItemId = _item1.Id,
						QuantityItem = 1,
						Stack = null,
						Blocked = 0
					},
					new ChangeBagRequestItem()
					{
						ItemId= _item2.Id,
						QuantityItem = 15,
						Stack = 2,
						Blocked = 11
					}
				});

			var newHandler = new ChangeBagHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var bag = _dbContext.Bags.FirstOrDefault(x => x.Id == _bag.Id);
			Assert.IsNotNull(bag);
			Assert.IsNotNull(bag.BagItems);
			Assert.AreEqual(3, bag.BagItems.Count);

			var bagItem1 = bag.BagItems.FirstOrDefault(x => x.Stack == 0);
			Assert.IsNotNull(bagItem1);
			Assert.AreEqual(bagItem1.ItemId, _item1.Id);
			Assert.IsTrue(bagItem1.MaxQuantityItem == 1);
			Assert.IsTrue(bagItem1.QuantityItem == 1);
			Assert.IsTrue(bagItem1.Blocked == 0);

			var bagItem2 = bag.BagItems.FirstOrDefault(x => x.Stack == 1);
			Assert.IsNotNull(bagItem2);
			Assert.AreEqual(bagItem2.ItemId, _item2.Id);
			Assert.IsTrue(bagItem2.MaxQuantityItem == 10);
			Assert.IsTrue(bagItem2.QuantityItem == 5);
			Assert.IsTrue(bagItem2.Blocked == 1);

			var bagItem3 = bag.BagItems.FirstOrDefault(x => x.Stack == 2);
			Assert.IsNotNull(bagItem3);
			Assert.AreEqual(bagItem3.ItemId, _item2.Id);
			Assert.IsTrue(bagItem3.MaxQuantityItem == 10);
			Assert.IsTrue(bagItem3.QuantityItem == 10);
			Assert.IsTrue(bagItem3.Blocked == 10);

			var notification = bag.NotificationDeletedItems.FirstOrDefault();
			Assert.IsNotNull(notification);
			Assert.IsTrue(notification.Message.Contains("броня"));
		}
	}
}
