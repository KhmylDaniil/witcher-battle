using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BattleRequests;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BattleRequests
{
	[TestClass]
	public class DeleteBattleHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Battle _battle;

		/// <summary>
		/// Конструктор для теста <see cref="DeleteBattleHandler"/>
		/// </summary>
		public DeleteBattleHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_battle = Battle.CreateForTest(game: _game);

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _battle));
		}

		/// <summary>
		/// Тест метода Handle - удаление битвы по айди
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_DeleteBattle_ShouldReturnUnit()
		{
			var request = new DeleteBattleCommand()
			{
				Id = _battle.Id,
			};

			var newHandler = new DeleteBattleHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNull(_dbContext.Battles
				.FirstOrDefault(x => x.Id == _battle.Id));
		}
	}
}
