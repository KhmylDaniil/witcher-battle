using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.SkillRequests.ChangeSkill;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.SkillRequests
{
	/// <summary>
	/// Тест для <see cref="ChangeSkillHandler"/>
	/// </summary>
	[TestClass]
	public class ChangeSkillHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Skill _parameter;

		/// <summary>
		/// Тест для <see cref="ChangeSkillHandler"/>
		/// </summary>
		public ChangeSkillHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_parameter = Skill.CreateForTest(
				name: "old",
				game: _game,
				description: "oldDescription",
				statName: "Ref",
				minValueSkill: 1,
				maxValueSkill: 2);
			_dbContext = CreateInMemoryContext(
				x => x.AddRange(_game, _parameter));
		}

		/// <summary>
		/// Тест метода Handle - изменение навыка
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreateParameter_ShouldReturnUnit()
		{
			var request = new ChangeSkillCommand(
				id: _parameter.Id,
				gameId: _game.Id,
				name: "Melee",
				description: "description",
				statName: "Ref",
				minValueSkill: 0,
				maxValueSkill: 10);

			var newHandler = new ChangeSkillHandler(_dbContext, AuthorizationService.Object);

			var result = newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			var parameter = _dbContext.Skills.FirstOrDefault();
			Assert.IsNotNull(parameter);

			Assert.AreEqual(parameter.Name, "Melee");
			Assert.AreEqual(parameter.Description, "description");
			Assert.AreEqual(parameter.StatName, "Ref");
			Assert.AreEqual(parameter.SkillBounds.MinValueSkill, 0);
			Assert.AreEqual(parameter.SkillBounds.MaxValueSkill, 10);
		}



	}
}
