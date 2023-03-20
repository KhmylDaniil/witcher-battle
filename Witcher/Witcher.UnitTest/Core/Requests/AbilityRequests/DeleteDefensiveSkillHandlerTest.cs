using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.AbilityRequests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Witcher.UnitTest.Core.Requests.AbilityRequests
{
	/// <summary>
	/// Тест для <see cref="DeleteDefensiveSkillHandler"/>
	/// </summary>
	[TestClass]
	public class DeleteDefensiveSkillHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Ability _ability;
		private readonly DefensiveSkill _defensiveSkill;

		/// <summary>
		/// Конструктор для теста <see cref="DeleteDefensiveSkillHandler"/>
		/// </summary>
		public DeleteDefensiveSkillHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_ability = Ability.CreateForTest(game: _game);
			_defensiveSkill = new(_ability.Id, Enums.Skill.Sword);

			_ability.DefensiveSkills.Add(_defensiveSkill);

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _ability));
		}

		/// <summary>
		/// Тест метода Handle - удаление защитного навыка
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreateDefensiveSkill_ShouldReturnUnit()
		{
			var command = new DeleteDefensiveSkillCommand
			{
				AbilityId = _ability.Id,
				Id = _defensiveSkill.Id
			};

			var newHandler = new DeleteDefensiveSkillHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(command, default);

			Assert.IsNotNull(result);
			Assert.IsTrue(!_dbContext.Abilities.Any(a => a.DefensiveSkills.Any(ds => ds.Id == _defensiveSkill.Id)));
		}
	}
}