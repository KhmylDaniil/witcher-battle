using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.CharacterRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.CharacterRequests;

namespace Witcher.UnitTest.Core.Requests.CharacterRequests
{
	[TestClass]
	public sealed class CreateCharacterHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly User _mainMaster;
		private readonly User _user;
		private readonly Game _game;
		private readonly BodyTemplate _bodyTemplate;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly Ability _ability;
		
		/// <summary>
		/// Конструктор для теста <see cref="DeleteCharacterHandler"/>
		/// </summary>
		public CreateCharacterHandlerTest() : base()
		{
			_mainMaster = User.CreateForTest();
			_user = User.CreateForTest(id: UserId);
			_game = Game.CreateForTest();
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_creatureTemplate = CreatureTemplate.CreateForTest(game: _game, bodyTemplate: _bodyTemplate, hp: 56);
			_ability = Ability.CreateForTest(game: _game);
			_creatureTemplate.Abilities.Add(_ability);

			_game.UserGames.Add(UserGame.CreateForTest(
				user: _mainMaster,
				game: _game,
				gameRole: GameRole.CreateForTest(id: GameRoles.MainMasterRoleId)));

			_game.UserGames.Add(UserGame.CreateForTest(
				user: _user,
				game: _game,
				gameRole: GameRole.CreateForTest(id: GameRoles.PlayerRoleId)));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_mainMaster,
				_user,
				_game,
				_bodyTemplate,
				_creatureTemplate,
				_ability));
		}

		/// <summary>
		/// Тест метода Handle - создание персонажа
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreateCreatureTemplate_ShouldReturnUnit()
		{
			var request = new CreateCharacterCommand()
			{
				CreatureTemplateId = _creatureTemplate.Id,
				Name = "newName",
				Description = "newDescription"
			};

			var newHandler = new CreateCharacterHandler(_dbContext, AuthorizationService.Object, UserContext.Object);

			var createdCharacter = await newHandler.Handle(request, default);

			Assert.IsNotNull(createdCharacter);
			Assert.AreEqual(request.Name, createdCharacter.Name);
			Assert.AreEqual(request.Description, createdCharacter.Description);

			Assert.IsTrue(createdCharacter.Abilities.Any());

			Assert.AreEqual(_creatureTemplate.HP, createdCharacter.HP);

			Assert.IsNotNull(_dbContext.Characters
				.FirstOrDefault(x => x.Id == createdCharacter.Id));
		}

	}
}
