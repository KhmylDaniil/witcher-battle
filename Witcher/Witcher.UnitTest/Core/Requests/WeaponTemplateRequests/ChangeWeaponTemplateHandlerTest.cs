﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Abstractions;
using Witcher.Core.Entities;
using Witcher.Core.Contracts.WeaponTemplateRequests;
using Witcher.Core.Requests.WeaponTemplateRequests;

namespace Witcher.UnitTest.Core.Requests.WeaponTemplateRequests
{
	[TestClass]
	public sealed class ChangeWeaponTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly WeaponTemplate _weaponTemplate;

		public ChangeWeaponTemplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_weaponTemplate = WeaponTemplate.CreateForTest(game: _game);

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _weaponTemplate));
		}

		[TestMethod]
		public async Task Handle_ChangeWeaponTemplate_ShouldReturnUnit()
		{
			var request = new ChangeWeaponTemplateCommand
			{
				Id = _weaponTemplate.Id,
				Name = "name",
				Description = "description",
				Price = 100,
				Weight = 5,
				AttackDiceQuantity = 2,
				DamageModifier = 4,
				Accuracy = 1,
				AttackSkill = Skill.Sword,
				DamageType = DamageType.Bludgeoning,
				Durability = 10,
				Range = null
			};

			var newHandler = new ChangeWeaponTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.ItemTemplates.Count());
			var weaponTemplate = _dbContext.ItemTemplates.FirstOrDefault(x => x.Id == request.Id) as WeaponTemplate;

			Assert.AreEqual(_game.Id, weaponTemplate.GameId);
			Assert.IsNotNull(weaponTemplate.Name);
			Assert.AreEqual(request.Name, weaponTemplate.Name);
			Assert.AreEqual(request.Description, weaponTemplate.Description);
			Assert.AreEqual(request.Price, weaponTemplate.Price);
			Assert.AreEqual(request.Weight, weaponTemplate.Weight);
			Assert.AreEqual(request.AttackDiceQuantity, weaponTemplate.AttackDiceQuantity);
			Assert.AreEqual(request.DamageModifier, weaponTemplate.DamageModifier);
			Assert.AreEqual(request.Accuracy, weaponTemplate.Accuracy);
			Assert.AreEqual(request.AttackSkill, weaponTemplate.AttackSkill);
			Assert.AreEqual(request.DamageType, weaponTemplate.DamageType);
			Assert.AreEqual(request.Durability, weaponTemplate.Durability);
			Assert.IsNull(weaponTemplate.Range);

			Assert.IsNotNull(weaponTemplate.DefensiveSkills);
			Assert.AreEqual(3, weaponTemplate.DefensiveSkills.Count);

			Assert.IsNotNull(weaponTemplate.AppliedConditions);
		}
	}
}
