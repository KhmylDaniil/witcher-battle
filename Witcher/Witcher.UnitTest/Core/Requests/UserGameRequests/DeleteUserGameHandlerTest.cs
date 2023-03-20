using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.UserGameRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.UserGameRequests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Witcher.UnitTest.Core.Requests.UserGameRequests
{
	/// <summary>
	/// Тест для <see cref="DeleteUserGameHandler"/>
	/// </summary>
	[TestClass]
	public class DeleteUserGameHandlerTest: UnitTestBase
	{
		private readonly User _currentUser;
		private readonly User _targetUser;
		private readonly GameRole _mainMasterRole;
		private readonly GameRole _masterRole;
		private readonly GameRole _playerRole;
		private readonly Game _gameAsMainMaster;
		private readonly Game _gameAsMaster;
		private readonly IAppDbContext _appDbContext;
		private Guid gameId;

		/// <summary>
		/// Конструктор для <see cref="DeleteUserGameHandler"/>
		/// </summary>
		public DeleteUserGameHandlerTest()
		{
			_currentUser = User.CreateForTest(id: UserId);
			_targetUser = User.CreateForTest(id: Guid.NewGuid());
			_mainMasterRole = GameRole.CreateForTest(id: GameRoles.MainMasterRoleId);
			_masterRole = GameRole.CreateForTest(id: GameRoles.MasterRoleId);
			_playerRole = GameRole.CreateForTest(id: GameRoles.PlayerRoleId);

			_gameAsMainMaster = Game.CreateForTest(name: "gameAsMainMaster");
			_gameAsMainMaster.UserGames = new List<UserGame>
			{
				UserGame.CreateForTest(user: _currentUser, gameRole: _mainMasterRole),
				UserGame.CreateForTest(user: _targetUser, gameRole: _masterRole),
			};
				
			_gameAsMaster = Game.CreateForTest(name: "gameAsMaster");
			_gameAsMaster.UserGames = new List<UserGame>
			{
				UserGame.CreateForTest(user: _currentUser, gameRole: _masterRole),
				UserGame.CreateForTest(user: _targetUser, gameRole: _playerRole)
			};

			_appDbContext = CreateInMemoryContext(x => x.AddRange(
				_currentUser,
				_targetUser,
				_gameAsMainMaster,
				_gameAsMaster,
				_mainMasterRole,
				_masterRole,
				_playerRole));

			AuthorizationServiceWithGameId
				.Setup(x => x.UserGameFilter(It.IsAny<IQueryable<Game>>()))
				.Returns(new Func<IQueryable<Game>>(() => _appDbContext.Games.Where(x => x.Id == gameId)));
		}

		/// <summary>
		/// Тест метода Handle для удаления пользователя игры главмастером
		/// </summary>
		/// <param name="role">Айди роли</param>
		/// <returns>Юнит</returns>
		[TestMethod]
		public async Task Handle_DeleteUserGameAsMainMaster_ShouldReturnUnit()
		{
			var request = new DeleteUserGameCommand()
			{
				UserId = _targetUser.Id
			};

			gameId = _gameAsMainMaster.Id;

			var newHandler = new DeleteUserGameHandler(_appDbContext, AuthorizationServiceWithGameId.Object, UserContext.Object);
			
			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNull(_appDbContext.UserGames
				.FirstOrDefault(x => x.UserId == request.UserId && x.GameId == _gameAsMainMaster.Id));
		}

		/// <summary>
		/// Тест метода удаления пользователя игры(игрока) мастером
		/// </summary>
		/// <returns>Юнит</returns>
		[TestMethod]
		public async Task Handle_DeleteUserGameAsMaster_ShouldReturnUnit()
		{
			var request = new DeleteUserGameCommand()
			{
				UserId = _targetUser.Id
			};

			gameId = _gameAsMaster.Id;

			var newHandler = new DeleteUserGameHandler(_appDbContext, AuthorizationServiceWithGameId.Object, UserContext.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNull(_appDbContext.UserGames
				.FirstOrDefault(x => x.UserId == request.UserId && x.GameId == _gameAsMaster.Id));
		}
	}
}
