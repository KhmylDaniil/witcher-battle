using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Services.Authorization;
using System.Linq;

namespace Sindie.ApiService.UnitTest.Core.Services.Authorization
{
	/// <summary>
	/// Тест для <see cref="AuthorizationService" >
	/// </summary>
	[TestClass]
	public class AuthorizationServiceTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly UserGame _userGame;
		private readonly Battle _instance;

		/// <summary>
		/// Конструктор
		/// </summary>
		public AuthorizationServiceTest()
		{
			_game = Game.CreateForTest();
			_userGame = UserGame.CreateForTest(
				game: _game,
				user: User.CreateForTest(UserContextAsUser.Object.CurrentUserId),
				gameRole: GameRole.CreateForTest(GameRoles.MasterRoleId));
			_instance = Battle.CreateForTest(game: _game);

			_dbContext = CreateInMemoryContext(
				x => x.AddRange(_game, _instance, _userGame));
		}

		/// <summary>
		/// Тест метода RoleGameFilter() - Проверить права доступа для ГлавМастера
		/// - должен возвращать игру
		/// </summary>
		/// <returns>-</returns>
		[TestMethod]
		public void Hash_RoleGameFilter_ShouldReturnGame()
		{
			//Arrange
			var authorizationService = new AuthorizationService(UserContextAsUser.Object);

			//Act
			var result = authorizationService.RoleGameFilter(_dbContext.Games, _game.Id, GameRoles.MasterRoleId);

			//Assert
			Assert.AreEqual(1, result.Count());
			Assert.AreEqual(_game.Id, result.First().Id);
		}

		/// <summary>
		/// Тест метода UserGameFilter() - Проверить права доступа для пользователя игры
		/// - должен возвращать игру
		/// </summary>
		/// <returns>-</returns>
		[TestMethod]
		public void Hash_UserGameFilter_ShouldReturnGame()
		{
			//Arrange
			var authorizationService = new AuthorizationService(UserContextAsUser.Object);

			//Act
			var result = authorizationService.UserGameFilter(_dbContext.Games, _game.Id);

			//Assert
			Assert.AreEqual(1, result.Count());
			Assert.AreEqual(_game.Id, result.First().Id);
		}

		/// <summary>
		/// Тест метода InstanceMasterFilter() - Проверить права доступа к инстансу
		/// - должен возвращать игру
		/// </summary>
		/// <returns>-</returns>
		[TestMethod]
		public void Hash_InstanceMasterFilter_ShouldReturnGame()
		{
			//Arrange
			var authorizationService = new AuthorizationService(UserContextAsUser.Object);

			//Act
			var result = authorizationService.BattleMasterFilter(_dbContext.Battles, _instance.Id);

			//Assert
			Assert.AreEqual(1, result.Count());
			Assert.AreEqual(_instance.Id, result.First().Id);
		}
	}
}

