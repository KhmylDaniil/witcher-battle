using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BodyTemplateRequests.GetBodyTemplate;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BodyTemplateRequests
{
	/// <summary>
	/// Тест для <see cref="GetBodyTemplateHandle"/>
	/// </summary>
	[TestClass]
	public class GetBodyTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly BodyTemplate _bodyTemplate;
		private readonly User _user;
		private readonly Game _game;

		/// <summary>
		/// Конструктор для теста <see cref="GetBodyTemplateHandle"/>
		/// </summary>
		public GetBodyTemplateHandlerTest() : base()
		{
			_user = User.CreateForTest(
				id: UserContext.Object.CurrentUserId,
				name: "Author");

			_game = Game.CreateForTest();
			_game.UserGames.Add(
				UserGame.CreateForTest(
					user: _user,
					gameRole: GameRole.CreateForTest(GameRoles.MasterRoleId)));

			_bodyTemplate = BodyTemplate.CreateForTest(
				name: "testName",
				game: _game,
				createdOn: DateTimeProvider.Object.TimeProvider,
				modifiedOn: DateTimeProvider.Object.TimeProvider,
				createdByUserId: _user.Id,
				bodyTemplateParts: new List<BodyTemplatePart>()
				{new BodyTemplatePart("head", 3, 3, 1, 3), new BodyTemplatePart("body", 1, 1, 4, 10)});

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game, _user, _bodyTemplate));
		}

		/// <summary>
		/// Тест метода Handle получение списка шаблонов тела с фильтрами
		/// по названию, дате создания/изменения, создавшему пользователю, названию, названию части шаблона тела
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_GetBodyTemplate_ShouldReturn_GetbodyTemplateResponse()
		{
			var creationMinTime = DateTimeProvider.Object.TimeProvider.AddDays(-1);
			var creationMaxTime = DateTimeProvider.Object.TimeProvider.AddDays(1);
			var modificationMinTime = DateTimeProvider.Object.TimeProvider.AddDays(-1);
			var modificationMaxTime = DateTimeProvider.Object.TimeProvider.AddDays(1);

			var request = new GetBodyTemplateCommand(
				gameId: _game.Id,
				name: "testName",
				userName: "Author",
				creationMinTime: creationMinTime,
				creationMaxTime: creationMaxTime,
				modificationMinTime: modificationMinTime,
				modificationMaxTime: modificationMaxTime,
				bodyPartName: "head",
				pageSize: 2,
				pageNumber: 1,
				orderBy: null,
				isAscending: false);

			var newHandler = new GetBodyTemplateHandler(_dbContext, AuthorizationService.Object, DateTimeProvider.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.TotalCount);

			var resultItem = result.BodyTemplatesList.First();
			Assert.IsTrue(resultItem.Name.Contains(request.Name));
			Assert.IsTrue(resultItem.CreatedOn >= creationMinTime && resultItem.CreatedOn <= creationMaxTime);
			Assert.IsTrue(resultItem.ModifiedOn >= modificationMinTime && resultItem.ModifiedOn <= modificationMaxTime);

			var bodyTemplate = _dbContext.BodyTemplates
				.FirstOrDefault(x => x.Id == resultItem.Id);
			Assert.IsNotNull(bodyTemplate);

			var user = _dbContext.Users.FirstOrDefault(x => x.Id == bodyTemplate.CreatedByUserId);
			Assert.IsNotNull(user);
			Assert.IsTrue(user.Name.Contains(request.UserName));

			var bodyPart = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Name == "head");
			Assert.IsNotNull(bodyPart);
		}
	}
}
