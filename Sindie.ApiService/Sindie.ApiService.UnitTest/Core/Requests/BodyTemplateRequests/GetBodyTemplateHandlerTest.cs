using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BodyTemplateRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.UnitTest.Core.Requests.BodyTemplateRequests
{
	/// <summary>
	/// Тест для <see cref="GetBodyTemplateHandler"/>
	/// </summary>
	[TestClass]
	public class GetBodyTemplateHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly BodyTemplate _bodyTemplate;
		private readonly BodyTemplatePart _torso;
		private readonly BodyTemplatePart _head;
		private readonly User _user;
		private readonly Game _game;

		/// <summary>
		/// Конструктор для теста <see cref="GetBodyTemplateHandler"/>
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
				createdByUserId: _user.Id);

			_head = BodyTemplatePart.CreateForTest(
				bodyTemplate: _bodyTemplate,
				bodyPartType: BodyPartType.Head,
				hitPenalty: 3,
				damageModifier: 3,
				minToHit: 1,
				maxToHit: 3);

			_torso = BodyTemplatePart.CreateForTest(
				bodyTemplate: _bodyTemplate,
				bodyPartType: BodyPartType.Torso,
				hitPenalty: 1,
				damageModifier: 1,
				minToHit: 4,
				maxToHit: 10);

			_bodyTemplate.BodyTemplateParts = new List<BodyTemplatePart> { _head, _torso };

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game, _user, _bodyTemplate, _torso, _head));
		}

		/// <summary>
		/// Тест метода Handle получение списка шаблонов тела с фильтрами
		/// по названию, дате создания/изменения, создавшему пользователю, названию, названию части шаблона тела
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_GetBodyTemplate_ShouldReturn_GetBodyTemplateResponse()
		{
			var creationMinTime = DateTimeProvider.Object.TimeProvider.AddDays(-1);
			var creationMaxTime = DateTimeProvider.Object.TimeProvider.AddDays(1);
			var modificationMinTime = DateTimeProvider.Object.TimeProvider.AddDays(-1);
			var modificationMaxTime = DateTimeProvider.Object.TimeProvider.AddDays(1);

			var request = new GetBodyTemplateQuery()
			{
				Name = "testName",
				UserName = "Author",
				CreationMinTime = creationMinTime,
				CreationMaxTime = creationMaxTime,
				ModificationMinTime = modificationMinTime,
				ModificationMaxTime = modificationMaxTime,
				BodyPartName = Enum.GetName(BodyPartType.Head),
				PageSize = 2,
				PageNumber = 1,
				OrderBy = null,
				IsAscending = false
		};
				
			var newHandler = new GetBodyTemplateHandler(_dbContext, AuthorizationService.Object, DateTimeProvider.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count());

			var resultItem = result.First();
			Assert.IsTrue(resultItem.Name.Contains(request.Name));
			Assert.IsTrue(resultItem.CreatedOn >= creationMinTime && resultItem.CreatedOn <= creationMaxTime);
			Assert.IsTrue(resultItem.ModifiedOn >= modificationMinTime && resultItem.ModifiedOn <= modificationMaxTime);

			var bodyTemplate = _dbContext.BodyTemplates
				.Include(x => x.BodyTemplateParts)
				.FirstOrDefault(x => x.Id == resultItem.Id);
			Assert.IsNotNull(bodyTemplate);

			var user = _dbContext.Users.FirstOrDefault(x => x.Id == bodyTemplate.CreatedByUserId);
			Assert.IsNotNull(user);
			Assert.IsTrue(user.Name.Contains(request.UserName));

			var bodyPart = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Name == Enum.GetName(BodyPartType.Head));
			Assert.IsNotNull(bodyPart);
		}
	}
}
