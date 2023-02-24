using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using System.Linq;
using System.Threading.Tasks;
using Sindie.ApiService.Core.Contracts.AbilityRequests;
using Sindie.ApiService.Core.Requests.AbilityRequests;

namespace Sindie.ApiService.UnitTest.Core.Requests.AbilityRequests
{
	/// <summary>
	/// Тест для <see cref="UpdateAppliedConditionHandler"/>
	/// </summary>
	[TestClass]
	public class UpdateAppliedConditionHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Ability _ability;

		/// <summary>
		/// Конструктор для теста <see cref="UpdateAppliedConditionHandler"/>
		/// </summary>
		public UpdateAppliedConditionHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_ability = Ability.CreateForTest(game: _game);
			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _ability));
		}

		/// <summary>
		/// Тест метода Handle - создание или изменение накладываемого состояния
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_UpdateApliedCondition_ShouldReturnUnit()
		{
			var createCommand = new UpdateAppliedCondionCommand
			{
				AbilityId = _ability.Id,
				Id = null,
				ApplyChance = 50,
				Condition = Condition.Bleed
			};

			var newHandler = new UpdateAppliedConditionHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(createCommand, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.Abilities.Count());
			var ability = _dbContext.Abilities.FirstOrDefault();

			Assert.IsNotNull(ability.AppliedConditions);
			Assert.AreEqual(1, ability.AppliedConditions.Count);
			var appliedCondition = ability.AppliedConditions.First();
			Assert.AreEqual(Condition.Bleed, appliedCondition.Condition);
			Assert.AreEqual(50, appliedCondition.ApplyChance);

			var updateCommand = new UpdateAppliedCondionCommand
			{
				AbilityId = _ability.Id,
				Id = appliedCondition.Id,
				ApplyChance = 30,
				Condition = Condition.Poison
			};

			result = await newHandler.Handle(updateCommand, default);

			Assert.IsNotNull(result);
			
			Assert.IsNotNull(ability.AppliedConditions);
			Assert.AreEqual(1, ability.AppliedConditions.Count);
			appliedCondition = ability.AppliedConditions.First();
			Assert.AreEqual(Condition.Poison, appliedCondition.Condition);
			Assert.AreEqual(30, appliedCondition.ApplyChance);
		}
	}
}
