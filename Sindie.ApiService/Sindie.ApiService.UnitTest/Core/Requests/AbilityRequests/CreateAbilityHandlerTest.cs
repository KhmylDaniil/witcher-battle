﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.AbilityRequests.CreateAbility;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.AbilityRequests.CreateAbility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.AbilityRequests
{
	/// <summary>
	/// Тест для <see cref="CreateAbilityHandler"/>
	/// </summary>
	[TestClass]
	public class CreateAbilityHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly DamageType _damageType;
		private readonly Condition _condition;
		private readonly Game _game;
		private readonly Parameter _parameter;

		/// <summary>
		/// Конструктор для теста <see cref="CreateAbilityHandler"/>
		/// </summary>
		public CreateAbilityHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_condition = Condition.CreateForTest();
			_damageType = DamageType.CreateForTest();
			_parameter = Parameter.CreateForTest(game: _game);

			_dbContext = CreateInMemoryContext(x => x.AddRange(_game, _condition, _damageType, _parameter));
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
				attackParameterId: _parameter.Id,
				defensiveParameters: new List<Guid> { _parameter.Id },
				damageTypes: new List<Guid> { _damageType.Id },
				appliedConditions: new List<CreateAbilityRequestAppliedCondition>
				{
					new CreateAbilityRequestAppliedCondition()
					{
						ConditionId = _condition.Id,
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
			Assert.AreEqual(ability.AttackParameterId, _parameter.Id);

			Assert.IsNotNull(ability.DefensiveParameters);
			Assert.AreEqual(ability.DefensiveParameters.Count, 1);
			var defensiveParameter = ability.DefensiveParameters.First();
			Assert.AreEqual(_parameter.Id, defensiveParameter.Id);

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
