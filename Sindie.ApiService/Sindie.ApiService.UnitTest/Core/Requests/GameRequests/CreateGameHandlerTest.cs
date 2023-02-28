using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.GameRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.GameRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.GameRequests
{
	/// <summary>
	/// Тест для <see cref="CreateGameHandler"/>
	/// </summary>
	[TestClass]
	public class CreateGameHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly ImgFile _imgFile;
		private readonly TextFile _textFile;
		private readonly User _user;
		private readonly Interface _interface;
		private readonly GameRole _mainMasterRole;

		/// <summary>
		/// Конструктор теста для <see cref="CreateGameHandler"/>
		/// </summary>
		public CreateGameHandlerTest(): base()
		{
			_imgFile = ImgFile.CreateForTest();
			_textFile = TextFile.CreateForTest();
			_user = User.CreateForTest(id: UserId);
			_interface = Interface.CreateForTest(id: SystemInterfaces.GameDarkId, name: SystemInterfaces.GameDarkName);
			_mainMasterRole = GameRole.CreateForTest(id: GameRoles.MainMasterRoleId, name: GameRoles.MainMasterRoleName);

			_dbContext = CreateInMemoryContext(
				x => x.AddRange(_imgFile, _textFile, _user, _interface, _mainMasterRole));
		}

		/// <summary>
		/// Тест метода Handle - создания игры
		/// должен создать игру с заполненными полями, списки файлов и UserGame
		/// </summary>
		/// <returns>Юнит</returns>
		[TestMethod]
		public async Task Handle_CreateGame_ShouldReturnUnit()
		{
			var request = new CreateGameCommand()
			{
				AvatarId = _dbContext.ImgFiles.First().Id,
				Name = "definitely unique name",
				Description = "description1",
				TextFiles = new List<Guid>(_dbContext.TextFiles.Select(x => x.Id).ToList()),
				ImgFiles = new List<Guid>(_dbContext.ImgFiles.Select(x => x.Id).ToList())
			};

			var newHandler = new CreateGameHandler(
				_dbContext, AuthorizationService.Object, UserContext.Object, ChangeListServiceMock.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
		
			Assert.AreEqual(1, _dbContext.Games.Count());
			var game = _dbContext.Games.FirstOrDefault();

			Assert.IsNotNull(game);
			Assert.AreEqual(request.Name, game.Name);
			Assert.AreEqual(request.Description, game.Description);
			Assert.AreEqual(request.AvatarId, game.AvatarId);
			
			Assert.AreEqual(1, _dbContext.UserGames
				.Where(x => x.GameId == game.Id
				&& x.UserId == _user.Id
				&& x.InterfaceId == _interface.Id
				&& x.GameRoleId == _mainMasterRole.Id)
				.Count());
		}
	}
}
