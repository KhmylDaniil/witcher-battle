using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.AbilityRequests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.UnitTest.Core.Requests.AbilityRequests
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
			var request = new CreateAbilityCommand
			{
				Name = "name",
				Description = "description",
				AttackDiceQuantity = 2,
				DamageModifier = 4,
				AttackSpeed = 1,
				Accuracy = 1,
				AttackSkill = Skill.Melee,
				DefensiveSkills = new List<Skill> { Skill.Melee },
				DamageType = DamageType.Slashing,
				AppliedConditions = new List<UpdateAttackFormulaCommandItemAppledCondition>
				{
					new UpdateAttackFormulaCommandItemAppledCondition()
					{
						Condition = Condition.Bleed,
						ApplyChance = 50
					}
				}
			};

			var newHandler = new CreateAbilityHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, _dbContext.Abilities.Count());
			var ability = _dbContext.Abilities.FirstOrDefault(x => x.Id == result);

			Assert.AreEqual(_game.Id, ability.GameId);
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
