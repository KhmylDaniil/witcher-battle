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
	public class DeleteCreatureTemplateSkillHandlerTest : UnitTestBase
 	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly CreatureTemplateSkill _skill;

		public DeleteCreatureTemplateSkillHandlerTest() : base()
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
		/// Тест метода Handle - удаление навыка шаблона существа
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_DeleteCreatureTemplateSkill()
		{
			var request = new DeleteCreatureTemplateSkillCommand()
			{
				CreatureTemplateId = _creatureTemplate.Id,
				Id = _skill.Id
			};

			var newHandler = new DeleteCreatureTemplateSkillHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.CreatureTemplates.Count());
			var creatureTemplate = _dbContext.CreatureTemplates.FirstOrDefault();
			Assert.IsNotNull(creatureTemplate.CreatureTemplateSkills);

			var skill = creatureTemplate.CreatureTemplateSkills.FirstOrDefault(x => x.Id == _skill.Id);
			Assert.IsNull(skill);
		}
	}
}
