using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.BattleRequests;
using System.Linq;
using System.Threading.Tasks;

namespace Witcher.UnitTest.Core.Requests.BattleRequests
{
	/// <summary>
	/// Тест для <see cref="CreateBattleHandler"/>
	/// </summary>
	[TestClass]
	public class CreateBattleHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly ImgFile _imgFile;

		/// <summary>
		/// Конструктор для теста <see cref="CreateBattleHandler"/>
		/// </summary>
		public CreateBattleHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_imgFile = ImgFile.CreateForTest();
	
			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_imgFile));
		}

		/// <summary>
		/// Тест метода Handle - создание боя
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreateBattle_ShouldReturnUnit()
		{
			var request = new CreateBattleCommand()
			{
				ImgFileId = _imgFile.Id,
				Name = "battle",
				Description = "description"
			};

			var newHandler = new CreateBattleHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			var instance = _dbContext.Battles.FirstOrDefault();
			Assert.IsNotNull(instance);
			Assert.IsTrue(_dbContext.Battles.Count() == 1);

			Assert.AreEqual("battle", instance.Name);
			Assert.AreEqual("description", instance.Description);
			Assert.AreEqual(instance.ImgFileId, _imgFile.Id);
			Assert.AreEqual(instance.GameId, _game.Id);
		}
	}
}
