using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.WeaponTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.WeaponTemplateRequests;

namespace Witcher.UnitTest.Core.Requests.WeaponTemplateRequests
{

	[TestClass]
	public sealed class UpdateAppliedConditionForWeaponTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly WeaponTemplate _weaponTemplate;


		public UpdateAppliedConditionForWeaponTemplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_weaponTemplate = WeaponTemplate.CreateForTest(game: _game);
			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _weaponTemplate));
		}

		[TestMethod]
		public async Task Handle_UpdateApliedCondition_ShouldReturnUnit()
		{
			var createCommand = new UpdateAppliedConditionForWeaponTemplateCommand
			{
				WeaponTemplateId = _weaponTemplate.Id,
				Id = null,
				ApplyChance = 50,
				Condition = Condition.Bleed
			};

			var newHandler = new UpdateAppliedConditionForWeaponTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(createCommand, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.ItemTemplates.Count());
			var weaponTemplate = _dbContext.ItemTemplates.FirstOrDefault() as WeaponTemplate;

			Assert.IsNotNull(weaponTemplate.AppliedConditions);
			Assert.AreEqual(1, weaponTemplate.AppliedConditions.Count);
			var appliedCondition = weaponTemplate.AppliedConditions[0];
			Assert.AreEqual(Condition.Bleed, appliedCondition.Condition);
			Assert.AreEqual(50, appliedCondition.ApplyChance);

			var updateCommand = new UpdateAppliedConditionForWeaponTemplateCommand
			{
				WeaponTemplateId = _weaponTemplate.Id,
				Id = appliedCondition.Id,
				ApplyChance = 30,
				Condition = Condition.Poison
			};

			result = await newHandler.Handle(updateCommand, default);

			Assert.IsNotNull(result);

			Assert.IsNotNull(weaponTemplate.AppliedConditions);
			Assert.AreEqual(1, weaponTemplate.AppliedConditions.Count);
			appliedCondition = weaponTemplate.AppliedConditions[0];
			Assert.AreEqual(Condition.Poison, appliedCondition.Condition);
			Assert.AreEqual(30, appliedCondition.ApplyChance);
		}
	}
}
