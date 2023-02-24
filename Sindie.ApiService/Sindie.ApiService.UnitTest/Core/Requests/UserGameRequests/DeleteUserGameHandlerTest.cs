using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.UserGameRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.UserGameRequests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.UserGameRequests
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

			_gameAsMainMaster = Game.CreateForTest();
			_gameAsMainMaster.UserGames = new List<UserGame>
			{
				UserGame.CreateForTest(user: _currentUser, gameRole: _mainMasterRole),
				UserGame.CreateForTest(user: _targetUser, gameRole: _masterRole),
				UserGame.CreateForTest(user: _targetUser, gameRole: _playerRole)
			};
				
			_gameAsMaster = Game.CreateForTest();
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
		[DataRow("8094e0d0-3147-4791-9053-9667cbe117d7")]
		[DataRow("8094e0d0-3148-4791-9053-9667cbe137d8")]
		[TestMethod]
		public async Task Handle_DeleteUserGameAsMainMaster_ShouldReturnUnit(string role)
		{
			var roleId = new Guid(role);

			var request = new DeleteUserGameCommand()
			{
				UserGameId = _gameAsMainMaster.UserGames
				.FirstOrDefault(x => x.GameRoleId == roleId).Id
			};

			gameId = _gameAsMainMaster.Id;
			
			var newHandler = new DeleteUserGameHandler(_appDbContext, AuthorizationServiceWithGameId.Object, UserContext.Object);
			
			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNull(_appDbContext.UserGames
				.FirstOrDefault(x => x.Id == request.UserGameId));
		}

		/// <summary>
		/// Тест метода удаления пользователя игры(игрока) мастером
		/// </summary>
		/// <returns>Юнит</returns>
		[DataRow("8094e0d0-3147-4791-9053-9667cbe117d7")]
		[DataRow("8094e0d0-3148-4791-9053-9667cbe137d8")]
		[TestMethod]
		public async Task Handle_DeleteUserGameAsMaster_ShouldReturnUnit(string role)
		{
			var roleId = new Guid(role);

			var request = new DeleteUserGameCommand()
			{
				UserGameId = _gameAsMaster.UserGames
				.FirstOrDefault(x => x.GameRoleId == roleId).Id
			};

			gameId = _gameAsMaster.Id;

			var newHandler = new DeleteUserGameHandler(_appDbContext, AuthorizationServiceWithGameId.Object, UserContext.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNull(_appDbContext.UserGames
				.FirstOrDefault(x => x.Id == request.UserGameId));
		}
	}
}
