using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Entities;
using Witcher.Core.Requests.CreatureTemplateRequests;
using Witcher.Core.Contracts.CharacterRequests;
using Witcher.Core.Requests.CharacterRequests;

namespace Witcher.UnitTest.Core.Requests.CharacterRequests
{
	/// <summary>
	/// Тест для <see cref="GetCharactersHandler"/>
	/// </summary>
	[TestClass]
	public class GetCharactersHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly BodyTemplate _bodyTemplate;
		private readonly Game _game;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly User _mainMasterUser;
		private readonly Character _mainMasterCharacter;
		private readonly User _playerUser;
		private readonly Character _playerCharacter;

		/// <summary>
		/// Конструктор для теста <see cref="GetCharactersHandler"/>
		/// </summary>
		public GetCharactersHandlerTest() : base()
		{
			_mainMasterUser = User.CreateForTest(
				id: UserContext.Object.CurrentUserId,
				name: "MainMaster");

			_playerUser = User.CreateForTest(id: Guid.NewGuid(), name: "player");

			_game = Game.CreateForTest();

			var mainMasterUserGame = UserGame.CreateForTest(
					user: _mainMasterUser,
					gameRole: GameRole.CreateForTest(GameRoles.MainMasterRoleId));

			var playerUserGame = UserGame.CreateForTest(
				user: _playerUser,
					gameRole: GameRole.CreateForTest(GameRoles.PlayerRoleId));

			_game.UserGames.Add(mainMasterUserGame);
			_game.UserGames.Add(playerUserGame);

			_bodyTemplate = BodyTemplate.CreateForTest(game: _game, name: "human");

			_creatureTemplate = CreatureTemplate.CreateForTest(
				name: "testName",
				game: _game,
				creatureType: CreatureType.Human,
				bodyTemplate: _bodyTemplate);

			_mainMasterCharacter = Character.CreateForTest(
				game: _game,
				name: "mainMasterCharacter",
				creatureTemlpate: _creatureTemplate);

			_mainMasterCharacter.UserGameCharacters.Add(new UserGameCharacter(_mainMasterCharacter, mainMasterUserGame));

			_playerCharacter = Character.CreateForTest(game: _game,
				name: "playerCharacter",
				creatureTemlpate: _creatureTemplate);

			_playerCharacter.UserGameCharacters.Add(new UserGameCharacter(_playerCharacter, playerUserGame));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_mainMasterUser,
				_playerUser,
				_game,
				_bodyTemplate,
				_creatureTemplate,
				_mainMasterCharacter,
				_playerCharacter));
		}

		/// <summary>
		/// Тест метода Handle получение списка персонажей с фильтрами
		/// по названию, создавшему пользователю
		/// </summary>
		/// <returns></returns>
		[DataRow("playerCharacter", "player")]
		[DataRow("mainMasterCharacter", "MainMaster")]
		[TestMethod]
		public async Task Handle_GetCharacters_ShouldReturn_GetCreatureTemplateResponse(string name, string userName)
		{
			var request = new GetCharactersCommand()
			{
				Name = name,
				UserName = userName
			};

			var newHandler = new GetCharactersHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count());

			var resultItem = result.First();
			Assert.IsTrue(resultItem.Name.Contains(request.Name));
			Assert.IsTrue(resultItem.OwnerName.Contains(request.UserName));
		}
	}
}
