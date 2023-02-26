using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.UserGameRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.UserGameRequests;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.UserGameRequests
{
	/// <summary>
	/// Тест для <see cref="CreateUserGameHandler"/>
	/// </summary>
	[TestClass]
	public class CreateUserGameHandlerTest: UnitTestBase
	{
		private readonly User _currentUser;
		private readonly User _assignedUser;
		private readonly GameRole _mainMasterRole;
		private readonly GameRole _masterRole;
		private readonly GameRole _playerRole;
		private readonly Interface _interface;
		private readonly Game _gameAsMainMaster;
		private readonly Game _gameAsMaster;
		private readonly IAppDbContext _appDbContext;
		private Guid gameId;

		/// <summary>
		/// Конструктор для <see cref="CreateUserGameHandler"/>
		/// </summary>
		public CreateUserGameHandlerTest()
		{
			_currentUser = User.CreateForTest(id: UserId);
			_assignedUser = User.CreateForTest(id: Guid.NewGuid());
			_mainMasterRole = GameRole.CreateForTest(id: GameRoles.MainMasterRoleId);
			_masterRole = GameRole.CreateForTest(id: GameRoles.MasterRoleId);
			_playerRole = GameRole.CreateForTest(id: GameRoles.PlayerRoleId);
			_interface = Interface.CreateForTest(id: SystemInterfaces.GameDarkId);

			_gameAsMainMaster = Game.CreateForTest();
			_gameAsMainMaster.UserGames.Add(UserGame.CreateForTest(
				user: _currentUser,
				gameRole: _mainMasterRole));

			_gameAsMaster = Game.CreateForTest();
			_gameAsMaster.UserGames.Add(UserGame.CreateForTest(
				user: _currentUser,
				gameRole: _masterRole));

			_appDbContext = CreateInMemoryContext(x => x.AddRange(
				_currentUser,
				_assignedUser,
				_gameAsMainMaster,
				_gameAsMaster,
				_mainMasterRole,
				_masterRole,
				_playerRole,
				_interface));

			AuthorizationServiceWithGameId
				.Setup(x => x.AuthorizedGameFilter(It.IsAny<IQueryable<Game>>(), It.IsAny<Guid>()))
				.Returns(new Func<IQueryable<Game>>(() => _appDbContext.Games.Where(x => x.Id == gameId)));
		}

		/// <summary>
		/// Тест метода Handle добавление пользователя игры главмастером
		/// </summary>
		/// <param name="role">Айди роли</param>
		/// <returns>Юнит</returns>
		[TestMethod]
		[DataRow("8094e0d0-3147-4791-9053-9667cbe117d7")]
		[DataRow("8094e0d0-3148-4791-9053-9667cbe137d8")]
		public async Task Handle_CreateUserGameAsMainMaster_ShouldReturnUnit(string role)
		{
			var roleId = new Guid(role);
			var request = new CreateUserGameCommand()
			{
				UserId = _assignedUser.Id,
				RoleId = roleId
			};

			gameId = _gameAsMainMaster.Id;
			//Arrange
			var newHandler = new CreateUserGameHandler(_appDbContext, AuthorizationServiceWithGameId.Object, UserContext.Object);

			//Act
			var result = await newHandler.Handle(request, default);

			//Assert
			Assert.IsNotNull(result);
			var userGame = _appDbContext.UserGames.FirstOrDefault(x => x.UserId == request.UserId);
			Assert.IsNotNull(userGame);
			Assert.IsTrue(userGame.GameId == gameId);
			Assert.IsTrue(userGame.GameRoleId == request.RoleId);
		}

		/// <summary>
		/// Тест метода Handle добавления пользователя игры(игрока) мастером
		/// </summary>
		/// <returns>Юнит</returns>
		[TestMethod]
		public async Task Handle_CreateUserGameAsMaster_ShouldReturnUnit()
		{
			var request = new CreateUserGameCommand()
			{
				UserId = _assignedUser.Id,
				RoleId = GameRoles.PlayerRoleId
			};

			gameId = _gameAsMaster.Id;

			var newHandler = new CreateUserGameHandler(_appDbContext, AuthorizationServiceWithGameId.Object, UserContext.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			var userGame = _appDbContext.UserGames.FirstOrDefault(x => x.UserId == request.UserId);
			Assert.IsNotNull(userGame);
			Assert.IsTrue(userGame.GameId == gameId);
			Assert.IsTrue(userGame.GameRoleId == request.RoleId);
		}
	}
}
