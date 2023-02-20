﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.AbilityRequests;
using Sindie.ApiService.Core.Contracts.AbilityRequests.UpdateAppliedCondition;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.AbilityRequests;
using Sindie.ApiService.Core.Requests.AbilityRequests.UpdateAppliedCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.AbilityRequests
{
	/// <summary>
	/// Тест для <see cref="CreateDefensiveSkillHandler"/>
	/// </summary>
	[TestClass]
	public class CreateDefensiveSkillHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Ability _ability;

		/// <summary>
		/// Конструктор для теста <see cref="CreateDefensiveSkillHandler"/>
		/// </summary>
		public CreateDefensiveSkillHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_ability = Ability.CreateForTest(game: _game);
			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _ability));
		}

		/// <summary>
		/// Тест метода Handle - создание защитного навыка
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreateDefensiveSkill_ShouldReturnUnit()
		{
			var command = new CreateDefensiveSkillCommand
			{
				GameId = _game.Id,
				AbilityId = _ability.Id,
				Skill = Enums.Skill.Sword
			};

			var newHandler = new CreateDefensiveSkillHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(command, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.Abilities.Count());
			var ability = _dbContext.Abilities.FirstOrDefault();

			Assert.IsNotNull(ability.DefensiveSkills);
			var defensiveSkill = ability.DefensiveSkills.FirstOrDefault(s => s.Skill == Enums.Skill.Sword);
			Assert.IsNotNull(defensiveSkill);
		}
	}
}