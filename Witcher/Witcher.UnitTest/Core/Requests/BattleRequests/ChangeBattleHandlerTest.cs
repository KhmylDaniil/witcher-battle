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
	public sealed class ChangeBattleHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Battle _battle;
		private readonly ImgFile _imgFile;

		/// <summary>
		/// Конструктор для теста <see cref="CreateBattleHandler"/>
		/// </summary>
		public ChangeBattleHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_battle = Battle.CreateForTest(game: _game);
			_imgFile = ImgFile.CreateForTest();

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_battle,
				_imgFile));
		}

		/// <summary>
		/// Тест метода Handle - изменение боя
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ChangeBattle_ShouldReturnUnit()
		{
			var request = new ChangeBattleCommand()
			{
				Id = _battle.Id,
				ImgFileId = _imgFile.Id,
				Name = "newBattle",
				Description = "newDescription"
			};

			var newHandler = new ChangeBattleHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			var instance = _dbContext.Battles.FirstOrDefault();
			Assert.IsNotNull(instance);
			Assert.IsTrue(_dbContext.Battles.Count() == 1);

			Assert.AreEqual("newBattle", instance.Name);
			Assert.AreEqual("newDescription", instance.Description);
			Assert.AreEqual(instance.ImgFileId, _imgFile.Id);
			Assert.AreEqual(instance.GameId, _game.Id);
		}
	}
}
