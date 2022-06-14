using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplateById;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.GetCreatureTemplateById;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.CreatureTemplatesRequests
{
	/// <summary>
	/// Тест для <see cref="GetCreatureTemplateByIdHandler"/>
	/// </summary>
	[TestClass]
	public class GetCreatureTemplateByIdHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly BodyTemplate _bodyTemplate;
		private readonly Game _game;
		private readonly Condition _condition;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly Ability _ability;
		private readonly Parameter _parameter;
		private readonly CreatureTemplateParameter _creatureTemplateParameter;

		/// <summary>
		/// Конструктор для теста <see cref="GetCreatureTemplateByIdHandler"/>
		/// </summary>
		public GetCreatureTemplateByIdHandlerTest() : base()
		{

			_game = Game.CreateForTest();

			_parameter = Parameter.CreateForTest(game: _game);
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game, name: "human");
			_condition = Condition.CreateForTest(
				game: _game,
				name: Conditions.BleedName);

			_creatureTemplate = CreatureTemplate.CreateForTest(
				name: "testName",
				game: _game,
				type: "type",
				bodyTemplate: _bodyTemplate);

			_creatureTemplateParameter = CreatureTemplateParameter.CreateForTest(
				creatureTemplate: _creatureTemplate,
				parameter: _parameter,
				value: 5);

			_creatureTemplate.CreatureTemplateParameters.Add(_creatureTemplateParameter);

			_ability = Ability.CreateForTest(
				creatureTemplate: _creatureTemplate,
				attackParameter: _parameter);
			_ability.AppliedConditions.Add(new AppliedCondition(
				ability: _ability,
				condition: _condition,
				applyChance: 100));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_bodyTemplate,
				_condition,
				_creatureTemplate,
				_ability));
		}

		/// <summary>
		/// Тест метода Handle получение шаблона существа по айди
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_GetCreatureTemplateById_ShouldReturn_GetCreatureTemplateByIdResponse()
		{
			var request = new GetCreatureTemplateByIdQuery()
			{
				GameId = _game.Id,
				Id = _creatureTemplate.Id
			};
			var newHandler = new GetCreatureTemplateByIdHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.AreEqual(_creatureTemplate.GameId, result.GameId);
			Assert.AreEqual(result.ImgFileId, _creatureTemplate.ImgFileId);
			Assert.AreEqual(result.BodyTemplateId, _bodyTemplate.Id);
			Assert.AreEqual(result.Name, "testName");
			Assert.AreEqual(result.Type, "type");
			Assert.AreEqual(result.Int, _creatureTemplate.Int);
			Assert.AreEqual(result.Ref, _creatureTemplate.Ref);
			Assert.AreEqual(result.Dex, _creatureTemplate.Dex);
			Assert.AreEqual(result.Body, _creatureTemplate.Body);
			Assert.AreEqual(result.Emp, _creatureTemplate.Emp);
			Assert.AreEqual(result.Cra, _creatureTemplate.Cra);
			Assert.AreEqual(result.Emp, _creatureTemplate.Emp);
			Assert.AreEqual(result.Will, _creatureTemplate.Will);
			Assert.AreEqual(result.Speed, _creatureTemplate.Speed);
			Assert.AreEqual(result.Luck, _creatureTemplate.Luck);
			Assert.AreEqual(result.CreatedByUserId, _creatureTemplate.CreatedByUserId);
			Assert.AreEqual(result.ModifiedByUserId, _creatureTemplate.ModifiedByUserId);
			Assert.AreEqual(result.CreatedOn, _creatureTemplate.CreatedOn);
			Assert.AreEqual(result.ModifiedOn, _creatureTemplate.ModifiedOn);

			Assert.IsNotNull(result.BodyParts);
			var bodyPart = result.BodyParts.First();
			Assert.IsNotNull(bodyPart);
			Assert.AreEqual(bodyPart.Name, "torso");
			Assert.AreEqual(bodyPart.HitPenalty, 1);
			Assert.AreEqual(bodyPart.DamageModifier, 1);
			Assert.AreEqual(bodyPart.MinToHit, 1);
			Assert.AreEqual(bodyPart.MaxToHit, 10);
			Assert.AreEqual(bodyPart.Armor, 5);

			Assert.IsNotNull(result.CreatureTemplateParameters);
			var creatureTemplateParameter = result.CreatureTemplateParameters.First();
			Assert.IsNotNull(creatureTemplateParameter);
			Assert.AreEqual(creatureTemplateParameter.Id, _creatureTemplateParameter.Id);
			Assert.AreEqual(creatureTemplateParameter.ParameterName, _parameter.Name);
			Assert.AreEqual(creatureTemplateParameter.ParameterId, _parameter.Id);
			Assert.AreEqual(creatureTemplateParameter.ParameterValue, 5);

			Assert.IsNotNull(result.Abilities);
			var ability = result.Abilities.First();
			Assert.IsNotNull(ability);
			Assert.AreEqual(ability.Name, _ability.Name);
			Assert.AreEqual(ability.Id, _ability.Id);
			Assert.AreEqual(ability.AttackParameterName, _parameter.Name);
			Assert.AreEqual(ability.Description, _ability.Description);
			Assert.AreEqual(ability.Accuracy, _ability.Accuracy);
			Assert.AreEqual(ability.AttackParameterId, _parameter.Id);
			Assert.AreEqual(ability.AttackDiceQuantity, _ability.AttackDiceQuantity);
			Assert.AreEqual(ability.DamageModifier, _ability.DamageModifier);
			Assert.AreEqual(ability.AttackSpeed, _ability.AttackSpeed);

			Assert.IsNotNull(ability.AppliedConditions);
			var appliedCondition = ability.AppliedConditions.First();
			Assert.IsNotNull(appliedCondition);
			Assert.AreEqual(appliedCondition.ConditionName, _condition.Name);
			Assert.AreEqual(appliedCondition.ConditionId, _condition.Id);
			Assert.AreEqual(appliedCondition.ApplyChance, 100);
		}
	}
}
