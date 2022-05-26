using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.CharacterTemplateRequests.GetCharacterTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.CharacterTemplateRequests.GetCharacterTemplate;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.CharacterTemplateRequests
{
	/// <summary>
	/// Тест для <see cref="GetCharacterTemplateHandler"/>
	/// </summary>
	[TestClass]
	public class GetCharacterTemplateHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly CharacterTemplate _characterTemplate;
		private readonly User _user;
		private readonly Game _game;

		/// <summary>
		/// Конструктор для теста <see cref="GetCharacterTemplateHandlerTest"/>
		/// </summary>
		public GetCharacterTemplateHandlerTest(): base()
		{
			_user = User.CreateForTest(
				id: UserContext.Object.CurrentUserId,
				name: "Author");

			_game = Game.CreateForTest();
			_game.UserGames.Add(
				UserGame.CreateForTest(
					user: _user,
					gameRole: GameRole.CreateForTest(GameRoles.MasterRoleId)));

			_characterTemplate = CharacterTemplate.CreateForTest(
				name: "testName",
				game: _game,
				createdOn: DateTimeProvider.Object.TimeProvider,
				modifiedOn: DateTimeProvider.Object.TimeProvider,
				createdByUserId: _user.Id);
			_characterTemplate.Characters.Add(
				Character.CreateForTest(
					characterTemplate: _characterTemplate,
					name: "character",
					instance: Instance.CreateForTest(game: _game)));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game, _user, _characterTemplate));
		}

		/// <summary>
		/// Тест метода Handle получение списка шаблонов персонажа с фильтрами
		/// по названию, дате создания/изменения, создавшему пользователю, имени персонажа
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_GetCharacterTemplate_ShouldReturn_GetCharacterTemplateResponse()
		{
			var creationMinTime = DateTimeProvider.Object.TimeProvider.AddDays(-1);
			var creationMaxTime = DateTimeProvider.Object.TimeProvider.AddDays(1);
			var modificationMinTime = DateTimeProvider.Object.TimeProvider.AddDays(-1);
			var modificationMaxTime = DateTimeProvider.Object.TimeProvider.AddDays(1);

			var request = new GetCharacterTemplateQuery()
			{
				GameId = _game.Id,
				Name = "testName",
				CharacterName = "character",
				AuthorName = "Author",
				CreationMaxTime = creationMaxTime,
				CreationMinTime = creationMinTime,
				ModificationMaxTime = modificationMaxTime,
				ModificationMinTime = modificationMinTime,
				PageSize = 1,
				PageNumber = 1
			};

			var newHandler = new GetCharacterTemplateHandler(_dbContext, AuthorizationService.Object, DateTimeProvider.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.TotalCount);

			var resultItem = result.CharacterTemplatesList.First();
			Assert.IsTrue(resultItem.Name.Contains(request.Name));
			Assert.IsTrue(resultItem.CreatedOn >= creationMinTime && resultItem.CreatedOn <= creationMaxTime);
			Assert.IsTrue(resultItem.ModifiedOn >= modificationMinTime && resultItem.ModifiedOn <= modificationMaxTime);

			var characterTemplate = _dbContext.CharacterTemplates
				.FirstOrDefault(x => x.Id == resultItem.Id);
			Assert.IsNotNull(characterTemplate);
			
			var user = _dbContext.Users.FirstOrDefault(x => x.Id == characterTemplate.CreatedByUserId);
			Assert.IsNotNull(user);
			Assert.IsTrue(user.Name.Contains(request.AuthorName));

			var character = _dbContext.Characters
				.FirstOrDefault(x => x.CharacterTemplateId == characterTemplate.Id);
			Assert.IsNotNull(character);
			Assert.IsTrue(character.Name.Contains(request.CharacterName));
		}
	}
}
