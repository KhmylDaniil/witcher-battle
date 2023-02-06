using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.AbilityRequests.CreateAbility;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.AbilityRequests.CreateAbility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.UnitTest.Core.Requests.AbilityRequests
{
	/// <summary>
	/// Тест для <see cref="CreateAbilityHandler"/>
	/// </summary>
	[TestClass]
	public class CreateAbilityHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;

		/// <summary>
		/// Конструктор для теста <see cref="CreateAbilityHandler"/>
		/// </summary>
		public CreateAbilityHandlerTest() : base()
		{
			_game = Game.CreateForTest();

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game));
		}

		/// <summary>
		/// Тест метода Handle - создание шаблона тела
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreateAbility_ShouldReturnUnit()
		{
			var request = new CreateAbilityCommand(
				gameId: _game.Id,
				name: "name",
				description: "description",
				attackDiceQuantity: 2,
				damageModifier: 4,
				attackSpeed: 1,
				accuracy: 1,
				attackSkill: Skill.Melee,
				defensiveSkills: new List<Skill> { Skill.Melee },
				damageType: DamageType.Slashing,
				appliedConditions: new List<CreateAbilityRequestAppliedCondition>
				{
					new CreateAbilityRequestAppliedCondition()
					{
						Condition = Condition.Bleed,
						ApplyChance = 50
					}
				});

			var newHandler = new CreateAbilityHandler(_dbContext, AuthorizationService.Object);

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
			Assert.AreEqual(ability.AttackSkill, Skill.Melee);

			Assert.IsNotNull(ability.DefensiveSkills);
			Assert.AreEqual(ability.DefensiveSkills.Count, 1);
			var defensiveSkill = ability.DefensiveSkills.First();
			Assert.AreEqual(Skill.Melee, defensiveSkill.Skill);
			Assert.AreEqual(DamageType.Slashing, ability.DamageType);

			Assert.IsNotNull(ability.AppliedConditions);
			Assert.AreEqual(ability.AppliedConditions.Count, 1);
			var appliedCondition = ability.AppliedConditions.First();
			Assert.AreEqual(Condition.Bleed, appliedCondition.Condition);
			Assert.AreEqual(appliedCondition.ApplyChance, 50);
		}
	}
}
