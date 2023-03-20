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
	/// Тест для <see cref="DeleteAppliedConditionHandler"/>
	/// </summary>
	[TestClass]
	public class DeleteAppliedConditionHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Ability _ability;
		private readonly AppliedCondition _appliedCondition;

		/// <summary>
		/// Конструктор для теста <see cref="DeleteAppliedConditionHandler"/>
		/// </summary>
		public DeleteAppliedConditionHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_ability = Ability.CreateForTest(game: _game);
			_appliedCondition = AppliedCondition.CreateAppliedCondition(_ability, Condition.Bleed, 50);
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
			var command = new DeleteAppliedCondionCommand
			{
				AbilityId = _ability.Id,
				Id = _appliedCondition.Id,
			};

			var newHandler = new DeleteAppliedConditionHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(command, default);

			Assert.IsNotNull(result);
			Assert.IsNull(_dbContext.AppliedConditions
				.FirstOrDefault(x => x.Id == _appliedCondition.Id));

		}
	}
}
