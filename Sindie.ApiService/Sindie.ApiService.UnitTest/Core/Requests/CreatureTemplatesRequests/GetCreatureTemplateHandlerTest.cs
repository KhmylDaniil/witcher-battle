using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.GetCreatureTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.CreatureTemplatesRequests
{
	/// <summary>
	/// Тест для <see cref="GetCreatureTemplateHandler"/>
	/// </summary>
	[TestClass]
	public class GetCreatureTemplateHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly BodyTemplate _bodyTemplate;
		private readonly User _user;
		private readonly Game _game;
		private readonly Condition _condition;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly CreatureTemplatePart _creatureTemplatePart;
		private readonly BodyPartType _bodyPartType;
		private readonly Ability _ability;
		private readonly Parameter _parameter;

		/// <summary>
		/// Конструктор для теста <see cref="GetCreatureTemplateHandler"/>
		/// </summary>
		public GetCreatureTemplateHandlerTest() : base()
		{
			_user = User.CreateForTest(
				id: UserContext.Object.CurrentUserId,
				name: "Author");

			_game = Game.CreateForTest();
			_game.UserGames.Add(
				UserGame.CreateForTest(
					user: _user,
					gameRole: GameRole.CreateForTest(GameRoles.MasterRoleId)));

			_bodyPartType = BodyPartType.CreateForTest();

			_parameter = Parameter.CreateForTest(game: _game);
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game, name: "human");
			_condition = Condition.CreateForTest(
				game: _game,
				name: Conditions.BleedName);

			_creatureTemplate = CreatureTemplate.CreateForTest(
				name: "testName",
				game: _game,
				type: "type",
				bodyTemplate: _bodyTemplate,
				createdOn: DateTimeProvider.Object.TimeProvider,
				modifiedOn: DateTimeProvider.Object.TimeProvider,
				createdByUserId: _user.Id);

			_creatureTemplatePart = CreatureTemplatePart.CreateForTest(
				creatureTemplate: _creatureTemplate,
				bodyPartType: _bodyPartType,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 1,
				maxToHit: 1,
				armor: 0);

			_creatureTemplate.CreatureTemplateParts.Add(_creatureTemplatePart);

			_ability = Ability.CreateForTest(
				creatureTemplate: _creatureTemplate,
				attackParameter: _parameter);
			_ability.AppliedConditions.Add(new AppliedCondition(
				ability: _ability,
				condition: _condition,
				applyChance: 100));
				
			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_user,
				_game,
				_bodyTemplate,
				_bodyPartType,
				_condition,
				_creatureTemplate,
				_ability));
		}

		/// <summary>
		/// Тест метода Handle получение списка шаблонов существа с фильтрами
		/// по названию, дате создания/изменения, создавшему пользователю, названию, названию шаблона телаб названию части тела, названию применяемого состояния
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_GetCreatureTemplate_ShouldReturn_GetCreatureTemplateResponse()
		{
			var creationMinTime = DateTimeProvider.Object.TimeProvider.AddDays(-1);
			var creationMaxTime = DateTimeProvider.Object.TimeProvider.AddDays(1);
			var modificationMinTime = DateTimeProvider.Object.TimeProvider.AddDays(-1);
			var modificationMaxTime = DateTimeProvider.Object.TimeProvider.AddDays(1);

			var request = new GetCreatureTemplateCommand(
				gameId: _game.Id,
				name: "testName",
				userName: "Author",
				type: "type",
				creationMinTime: creationMinTime,
				creationMaxTime: creationMaxTime,
				modificationMinTime: modificationMinTime,
				modificationMaxTime: modificationMaxTime,
				bodyTemplateName: "human",
				bodyPartTypeId: BodyPartTypes.VoidId,
				conditionName: Conditions.BleedName,
				pageSize: 2,
				pageNumber: 1,
				orderBy: null,
				isAscending: false);

			var newHandler = new GetCreatureTemplateHandler(_dbContext, AuthorizationService.Object, DateTimeProvider.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.TotalCount);

			var resultItem = result.CreatureTemplatesList.First();
			Assert.IsTrue(resultItem.Name.Contains(request.Name));
			Assert.IsTrue(resultItem.Type.Contains(request.Type));
			Assert.IsTrue(resultItem.BodyTemplateName.Contains(request.BodyTemplateName));
			Assert.IsTrue(resultItem.CreatedOn >= creationMinTime && resultItem.CreatedOn <= creationMaxTime);
			Assert.IsTrue(resultItem.ModifiedOn >= modificationMinTime && resultItem.ModifiedOn <= modificationMaxTime);

			var creatureTemplate = _dbContext.CreatureTemplates
				.Include(x => x.CreatureTemplateParts)
					.ThenInclude(x => x.BodyPartType)
				.Include(x => x.Abilities)
					.ThenInclude(x => x.AppliedConditions)
					.ThenInclude(x => x.Condition)
				.FirstOrDefault(x => x.Id == resultItem.Id);
			Assert.IsNotNull(creatureTemplate);

			var user = _dbContext.Users.FirstOrDefault(x => x.Id == creatureTemplate.CreatedByUserId);
			Assert.IsNotNull(user);
			Assert.IsTrue(user.Name.Contains(request.UserName));

			Assert.IsNotNull(creatureTemplate.CreatureTemplateParts
				.Any(x => x.BodyPartTypeId == request.BodyPartTypeId));
			Assert.IsNotNull(creatureTemplate.Abilities
				.Any(a => a.AppliedConditions.Any(ac => ac.Condition.Name.Contains(request.ConditionName))));
		}
	}
}
