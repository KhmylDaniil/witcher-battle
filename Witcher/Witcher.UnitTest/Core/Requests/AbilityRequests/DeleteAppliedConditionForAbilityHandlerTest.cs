using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.AbilityRequests;
using System.Linq;
using System.Threading.Tasks;

namespace Witcher.UnitTest.Core.Requests.AbilityRequests
{
	/// <summary>
	/// Тест для <see cref="DeleteAppliedConditionForAbilityHandler"/>
	/// </summary>
	[TestClass]
	public class DeleteAppliedConditionForAbilityHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Ability _ability;
		private readonly AppliedCondition _appliedCondition;

		/// <summary>
		/// Конструктор для теста <see cref="DeleteAppliedConditionForAbilityHandler"/>
		/// </summary>
		public DeleteAppliedConditionForAbilityHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_ability = Ability.CreateForTest(game: _game);
			_appliedCondition = new AppliedCondition(Condition.Bleed, 50);
			_ability.AppliedConditions.Add(_appliedCondition);
			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _ability));
		}

		/// <summary>
		/// Тест метода Handle - удаление накладываемого состояния
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_DeleteApliedCondition_ShouldReturnUnit()
		{
			var command = new DeleteAppliedConditionForAbilityCommand
			{
				AbilityId = _ability.Id,
				Id = _appliedCondition.Id,
			};

			var newHandler = new DeleteAppliedConditionForAbilityHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(command, default);

			Assert.IsNotNull(result);

			var ability = _dbContext.Abilities.FirstOrDefault(x => x.Id == _ability.Id);
			Assert.IsNotNull(ability);

			Assert.IsNull(ability.AppliedConditions
				.Find(x => x.Id == _appliedCondition.Id));

		}
	}
}
