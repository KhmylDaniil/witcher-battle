using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BagRequests.GetBag;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BagRequests
{
	/// <summary>
	/// Тест для <see cref="GetBagHandler"/>
	/// </summary>
	[TestClass]
	public class GetBagHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Instance _instance;
		private readonly ItemTemplate _itemTemplate;
		private readonly Slot _slot;
		private readonly Bag _bag;
		private readonly Item _item;

		/// <summary>
		/// Конструктор для теста<see cref="GetBagHandler"/>
		/// </summary>
		public GetBagHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_instance = Instance.CreateForTest(game: _game);
			_itemTemplate = ItemTemplate.CreateForTest(game: _game, name: "шлем");
			_slot = Slot.CreateForTest(game: _game, name: "голова");

			_item = Item.CreateForTest(
				itemTemplate: _itemTemplate,
				slot: _slot,
				name: "шлем",
				weight: 5,
				maxQuantity: 1);
			_bag = Bag.CreateForTest(
				instance: _instance);
			_bag.BagItems.Add(new BagItem(
				bag: _bag,
				item: _item,
				quantityItem: 1,
				maxQuantityItem: 1,
				stack: 0));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_instance,
				_itemTemplate,
				_slot,
				_bag,
				_item));
		}

		// <summary>
		/// Тест метода Handle получение списка предметов в сумке с фильтрами
		/// по названию предмета, шаблона предмета, слота
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_GetBag_ShouldReturnUnit()
		{
			var request = new GetBagCommand(
				gameId: _game.Id,
				instanceId: _instance.Id,
				bagId: _bag.Id,
				itemName: "шлем",
				itemTemplateName: "шлем",
				slotName: "голова",
				pageSize: 1,
				pageNumber: 1,
				orderBy: null,
				isAscending: false);

			var newHandler = new GetBagHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.TotalCount);

			var resultItem = result.BagItemsList.First();
			Assert.AreEqual(resultItem.ItemId, _item.Id);
			Assert.IsTrue(resultItem.ItemName.Contains("шлем"));
			Assert.AreEqual(resultItem.QuantityItem, 1);
			Assert.AreEqual(resultItem.MaxQuantityItem, 1);
			Assert.AreEqual(resultItem.Stack, 0);
			Assert.AreEqual(resultItem.Blocked, 0);
		}
	}
}
