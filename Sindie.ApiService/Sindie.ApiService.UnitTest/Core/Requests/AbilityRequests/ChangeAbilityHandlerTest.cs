using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.AbilityRequests.ChangeAbility;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.AbilityRequests.ChangeAbility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.AbilityRequests
{
	/// <summary>
	/// Тест для <see cref="ChangeAbilityHandler"/>
	/// </summary>
	[TestClass]
	public class ChangeAbilityHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly DamageType _damageType;
		private readonly Condition _condition;
		private readonly Game _game;
		private readonly Skill _parameter1;
		private readonly Skill _parameter2;
		private readonly Ability _ability;

		/// <summary>
		/// Конструктор для теста <see cref="ChangeAbilityHandler"/>
		/// </summary>
		public ChangeAbilityHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_condition = Condition.CreateForTest();
			_damageType = DamageType.CreateForTest();
			_parameter1 = Skill.CreateForTest(game: _game);
			_parameter2 = Skill.CreateForTest(game: _game);
			_ability = Ability.CreateForTest(game: _game, attackSkill: _parameter1);

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _condition, _damageType, _ability, _parameter1, _parameter2));
		}

		/// <summary>
		/// Тест метода Handle - создание шаблона тела
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_ChangeAbility_ShouldReturnUnit()
		{
			var request = new ChangeAbilityCommand(
				id: _ability.Id,
				gameId: _game.Id,
				name: "newName",
				description: "newDescription",
				attackDiceQuantity: 2,
				damageModifier: 4,
				attackSpeed: 1,
				accuracy: 1,
				attackSkillId: _parameter2.Id,
				defensiveSkills: new List<Guid> { _parameter2.Id },
				damageTypes: new List<Guid> { _damageType.Id },
				appliedConditions: new List<ChangeAbilityRequestAppliedCondition>
				{
					new ChangeAbilityRequestAppliedCondition()
					{
						ConditionId = _condition.Id,
						ApplyChance = 50
					}
				});

			var newHandler = new ChangeAbilityHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.Abilities.Count());
			var ability = _dbContext.Abilities.FirstOrDefault();

			Assert.AreEqual(request.GameId, ability.GameId);
			Assert.IsNotNull(ability.Name);
			Assert.AreEqual(request.Name, ability.Name);
			Assert.AreEqual(request.Description, ability.Description);
			Assert.AreEqual(ability.AttackDiceQuantity, 2);
			Assert.AreEqual(ability.DamageModifier, 4);
			Assert.AreEqual(ability.AttackSpeed, 1);
			Assert.AreEqual(ability.Accuracy, 1);
			Assert.AreEqual(ability.AttackSkillId, _parameter2.Id);

			Assert.IsNotNull(ability.DefensiveSkills);
			Assert.AreEqual(ability.DefensiveSkills.Count, 1);
			var defensiveParameter = ability.DefensiveSkills.First();
			Assert.AreEqual(_parameter2.Id, defensiveParameter.Id);

			Assert.IsNotNull(ability.DamageTypes);
			Assert.AreEqual(ability.DamageTypes.Count, 1);
			var damageType = ability.DamageTypes.First();
			Assert.AreEqual(_damageType.Id, damageType.Id);

			Assert.IsNotNull(ability.AppliedConditions);
			Assert.AreEqual(ability.AppliedConditions.Count, 1);
			var appliedCondition = ability.AppliedConditions.First();
			Assert.AreEqual(_condition.Id, appliedCondition.ConditionId);
			Assert.AreEqual(appliedCondition.ApplyChance, 50);
		}
	}
}
