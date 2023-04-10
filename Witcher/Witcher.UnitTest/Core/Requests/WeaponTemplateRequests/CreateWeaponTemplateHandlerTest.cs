using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.AbilityRequests;
using Witcher.Core.Contracts.WeaponTemplateRequests;
using Witcher.Core.Requests.WeaponTemplateRequests;

namespace Witcher.UnitTest.Core.Requests.WeaponTemplateRequests
{
	[TestClass]
	public class CreateWeaponTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;

		public CreateWeaponTemplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game));
		}

		[TestMethod]
		public async Task Handle_CreateWeaponTemplate_ShouldReturnUnit()
		{
			var request = new CreateWeaponTemplateCommand
			{
				Name = "name",
				Description = "description",
				Price = 100,
				Weight = 5,
				AttackDiceQuantity = 2,
				DamageModifier = 4,
				Accuracy = 1,
				AttackSkill = Skill.Melee,
				DamageType = DamageType.Slashing,
				Durability = 10,
				Range = null,
				AppliedConditions = new List<UpdateAttackFormulaCommandItemAppledCondition>
				{
					new UpdateAttackFormulaCommandItemAppledCondition()
					{
						Condition = Condition.Bleed,
						ApplyChance = 50
					}
				}
			};

			var newHandler = new CreateWeaponTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.ItemTemplates.Count());
			var weaponTemplate = _dbContext.ItemTemplates.FirstOrDefault(x => x.Id == result) as WeaponTemplate;

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
			Assert.AreEqual(1, weaponTemplate.AppliedConditions.Count);
			var appliedCondition = weaponTemplate.AppliedConditions.First();
			Assert.AreEqual(Condition.Bleed, appliedCondition.Condition);
			Assert.AreEqual(50, appliedCondition.ApplyChance);
		}
	}
}
