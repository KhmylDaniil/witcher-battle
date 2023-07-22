using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.CreatureTemplateRequests;
using System.Linq;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.UnitTest.Core.Requests.CreatureTemplatesRequests
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

			var skill = creatureTemplate.CreatureTemplateSkills.Find(x => x.Id == _skill.Id);
			Assert.IsNull(skill);
		}
	}
}
