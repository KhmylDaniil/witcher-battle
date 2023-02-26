using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.GameRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.GameRequests;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.GameRequests
{
	/// <summary>
	/// Тест для <see cref="ChangeGameHandler"/>
	/// </summary>
	[TestClass]
	public class ChangeGameHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly ImgFile _imgFile;
		private readonly ImgFile _imgFile2;

		/// <summary>
		/// Конструктор теста изменения игры
		/// </summary>
		public ChangeGameHandlerTest(): base()
		{
			_imgFile = ImgFile.CreateForTest();
			_imgFile2 = ImgFile.CreateForTest();

			_game = Game.CreateForTest(
				name: "old name",
				description: "old description",
				avatar: _imgFile);

			_dbContext = CreateInMemoryContext(
				x => x.AddRange(
					_imgFile,
					_imgFile2,
					_game));
		}

		/// <summary>
		/// Тест метода Handle - изменение игры, возвращает юнит
		/// должен изменить поля игры и списки файлов
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ChangeGame_ShouldReturnUnit()
		{
			var request = new ChangeGameCommand()
			{
				Id = _game.Id,
				AvatarId = _imgFile2.Id,
				Name = "absolutely new name",
				Description = "new description",
				TextFiles = null,
				ImgFiles = null
			};

			//Arrange
			var newHandler = new ChangeGameHandler(_dbContext, AuthorizationService.Object, ChangeListServiceMock.Object);

			//Act
			var result = await newHandler.Handle(request, default);

			//Assert
			Assert.IsNotNull(result);

			var game = _dbContext.Games
				.FirstOrDefault(x => x.Id == request.Id);
			Assert.IsNotNull(game);
			Assert.AreEqual(request.Name, game.Name);
			Assert.AreEqual(request.Description, game.Description);
			Assert.AreEqual(request.AvatarId, game.AvatarId);
		}
	}
}
