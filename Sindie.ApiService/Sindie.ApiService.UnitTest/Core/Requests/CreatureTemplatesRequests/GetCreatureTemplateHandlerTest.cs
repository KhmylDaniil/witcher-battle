using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.GetCreatureTemplate;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Sindie.ApiService.Core.BaseData.Enums;

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
		private readonly CreatureTemplate _creatureTemplate;
		private readonly CreatureTemplatePart _creatureTemplatePart;
		private readonly Ability _ability;

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

			_bodyTemplate = BodyTemplate.CreateForTest(game: _game, name: "human");

			_creatureTemplate = CreatureTemplate.CreateForTest(
				name: "testName",
				game: _game,
				creatureType: CreatureType.Human,
				bodyTemplate: _bodyTemplate,
				createdOn: DateTimeProvider.Object.TimeProvider,
				modifiedOn: DateTimeProvider.Object.TimeProvider,
				createdByUserId: _user.Id);

			_creatureTemplatePart = CreatureTemplatePart.CreateForTest(
				creatureTemplate: _creatureTemplate,
				bodyPartType: BodyPartType.Void,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 1,
				maxToHit: 1,
				armor: 0);

			_creatureTemplate.CreatureTemplateParts.Add(_creatureTemplatePart);

			_ability = Ability.CreateForTest(
				game: _game,
				attackSkill: Skill.Melee);
			_ability.AppliedConditions.Add(new AppliedCondition(
				ability: _ability,
				condition: Condition.Bleed,
				applyChance: 100));
			_creatureTemplate.Abilities.Add(_ability);
				
			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_user,
				_game,
				_bodyTemplate,
				_creatureTemplate,
				_ability));
		}

		/// <summary>
		/// Тест метода Handle получение списка шаблонов существа с фильтрами
		/// по названию, дате создания/изменения, создавшему пользователю, названию, названию шаблона тела, названию части тела, названию применяемого состояния
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_GetCreatureTemplate_ShouldReturn_GetCreatureTemplateResponse()
		{
			var creationMinTime = DateTimeProvider.Object.TimeProvider.AddDays(-1);
			var creationMaxTime = DateTimeProvider.Object.TimeProvider.AddDays(1);
			var modificationMinTime = DateTimeProvider.Object.TimeProvider.AddDays(-1);
			var modificationMaxTime = DateTimeProvider.Object.TimeProvider.AddDays(1);

			var request = new GetCreatureTemplateQuery()
			{
				GameId = _game.Id,
				Name = "testName",
				UserName = "Author",
				CreatureType = "Human",
				CreationMinTime = creationMinTime,
				CreationMaxTime = creationMaxTime,
				ModificationMinTime = modificationMinTime,
				ModificationMaxTime = modificationMaxTime,
				BodyTemplateName = "human",
				BodyPartType = "Void",
				ConditionName = CritNames.GetConditionFullName(Condition.Bleed),
				PageSize = 2,
				PageNumber = 1,
			};

			var newHandler = new GetCreatureTemplateHandler(_dbContext, AuthorizationService.Object, DateTimeProvider.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.TotalCount);

			var resultItem = result.CreatureTemplatesList.First();
			Assert.IsTrue(resultItem.Name.Contains(request.Name));
			Assert.IsTrue(Enum.GetName(resultItem.CreatureType).Contains(request.CreatureType));
			Assert.IsTrue(resultItem.BodyTemplateName.Contains(request.BodyTemplateName));
			Assert.IsTrue(resultItem.CreatedOn >= creationMinTime && resultItem.CreatedOn <= creationMaxTime);
			Assert.IsTrue(resultItem.ModifiedOn >= modificationMinTime && resultItem.ModifiedOn <= modificationMaxTime);

			var creatureTemplate = _dbContext.CreatureTemplates
				.Include(x => x.CreatureTemplateParts)
				.Include(x => x.Abilities)
					.ThenInclude(x => x.AppliedConditions)
				.FirstOrDefault(x => x.Id == resultItem.Id);
			Assert.IsNotNull(creatureTemplate);

			var user = _dbContext.Users.FirstOrDefault(x => x.Id == creatureTemplate.CreatedByUserId);
			Assert.IsNotNull(user);
			Assert.IsTrue(user.Name.Contains(request.UserName));

			Assert.IsNotNull(creatureTemplate.CreatureTemplateParts
				.Any(x => Enum.GetName(x.BodyPartType).Contains(request.BodyPartType)));
			Assert.IsNotNull(creatureTemplate.Abilities
				.Any(a => a.AppliedConditions.Any(ac => Enum.GetName(ac.Condition).Contains(request.ConditionName))));
		}
	}
}
