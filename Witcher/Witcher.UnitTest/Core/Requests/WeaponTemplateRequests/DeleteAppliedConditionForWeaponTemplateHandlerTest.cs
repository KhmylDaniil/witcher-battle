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
	public sealed class DeleteAppliedConditionForWeaponTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly WeaponTemplate _weaponTemplate;
		private readonly AppliedCondition _appliedCondition;

		public DeleteAppliedConditionForWeaponTemplateHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_weaponTemplate = WeaponTemplate.CreateForTest(game: _game);
			_appliedCondition = new AppliedCondition(Condition.Bleed, 50);
			_weaponTemplate.AppliedConditions.Add(_appliedCondition);
			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _weaponTemplate));
		}

		[TestMethod]
		public async Task Handle_DeleteApliedConditionForWeaponTemplate_ShouldReturnUnit()
		{
			var command = new DeleteAppliedConditionForWeaponTemplateCommand
			{
				WeaponTemplateId = _weaponTemplate.Id,
				Id = _appliedCondition.Id,
			};

			var newHandler = new DeleteAppliedConditionForWeaponTemplateHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(command, default);

			Assert.IsNotNull(result);

			var weaponTemplate = _dbContext.ItemTemplates.FirstOrDefault(x => x.Id == _weaponTemplate.Id) as WeaponTemplate;
			Assert.IsNotNull(weaponTemplate);

			Assert.IsNull(weaponTemplate.AppliedConditions
				.Find(x => x.Id == _appliedCondition.Id));
		}
	}
}
