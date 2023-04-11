using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.BagRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.BagRequests;
using Witcher.Core.Requests.WeaponTemplateRequests;

namespace Witcher.UnitTest.Core.Requests.BagRequests
{
	[TestClass]
	public class AddItemToBagHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly WeaponTemplate _weaponTemplate;
		private readonly Character _character;

		public AddItemToBagHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_weaponTemplate = WeaponTemplate.CreateForTest(game: _game);
			_character = Character.CreateForTest(game: _game);

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _weaponTemplate, _character));
		}

		[TestMethod]
		public async Task Handle_AddWeapon_ShouldReturnUnit()
		{
			var request = new AddItemToBagCommand
			{
				CharacterId = _character.Id,
				ItemTemplateId = _weaponTemplate.Id,
			};

			var newHandler = new AddItemToBagHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var bag = _dbContext.Characters
				.Include(c => c.Bag)
				.FirstOrDefault(x => x.Id == _character.Id).Bag;

			Assert.IsNotNull(bag);
			Assert.IsTrue(bag.Items.Any());
			var item = bag.Items.First();
			Assert.AreEqual(item.ItemTemplateId, _weaponTemplate.Id);

			var weapon = item as Weapon;
			Assert.IsNotNull(weapon);
			Assert.AreEqual(_weaponTemplate.Durability, weapon.CurrentDurability);
		}
	}
}
