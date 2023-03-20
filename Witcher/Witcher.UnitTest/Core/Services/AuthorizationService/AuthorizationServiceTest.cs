using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Entities;
using Witcher.Core.Services.Authorization;
using System.Linq;

namespace Witcher.UnitTest.Core.Services.Authorization
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
			_game = Game.CreateForTest(id: GameId);
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
			var authorizationService = new AuthorizationService(UserContextAsUser.Object, GameIdService.Object);

			var result = authorizationService.AuthorizedGameFilter(_dbContext.Games, GameRoles.MasterRoleId);

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
			var authorizationService = new AuthorizationService(UserContextAsUser.Object, GameIdService.Object);

			var result = authorizationService.UserGameFilter(_dbContext.Games);

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
			var authorizationService = new AuthorizationService(UserContextAsUser.Object, GameIdService.Object);

			var result = authorizationService.BattleMasterFilter(_dbContext.Battles, _instance.Id);

			Assert.AreEqual(1, result.Count());
			Assert.AreEqual(_instance.Id, result.First().Id);
		}
	}
}

