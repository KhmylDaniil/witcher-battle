using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests;
using System.Linq;
using System.Threading.Tasks;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.UnitTest.Core.Requests.CreatureTemplatesRequests
{
	[TestClass]
	public class UpdateCreatureTemplateSkillHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly CreatureTemplateSkill _skill;

		public UpdateCreatureTemplateSkillHandlerTest() : base()
		{
			_game = Game.CreateForTest();

			_creatureTemplate = CreatureTemplate.CreateForTest(
				game: _game,
				bodyTemplate: BodyTemplate.CreateForTest(game: _game));

			_skill = new CreatureTemplateSkill(
				creatureTemplate: _creatureTemplate,
				skill: Skill.Awareness,
				skillValue: 5);

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_creatureTemplate,
				_skill));
		}

		/// <summary>
		/// Тест метода Handle - изменение навыка шаблона существа
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_UpdateCreatureTemplateSkill_AddCreatureTemplateSkill()
		{
			var request = new UpdateCreatureTemplateSkillCommand()
			{
				GameId = _game.Id,
				CreatureTemplateId = _creatureTemplate.Id,
				Skill = Skill.Dodge,
				Value = 5
			};

			var newHandler = new UpdateCreatureTemplateSkillHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.CreatureTemplates.Count());
			var creatureTemplate = _dbContext.CreatureTemplates.FirstOrDefault();
			Assert.IsNotNull(creatureTemplate.CreatureTemplateSkills);

			var skill = creatureTemplate.CreatureTemplateSkills.FirstOrDefault(x => x.Skill == Skill.Dodge);
			Assert.IsNotNull(skill);
			Assert.AreEqual(5, skill.SkillValue);
		}

		/// <summary>
		/// Тест метода Handle - изменение навыка шаблона существа
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_UpdateCreatureTemplateSkill_UpdateCreatureTemplateSkill()
		{
			var request = new UpdateCreatureTemplateSkillCommand()
			{
				GameId = _game.Id,
				CreatureTemplateId = _creatureTemplate.Id,
				Id = _skill.Id,
				Skill = Skill.Awareness,
				Value = 7
			};

			var newHandler = new UpdateCreatureTemplateSkillHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.CreatureTemplates.Count());
			var creatureTemplate = _dbContext.CreatureTemplates.FirstOrDefault();
			Assert.IsNotNull(creatureTemplate.CreatureTemplateSkills);

			var skill = creatureTemplate.CreatureTemplateSkills.FirstOrDefault(x => x.Id == _skill.Id);
			Assert.IsNotNull(skill);
			Assert.AreEqual(Skill.Awareness, skill.Skill);
			Assert.AreEqual(7, skill.SkillValue);

			//can`t change skill, only skill value
			var request2 = new UpdateCreatureTemplateSkillCommand()
			{
				GameId = _game.Id,
				CreatureTemplateId = _creatureTemplate.Id,
				Id = _skill.Id,
				Skill = Skill.Business,
				Value = 7
			};

			var result2 = await newHandler.Handle(request2, default);

			Assert.IsNotNull(result2);
			Assert.AreEqual(1, _dbContext.CreatureTemplates.Count());
			creatureTemplate = _dbContext.CreatureTemplates.FirstOrDefault();
			Assert.IsNotNull(creatureTemplate.CreatureTemplateSkills);

			skill = creatureTemplate.CreatureTemplateSkills.FirstOrDefault(x => x.Id == _skill.Id);
			Assert.IsNotNull(skill);
			Assert.AreEqual(Skill.Awareness, skill.Skill);
			Assert.AreEqual(7, skill.SkillValue);
		}
	}
}
