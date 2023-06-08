using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.ItemRequests;
using Witcher.Core.Drafts.BodyTemplateDrafts;
using Witcher.Core.Entities;
using Witcher.Core.Requests.ItemRequests;

namespace Witcher.UnitTest.Core.Requests.BagRequests
{
	[TestClass]
	public class CreateItemHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly WeaponTemplate _weaponTemplate;
		private readonly WeaponTemplate _weaponTemplateStackable;
		private readonly Character _character;

		private readonly ArmorTemplate _armorTemplate;
		private readonly BodyTemplate _bodyTemplate;

		public CreateItemHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_weaponTemplate = WeaponTemplate.CreateForTest(game: _game);
			_weaponTemplateStackable = WeaponTemplate.CreateForTest(game: _game);
			_weaponTemplateStackable.IsStackable = true;
			_character = Character.CreateForTest(game: _game);

			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_bodyTemplate.CreateBodyTemplateParts(CreateBodyTemplatePartsDraft.CreateBodyPartsDraft());

			_armorTemplate = ArmorTemplate.CreateForTest(game: _game, bodyTemplate: _bodyTemplate, armor: 5);
			_armorTemplate.BodyTemplateParts.AddRange(_bodyTemplate.BodyTemplateParts);

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _weaponTemplate, _weaponTemplateStackable, _character, _bodyTemplate, _armorTemplate));
		}

		[TestMethod]
		public async Task Handle_AddWeapon_ShouldReturnUnit()
		{
			var request = new CreateItemCommand
			{
				CharacterId = _character.Id,
				ItemTemplateId = _weaponTemplate.Id,
				Quantity = 1,
			};

			var newHandler = new CreateItemHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var character = _dbContext.Characters
				.FirstOrDefault(x => x.Id == _character.Id);

			Assert.IsNotNull(character);
			Assert.IsTrue(character.Items.Any());
			var item = character.Items.First();
			Assert.AreEqual(item.ItemTemplateId, _weaponTemplate.Id);

			var weapon = item as Weapon;
			Assert.IsNotNull(weapon);
			Assert.AreEqual(_weaponTemplate.Durability, weapon.CurrentDurability);
		}

		[TestMethod]
		public async Task Handle_AddTwoWeapons_ShouldReturnUnit()
		{
			var request = new CreateItemCommand
			{
				CharacterId = _character.Id,
				ItemTemplateId = _weaponTemplate.Id,
				Quantity = 2
			};

			var newHandler = new CreateItemHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var character = _dbContext.Characters
				.FirstOrDefault(x => x.Id == _character.Id);

			Assert.IsNotNull(character);
			Assert.IsTrue(character.Items.Any());

			Assert.IsTrue(character.Items.Count == 2);

			foreach(var item in character.Items)
			{
				Assert.AreEqual(item.ItemTemplateId, _weaponTemplate.Id);

				var weapon = item as Weapon;
				Assert.IsNotNull(weapon);
				Assert.AreEqual(_weaponTemplate.Durability, weapon.CurrentDurability);
				Assert.AreEqual(1, weapon.Quantity);
			}
		}

		[TestMethod]
		public async Task Handle_AddTwoStackableWeapons_ShouldReturnUnit()
		{
			var request = new CreateItemCommand
			{
				CharacterId = _character.Id,
				ItemTemplateId = _weaponTemplateStackable.Id,
				Quantity = 2
			};

			var newHandler = new CreateItemHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var character = _dbContext.Characters
				.FirstOrDefault(x => x.Id == _character.Id);

			Assert.IsNotNull(character);
			Assert.IsTrue(character.Items.Any());

			var item = character.Items.SingleOrDefault();

			Assert.IsNotNull(item);
			Assert.AreEqual(item.ItemTemplateId, _weaponTemplateStackable.Id);

			var weapon = item as Weapon;
			Assert.IsNotNull(weapon);
			Assert.AreEqual(_weaponTemplateStackable.Durability, weapon.CurrentDurability);
			Assert.AreEqual(2, weapon.Quantity);
		}

		[TestMethod]
		public async Task Handle_AddArmor_ShouldReturnUnit()
		{
			var request = new CreateItemCommand
			{
				CharacterId = _character.Id,
				ItemTemplateId = _armorTemplate.Id,
				Quantity = 1,
			};

			var newHandler = new CreateItemHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var character = _dbContext.Characters
				.FirstOrDefault(x => x.Id == _character.Id);

			Assert.IsNotNull(character);
			Assert.IsTrue(character.Items.Any());
			var item = character.Items.First();
			Assert.AreEqual(item.ItemTemplateId, _armorTemplate.Id);

			var armor = item as Armor;
			Assert.IsNotNull(armor);
			Assert.IsNotNull(armor.ArmorParts);

			Assert.IsTrue(armor.ArmorParts.Count == 6);
			
			foreach(var part in armor.ArmorParts)
			{
				Assert.AreEqual(_armorTemplate.Armor, part.CurrentArmor);
			}
		}
	}
}
