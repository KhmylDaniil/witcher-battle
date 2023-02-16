using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.AbilityRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.AbilityRequests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.AbilityRequests
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
				GameId = _game.Id,
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