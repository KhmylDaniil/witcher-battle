using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.BattleRequests;
using System.Linq;
using System.Threading.Tasks;

namespace Witcher.UnitTest.Core.Requests.BattleRequests
{
	[TestClass]
	public sealed class GetBattleHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Battle _battle;

		/// <summary>
		/// Конструктор для теста <see cref="GetBattleHandler"/>
		/// </summary>
		public GetBattleHandlerTest() : base()
		{

			_game = Game.CreateForTest();

			_battle = Battle.CreateForTest(
				game: _game,
				name: "test",
				description: "description");

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _battle));
		}

		/// <summary>
		/// Тест метода Handle получение списка битв с фильтрами
		/// по названию/описанию
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_GetAbility_ShouldReturn_GetAbilityResponse()
		{
			var request = new GetBattleQuery()
			{
				Name = "test",
				Description = "description"
			};

			var newHandler = new GetBattleHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count());

			var resultItem = result.First();

			var battle = _dbContext.Battles
				.FirstOrDefault(x => x.Id == resultItem.Id);
			Assert.IsNotNull(battle);
			Assert.IsTrue(battle.Name.Contains(request.Name));
			Assert.IsTrue(battle.Description.Contains(request.Description));
		}
	}
}
