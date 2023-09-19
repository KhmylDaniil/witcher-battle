using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Entities;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Contracts.WeaponTemplateRequests;
using Witcher.Core.Requests.WeaponTemplateRequests;

namespace Witcher.UnitTest.Core.Requests.WeaponTemplateRequests
{
	[TestClass]
	public sealed class GetWeaponTemplateByIdHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly WeaponTemplate _weaponTemplate;

		public GetWeaponTemplateByIdHandlerTest() : base()
		{
			_game = Game.CreateForTest();

			_weaponTemplate = WeaponTemplate.CreateForTest(
				game: _game,
				name: "testName",
				description: "test",
				price: 500,
				weight: 50,
				attackDiceQuantity: 7,
				damageModifier: 5,
				accuracy: -1,
				attackSkill: Skill.Brawling,
				damageType: DamageType.Bludgeoning,
				durability: 10);

			_weaponTemplate.AppliedConditions.Add(new AppliedCondition(Witcher.Core.BaseData.Condition.Poison, 33));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game, _weaponTemplate));
		}

		[TestMethod]
		public async Task Handle_GetWeaponTemplate_ShouldReturn_GetWeaponTemplateByIdResponse()
		{
			var request = new GetWeaponTemplateByIdQuery()
			{
				Id = _weaponTemplate.Id
			};

			var newHandler = new GetWeaponTemplateByIdHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(result.Name, _weaponTemplate.Name);
			Assert.AreEqual(result.Description, _weaponTemplate.Description);
			Assert.IsFalse(result.IsStackable);
			Assert.AreEqual(result.Price, _weaponTemplate.Price);
			Assert.AreEqual(result.Weight, _weaponTemplate.Weight);
			Assert.AreEqual(result.AttackSkill, _weaponTemplate.AttackSkill);
			Assert.AreEqual(result.DamageType, _weaponTemplate.DamageType);
			Assert.AreEqual(result.Accuracy, _weaponTemplate.Accuracy);
			Assert.AreEqual(result.AttackDiceQuantity, _weaponTemplate.AttackDiceQuantity);
			Assert.AreEqual(result.DamageModifier, _weaponTemplate.DamageModifier);
			Assert.AreEqual(result.Durability, _weaponTemplate.Durability);
			Assert.IsNull(result.Range);

			Assert.IsNotNull(result.AppliedConditions);
			Assert.IsTrue(result.AppliedConditions.Count == 1);
			var appliedCondition = result.AppliedConditions[0];
			Assert.AreEqual(Witcher.Core.BaseData.Condition.Poison, appliedCondition.Condition);
			Assert.AreEqual(33, appliedCondition.ApplyChance);
		}
	}
}
